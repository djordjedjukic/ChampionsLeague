namespace Core.UnitOfWork
{
    using System;
    using Core.UnitOfWork.Repositories;

    public interface IUnitOfWork : IDisposable
    {
        IMatches Matches { get; }

        ITables Tables { get; }

        void Commit();  
        
        void Clear();
    }
}
