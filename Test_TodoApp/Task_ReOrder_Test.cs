using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using TodoApp.Data;
using TodoApp.Models;

namespace Test_TodoApp;

public class Task_ReOrder_Test : IDisposable
{
    private readonly AppDbContext db;
    private readonly HttpClient _client;

    public Task_ReOrder_Test()
    {
        var appFactory = new WebApplicationFactory<Program>();
        _client = appFactory.CreateClient();

        // TaskControllerがサーバに認識されているかの確認
        var dummyTaskIds = new List<int> { 1, 2, 3 };
        var dummyResponse = _client.PostAsJsonAsync("/api/tasks/reorder", dummyTaskIds).Result;
        Console.WriteLine($"POST /api/tasks/reorder => Status Code:{dummyResponse.StatusCode}");


        db = TestDbHelper.CreateDbContext();
    }
    public void Dispose()
    {
        db.Dispose();
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