using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace Test_TodoApp;

public class TodoTask_Test : IDisposable
{
    private readonly AppDbContext db;

    public TodoTask_Test()
    {
        db = TestDbHelper.CreateDbContext();
    }

    public void Dispose()
    {
        db.Dispose();
    }


    [Fact]
    public void Create_Ok_Test()
    {
        using var transaction = db.Database.BeginTransaction();

        db.Add(TodoTask.Create("aa", "sss"));
        db.SaveChanges();

        var item = db.TodoTasks.First();
        Assert.Equal("aa", item.Title);

        transaction.Rollback();

    }

    [Fact]
    public async Task SaveAsync_Ok_Test()
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

    public async Task GetSortedTasksAsync_Ok_Test()
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


    [Fact]

    public async Task DeleteAsync_Ok_Test()
    {
        using var transaction = db.Database.BeginTransaction();

        var task = TodoTask.Create("ToDelete", "Test");
        db.TodoTasks.Add(task);
        db.SaveChanges();

        var taskToDelete = await db.TodoTasks.FirstOrDefaultAsync(t => t.Title == "ToDelete");

        Assert.NotNull(taskToDelete);

        await taskToDelete.DeleteAsync(db);

        var deletedTask = await db.TodoTasks.FirstOrDefaultAsync(t => t.Title == "ToDelete");
        Assert.Null(deletedTask);

        transaction.Rollback();
    }

    [Fact]
    public void Update_Ok_Test()
    {
        var task = TodoTask.Create("Old Title", "Old Body");

        task.Update("New Title", "New Body", Priority.Urgent);

        Assert.Equal("New Title", task.Title);
        Assert.Equal("New Body", task.Body);
        Assert.Equal(Priority.Urgent, task.Priority);
    }

    [Fact]
    public void SetCompleted_Ok_Test()
    {
        var task = TodoTask.Create("Test Task", "Test Body");

        Assert.False(task.IsCompleted);

        task.SetCompleted();

        Assert.True(task.IsCompleted);
    }

}
