namespace UnitOfWork.InMemory.Infrastructure
{
    using System.Collections.Generic;
    using Core.Entities;

    internal class Storage
    {
        private Dictionary<string, object> EntityStorage { get; set; } = new Dictionary<string, object>();

        public void Clear()
        {
            this.EntityStorage = new Dictionary<string, object>();
        }

        public IEntityStorage<T> GetStorage<T>() where T : Entity
        {
            string typeName = typeof(T).Name;

            if (this.EntityStorage.ContainsKey(typeName))
            {
                return this.EntityStorage[typeName] as IEntityStorage<T>;
            }
            else
            {
                var storage = new EntityStorage<T>();
                this.EntityStorage.Add(typeName, storage);
                return storage;
            }
        }
    }
}
