namespace UnitOfWork.InMemory
{
    using Core.UnitOfWork;
    using Core.UnitOfWork.Repositories;
    using UnitOfWork.InMemory.Infrastructure;
    using UnitOfWork.InMemory.Repositories;

    public sealed class InMemoryUnitOfWork : IUnitOfWork
    {
        private bool commitExecuted = false;

        public InMemoryUnitOfWork()
        {
            StorageHolder.CreateBackup();

            this.Matches = new Matches();
            this.Tables = new Tables();
        }

        public void Dispose()
        {
            if (!this.commitExecuted)
                StorageHolder.RestoreBackup();
        }

        public IMatches Matches { get; }

        public ITables Tables { get; }

        public void Commit()
        {
            this.commitExecuted = true;
        }

        public void Clear()
        {
            StorageHolder.Storage.Clear();
        }
    }
}
