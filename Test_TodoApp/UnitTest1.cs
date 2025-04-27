using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace Test_TodoApp;

public class UnitTest1 : IDisposable
{   
    private static AppDbContext CreateDbContext()
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TodoAppDb-test;Trusted_Connection=True;");
        var options = builder.Options;

        return new AppDbContext(options);
    }
    static UnitTest1()
    {
        var db = CreateDbContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

    }
    private readonly AppDbContext db;
    public UnitTest1()
    {
        db = CreateDbContext();
    }

    public void Dispose()
    {
        db.Dispose();
    }


    [Fact]
    public void Create_Test01()
    {
        using var transaction = db.Database.BeginTransaction();

        db.Add(TodoTask.Create("aa", "sss"));
        db.SaveChanges();

        var item = db.TodoTasks.First();
        Assert.Equal("aa", item.Title);

        transaction.Rollback();

    }


    [Fact]
    public void Create_Test02()
    {
        using var transaction = db.Database.BeginTransaction();

        db.Add(TodoTask.Create("bb", "sss"));
        db.SaveChanges();

        var item = db.TodoTasks.First();
        Assert.Equal("bb", item.Title);

        transaction.Rollback();
    }

    [Fact]
    public async Task SaveAsync_Test()
    {
        using var transaction = db.Database.BeginTransaction();

        var task = TodoTask.Create("title", "body");
        await task.SaveAsync(db);

        var firstItem = db.TodoTasks.First();
        Assert.Equal("title", firstItem.Title);
        Assert.Equal("body", firstItem.Body);

        transaction.Rollback();
    }

    [Fact]

    public async Task GetSortedTasksAsync_Test()
    {
        using var transaction = db.Database.BeginTransaction();

        db.TodoTasks.AddRange(new[]
        {
            TodoTask.Create("First", "A"),
            TodoTask.Create("Second", "B"),
            TodoTask.Create("Third", "C")
        });
        db.SaveChanges();

        var tasks = await TodoTask.GetSortedTasksAsync(db);
        var titles = tasks.Select(t => t.Title).ToList();

        Assert.Equal(new List<string> { "Third", "Second", "First" }, titles);

        transaction.Rollback();

    }


}
