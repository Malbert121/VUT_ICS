using ICS.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ICS.DAL.Factories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SchoolContext>
{
    private readonly DbContextSqLiteFactory _dbContextSqLiteFactory = new("SchoolApp.db");

    public SchoolContext CreateDbContext(string[] args) => _dbContextSqLiteFactory.CreateDbContext();
}