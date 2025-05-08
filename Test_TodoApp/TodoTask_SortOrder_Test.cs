using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TodoApp.Data;
using TodoApp.Models;

namespace Test_TodoApp;


public class TodoTask_SortOrder_Test : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly AppDbContext db;
    private readonly HttpClient _client;

    public TodoTask_SortOrder_Test(WebApplicationFactory<Program> factory)
    {
        db = TestDbHelper.CreateDbContext();
        _client = factory.CreateClient();
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

        var result = await TodoTask.GetSortedTasksAsync(db, TaskOrderBy.SortOrder);

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

        var result = await TodoTask.GetSortedTasksAsync(db, TaskOrderBy.SortOrder);

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
    public async Task 並び替えAPIにIDリストを渡すとタスクのSortOrderが更新される()
    {
        using var transaction = db.Database.BeginTransaction();

        var tasks = new[]
        {
            TodoTask.Create("Task A","Body A"),
            TodoTask.Create("Task B","Body B"),
            TodoTask.Create("Task C","Body C"),
        };

        tasks[0].SortOrder = 1;
        tasks[1].SortOrder = 2;
        tasks[2].SortOrder = 3;

        db.TodoTasks.AddRange(tasks);
        db.SaveChanges();

        // 並び替えAPIの呼び出し
        var reorderRequest = new List<int> { tasks[2].Id, tasks[0].Id, tasks[1].Id };
        var response = await _client.PostAsJsonAsync("/api/tasks/reorder", reorderRequest);

        response.EnsureSuccessStatusCode();

        var sortTasks = await TodoTask.GetSortedTasksAsync(db, TaskOrderBy.SortOrder);

        Assert.Equal(reorderRequest[0], sortTasks[0].Id);
        Assert.Equal(reorderRequest[1], sortTasks[1].Id);
        Assert.Equal(reorderRequest[2], sortTasks[2].Id);

        transaction.Rollback();

    }

}
