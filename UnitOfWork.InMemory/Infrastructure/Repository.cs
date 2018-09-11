namespace UnitOfWork.InMemory.Infrastructure
{
    using System;
    using System.Collections.Generic;

    using Core.UnitOfWork;
    using Core.Entities;

    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        public T WithId(Guid id)
        {
            return StorageHolder.Storage.GetStorage<T>().SingleOrDefault(x => x.Id == id);
        }

        public void Update(T entity)
        {
            StorageHolder.Storage.GetStorage<T>().Update(entity);
        }

        public void Add(T entity)
        {
            StorageHolder.Storage.GetStorage<T>().Add(entity);
        }

        public void Add(IEnumerable<T> entities)
        {
            StorageHolder.Storage.GetStorage<T>().AddRange(entities);
        }

        public T SingleOrDefault(Func<T, bool> func)
        {
            return StorageHolder.Storage.GetStorage<T>().SingleOrDefault(func);
        }

        public (List<T> page, int totalCount) All(int pageNum, int pageSize)
        {
            return StorageHolder.Storage.GetStorage<T>().All(pageNum, pageSize);
        }

        public (List<T> page, int totalCount) All(int pageNum, int pageSize, Func<T, bool> filter)
        {
            return StorageHolder.Storage.GetStorage<T>().All(pageNum, pageSize, filter);
        }

        public List<T> All(Func<T, bool> filter)
        {
            return StorageHolder.Storage.GetStorage<T>().All(filter);
        }

        public List<T> All()
        {
            return StorageHolder.Storage.GetStorage<T>().All();
        }

        public bool Any()
        {
            return StorageHolder.Storage.GetStorage<T>().Any();
        }

        public void Remove(Guid id)
        {
            StorageHolder.Storage.GetStorage<T>().Remove(id);
        }
    }
}
