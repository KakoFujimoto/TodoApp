using Microsoft.EntityFrameworkCore;
using TodoApp.Data;

namespace Test_TodoApp;

public static class TestDbHelper
{
    private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=TodoAppDb-test;Trusted_Connection=True;";


    public static AppDbContext CreateDbContext()
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer(ConnectionString);
        return new AppDbContext(builder.Options);
    }

    static TestDbHelper()
    {
        using var db = CreateDbContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
}
