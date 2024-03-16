using ICS.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.UnitOfWork;

public record UnitOfWorkFactory(IDbContextFactory<SchoolContext> dbContextFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create() => new UnitOfWork(dbContextFactory.CreateDbContext());
}