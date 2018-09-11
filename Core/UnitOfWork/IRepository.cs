namespace Core.UnitOfWork
{    
    using System;
    using System.Collections.Generic;
    using Core.Entities;

    public interface IRepository<T> where T : Entity
    {
        T WithId(Guid id);

        void Update(T entity);

        void Add(T entity);

        void Add(IEnumerable<T> entities);

        (List<T> page, int totalCount) All(int pageNum, int pageSize);

        void Remove(Guid id);

        bool Any();
    }
}
