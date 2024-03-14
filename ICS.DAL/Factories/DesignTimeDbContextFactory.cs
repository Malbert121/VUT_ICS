using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ICS.DAL.Factories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CookBookDbContext>
{
    private readonly DbContextSqLiteFactory _dbContextSqLiteFactory = new("SchoolApp.db");

    public CookBookDbContext CreateDbContext(string[] args) => _dbContextSqLiteFactory.CreateDbContext();
}