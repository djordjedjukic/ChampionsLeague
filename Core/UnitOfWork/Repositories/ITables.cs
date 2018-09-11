using System.Collections.Generic;

namespace Core.UnitOfWork.Repositories
{
    using Core.Entities;

    public interface ITables : IRepository<Table>
    {
        IEnumerable<Table> Get(string[] groups);
    }
}
