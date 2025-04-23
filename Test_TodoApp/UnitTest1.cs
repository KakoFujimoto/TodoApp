using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace Test_TodoApp;

public class UnitTest1
{
    private static readonly AppDbContext db;

    static UnitTest1()
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TodoAppDb-test;Trusted_Connection=True;");
        db = new AppDbContext(builder.Options);

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

    }

    public static AppDbContext GetContext() => db;

    [Fact]
    public void Test1()
    {
        using var transaction = db.Database.BeginTransaction();

        db.Add(TodoTask.Create("aa", "sss"));
        db.SaveChanges();

        var item = db.TodoTasks.First();
        Assert.Equal("aa", item.Title);

        transaction.Rollback();
    }


    [Fact]
    public void Test2()
    {
        using var transaction = db.Database.BeginTransaction();

        db.Add(TodoTask.Create("bb", "sss"));
        db.SaveChanges();

        var item = db.TodoTasks.First();
        Assert.Equal("bb", item.Title);

        transaction.Rollback();
    }
}
