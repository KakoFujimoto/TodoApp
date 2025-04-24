using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using TodoApp.Data;
using TodoApp.Models;

namespace Test_TodoApp;

public class UnitTest1 : IDisposable
{
    static UnitTest1()
    {
        AppDbContext db;
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TodoAppDb-test;Trusted_Connection=True;");
        db = new AppDbContext(builder.Options);

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

    }
    private readonly AppDbContext db;
    public UnitTest1()
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TodoAppDb-test;Trusted_Connection=True;");
        db = new AppDbContext(builder.Options);
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
        var ids = tasks.Select(t => t.Id).ToList();

        Assert.Equal(new List<int> { 3, 2, 1 }, ids);

        transaction.Rollback();

    }


}
