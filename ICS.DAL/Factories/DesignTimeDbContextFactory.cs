using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CookBook.DAL.Factories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CookBookDbContext>
{
    private readonly DbContextSqLiteFactory _dbContextSqLiteFactory = new("cookbook.db");

    public CookBookDbContext CreateDbContext(string[] args) => _dbContextSqLiteFactory.CreateDbContext();
}