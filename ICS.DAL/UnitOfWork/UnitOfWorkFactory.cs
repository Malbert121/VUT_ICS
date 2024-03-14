using Microsoft.EntityFrameworkCore;

namespace ICS.DAL.UnitOfWork;

public class UnitOfWorkFactory(IDbContextFactory<CookBookDbContext> dbContextFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create() => new UnitOfWork(dbContextFactory.CreateDbContext());
}