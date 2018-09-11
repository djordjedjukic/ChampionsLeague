namespace UnitOfWork.InMemory.Infrastructure
{
    using System;

    internal static class StorageHolder
    {
        private static Storage backup;

        private static Lazy<Storage> LazyStorage { get; set; } =
            new Lazy<Storage>(() => new Storage());

        public static void RestoreBackup()
        {
            LazyStorage = new Lazy<Storage>(() => backup);
        }

        public static void CreateBackup()
        {
            backup = LazyStorage.Value.Clone();
        }

        internal static Storage Storage => LazyStorage.Value;
    }
}
