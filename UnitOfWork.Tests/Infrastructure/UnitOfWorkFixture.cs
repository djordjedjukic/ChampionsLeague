[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]

namespace UnitOfWork.Tests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;

    using Core;
    using Core.UnitOfWork;

    public class UnitOfWorkFixture : IDisposable
    {
        public readonly IUnitOfWork Uow;

        public UnitOfWorkFixture(IUnitOfWork uow)
        {
            this.Uow = uow;

            this.Uow.Clear();
        }

        public void Dispose()
        {
            this.Uow.Clear();
            this.Uow.Dispose();
        }
    }
}
