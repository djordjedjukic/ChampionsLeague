namespace UnitOfWork.InMemory.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using UnitOfWork.InMemory.Infrastructure.ArrayExtensions;

    public static class Extensions
    {
        private static readonly MethodInfo CloneMethod
            = typeof(Object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

        private static bool IsPrimitive(this Type type)
        {
            if (type == typeof(String)) return true;
            return type.IsValueType && type.IsPrimitive;
        }

        private static Object Clone(this Object originalObject)
        {
            return InternalClone(originalObject,
                new Dictionary<Object, Object>(new ReferenceEqualityComparer()));
        }

        private static Object InternalClone(Object originalObject, IDictionary<Object, Object> visited)
        {
            if (originalObject == null) return null;
            var typeToReflect = originalObject.GetType();
            if (IsPrimitive(typeToReflect)) return originalObject;
            if (visited.ContainsKey(originalObject)) return visited[originalObject];
            if (typeof(Delegate).IsAssignableFrom(typeToReflect)) return null;
            var cloneObject = CloneMethod.Invoke(originalObject, null);
            if (typeToReflect.IsArray)
            {
                var arrayType = typeToReflect.GetElementType();
                if (IsPrimitive(arrayType) == false)
                {
                    Array clonedArray = (Array)cloneObject;
                    clonedArray.ForEach((array, indices) => array.SetValue(InternalClone(clonedArray.GetValue(indices), visited), indices));
                }
            }
            visited.Add(originalObject, cloneObject);
            CopyFields(originalObject, visited, cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
            return cloneObject;
        }

        private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        {
            if (typeToReflect.BaseType != null)
            {
                RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
            }
        }

        private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        {
            foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
            {
                if (filter != null && filter(fieldInfo) == false) continue;
                if (IsPrimitive(fieldInfo.FieldType)) continue;
                var originalFieldValue = fieldInfo.GetValue(originalObject);
                var clonedFieldValue = InternalClone(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }

        public static T Clone<T>(this T original)
        {
            return (T)Clone((Object)original);
        }
    }

    public class ReferenceEqualityComparer : EqualityComparer<object>
    {
        public override bool Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }

        public override int GetHashCode(object obj)
        {
            if (obj == null) return 0;
            return obj.GetHashCode();
        }
    }

    namespace ArrayExtensions
    {
        public static class ArrayExtensions
        {
            public static void ForEach(this Array array, Action<Array, int[]> action)
            {
                if (array.LongLength == 0) return;
                ArrayTraverse walker = new ArrayTraverse(array);
                do action(array, walker.Position);
                while (walker.Step());
            }
        }

        internal class ArrayTraverse
        {
            public readonly int[] Position;
            private readonly int[] maxLengths;

            public ArrayTraverse(Array array)
            {
                this.maxLengths = new int[array.Rank];
                for (int i = 0; i < array.Rank; ++i)
                {
                    this.maxLengths[i] = array.GetLength(i) - 1;
                }
                this.Position = new int[array.Rank];
            }

            public bool Step()
            {
                for (int i = 0; i < this.Position.Length; ++i)
                {
                    if (this.Position[i] < this.maxLengths[i])
                    {
                        this.Position[i]++;
                        for (int j = 0; j < i; j++)
                        {
                            this.Position[j] = 0;
                        }
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
