namespace UnitOfWork.InMemory.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Core.Entities;

    public class EntityStorage<T> : IEntityStorage<T> where T : Entity
    {
        protected readonly List<T> storage = new List<T>();

        public void Add(T entity)
        {
            this.storage.Add(entity.Clone());
        }

        public void AddRange(IEnumerable<T> entities)
        {
            this.storage.AddRange(entities.Clone());
        }

        public void Remove(Guid id)
        {
            T existingEntity = this.storage.SingleOrDefault(x => x.Id == id);

            if (existingEntity != null)
                this.storage.Remove(existingEntity);
        }

        public void Update(T entity)
        {
            T existingEntity = this.storage.SingleOrDefault(x => x.Id == entity.Id);

            if (existingEntity != null)
            {
                int index = this.storage.FindIndex(i => i.Id == entity.Id);
                this.storage.RemoveAt(index);
                this.storage.Insert(index, entity.Clone());
            }
        }

        public (List<T> entities, int totalCount) All(int pageNum, int pageSize, Func<T, bool> filter = null)
        {
            var filteredStorage = filter == null ? this.storage : this.storage.Where(filter);

            List<T> entities = filteredStorage
                .Skip(pageNum * pageSize).Take(pageSize)
                .ToList()
                .Clone();

            return (entities, filteredStorage.ToList().Count);
        }

        public List<T> All(Func<T, bool> filter = null)
        {
            if (filter == null)
                return this.storage.Clone();
            else
                return this.storage.Where(filter).ToList().Clone();
        }

        public T SingleOrDefault(Func<T, bool> func)
        {
            return this.storage.SingleOrDefault(func).Clone();
        }

        public bool Any()
        {
            return this.storage.Any();
        }
    }
}
