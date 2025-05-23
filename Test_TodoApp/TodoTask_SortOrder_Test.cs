using System.Runtime.CompilerServices;
using TodoApp.Common;
using TodoApp.Data;
using TodoApp.Models;

namespace Test_TodoApp;

public class TodoTask_SortOrder_Test : IDisposable
{
    private readonly AppDbContext db;

    public TodoTask_SortOrder_Test()
    {
        db = TestDbHelper.CreateDbContext();
    }
    public void Dispose()
    {
        db.Dispose();
    }

    [Fact]
    public async Task SortOrderの昇順にならぶ()
    {
        using var transaction = db.Database.BeginTransaction();

        var tasks = new[]
        {
            TodoTask.Create("Task A", "Body A"),
            TodoTask.Create("Task B", "Body B"),
            TodoTask.Create("Task C", "Body C"),
        };

        tasks[0].SortOrder = 1;
        tasks[1].SortOrder = 3;
        tasks[2].SortOrder = 2;

        db.TodoTasks.AddRange(tasks);
        db.SaveChanges();

        var result = await TodoTask.GetSortedTasksAsync(db, TaskOrderBy.SortOrder, false);

        Assert.Equal(1, result[0].SortOrder);
        Assert.Equal(tasks[0].Title, result[0].Title);

        Assert.Equal(2, result[1].SortOrder);
        Assert.Equal(tasks[2].Title, result[1].Title);

        Assert.Equal(3, result[2].SortOrder);
        Assert.Equal(tasks[1].Title, result[2].Title);

        transaction.Rollback();
    }

    [Fact]
    public async Task タスクの4番目を2番目の前に並び替える()
    {
        using var transaction = db.Database.BeginTransaction();

        var tasks = new[]
        {
            TodoTask.Create("Task A", "Body A"),
            TodoTask.Create("Task B", "Body B"),
            TodoTask.Create("Task C", "Body C"),
            TodoTask.Create("Task D", "Body D"),
        };

        tasks[0].SortOrder = 1;
        tasks[1].SortOrder = 2;
        tasks[2].SortOrder = 3;
        tasks[3].SortOrder = 4;

        db.TodoTasks.AddRange(tasks);
        db.SaveChanges();

        // 1, 2, 3, 4 -> 1, 4, 2, 3
        await tasks[3].OrderAsync(db, 2);

        var result = await TodoTask.GetSortedTasksAsync(db, TaskOrderBy.SortOrder, false);

        Assert.Equal(1, result[0].SortOrder);
        Assert.Equal(tasks[0].Title, result[0].Title);

        Assert.Equal(2, result[1].SortOrder);
        Assert.Equal(tasks[3].Title, result[1].Title);

        Assert.Equal(3, result[2].SortOrder);
        Assert.Equal(tasks[1].Title, result[2].Title);

        Assert.Equal(4, result[3].SortOrder);
        Assert.Equal(tasks[2].Title, result[3].Title);
    }

    [Fact]
    public async Task タスクの2番目を4番目の前に並び替える()
    {
        using var transaction = db.Database.BeginTransaction();

        var tasks = new[]
        {
            TodoTask.Create("Task A", "Body A"),
            TodoTask.Create("Task B", "Body B"),
            TodoTask.Create("Task C", "Body C"),
            TodoTask.Create("Task D", "Body D"),
        };

        tasks[0].SortOrder = 1;
        tasks[1].SortOrder = 2;
        tasks[2].SortOrder = 3;
        tasks[3].SortOrder = 4;

        db.TodoTasks.AddRange(tasks);
        db.SaveChanges();

        // 1, 2, 3, 4 -> 1, 3, 4, 2
        await tasks[1].OrderAsync(db, 4);

        var result = await TodoTask.GetSortedTasksAsync(db, TaskOrderBy.SortOrder, false);

        Assert.Equal(1, result[0].SortOrder);
        Assert.Equal(tasks[0].Title, result[0].Title);

        Assert.Equal(2, result[1].SortOrder);
        Assert.Equal(tasks[2].Title, result[1].Title);

        Assert.Equal(3, result[2].SortOrder);
        Assert.Equal(tasks[3].Title, result[2].Title);

        Assert.Equal(4, result[3].SortOrder);
        Assert.Equal(tasks[1].Title, result[3].Title);
    }
    
    [Fact]
    public async Task タスクの4番目を4番目の前に並び替える()
    {
        using var transaction = db.Database.BeginTransaction();

        var tasks = new[]
        {
            TodoTask.Create("Task A", "Body A"),
            TodoTask.Create("Task B", "Body B"),
            TodoTask.Create("Task C", "Body C"),
            TodoTask.Create("Task D", "Body D"),
        };

        tasks[0].SortOrder = 1;
        tasks[1].SortOrder = 2;
        tasks[2].SortOrder = 3;
        tasks[3].SortOrder = 4;

        db.TodoTasks.AddRange(tasks);
        db.SaveChanges();

        // 1, 2, 3, 4 -> 1, 2, 3, 4
        await tasks[3].OrderAsync(db, 4);

        var result = await TodoTask.GetSortedTasksAsync(db, TaskOrderBy.SortOrder,false);

        Assert.Equal(1, result[0].SortOrder);
        Assert.Equal(tasks[0].Title, result[0].Title);

        Assert.Equal(2, result[1].SortOrder);
        Assert.Equal(tasks[1].Title, result[1].Title);

        Assert.Equal(3, result[2].SortOrder);
        Assert.Equal(tasks[2].Title, result[2].Title);

        Assert.Equal(4, result[3].SortOrder);
        Assert.Equal(tasks[3].Title, result[3].Title);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(-1, 2)]
    [InlineData(0, 4)]
    [InlineData(-1, 4)]
    public async Task SortOrderに0以下の数値が入ったときエラーになる(int 新しい並び順, int 元々の順番)
    {
        using var transaction = db.Database.BeginTransaction();

        var tasks = new[]
        {
            TodoTask.Create("Task A", "Body A"),
            TodoTask.Create("Task B", "Body B"),
            TodoTask.Create("Task C", "Body C"),
            TodoTask.Create("Task D", "Body D"),
        };

        for (int i = 0; i > tasks.Length; i++)
        {
            tasks[i].SortOrder = i + 1;
        }

        db.TodoTasks.AddRange(tasks);
        db.SaveChanges();

        var targetTask = tasks[元々の順番 - 1];
        var error = await targetTask.OrderAsync(db, 新しい並び順);

        Assert.NotNull(error);
        Assert.Equal(ErrorMessages.InvalidSortOrder.Code, error.Code);
        Assert.Equal(ErrorMessages.InvalidSortOrder.Message, error.Message);
    }

}
