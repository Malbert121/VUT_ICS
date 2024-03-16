using ICS.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.UnitOfWork;

public class UnitOfWorkFactory(IDbContextFactory<SchoolContext> dbContextFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create() => new UnitOfWork(dbContextFactory.CreateDbContext());
}