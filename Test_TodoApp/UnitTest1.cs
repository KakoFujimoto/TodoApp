using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace Test_TodoApp;

public class UnitTest1
{
    private static AppDbContext db;

    public static void Initialize()
    {
        if (db != null) { return; }

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TodoAppDb-test;Trusted_Connection=True;");
        db = new AppDbContext(builder.Options);

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }

    public UnitTest1()
    {
        Initialize();
    }

    public static AppDbContext GetContext() => db;

    [Fact]
    public void Test1()
    {
        db.Add(TodoTask.Create("aa","sss"));
        db.SaveChanges();

        var item = db.TodoTasks.First();
        Assert.Equal("aa", item.Title);
    }


    [Fact]
    public void Test2()
    {
        db.Add(TodoTask.Create("bb","sss"));
        db.SaveChanges();

        var item = db.TodoTasks.First();
        Assert.Equal("bb", item.Title);
    }
}
