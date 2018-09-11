namespace UnitOfWork.InMemory.Infrastructure
{
    using System;
    using System.Collections.Generic;

    using Core.Entities;

    public interface IEntityStorage<T> where T : Entity
    {
        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        (List<T> entities, int totalCount) All(int pageNum, int pageSize, Func<T, bool> filter = null);

        List<T> All(Func<T, bool> filter = null);

        bool Any();

        void Remove(Guid id);

        T SingleOrDefault(Func<T, bool> func);

        void Update(T entity);
    }
}
