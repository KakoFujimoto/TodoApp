using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoApp.Controllers;
using TodoApp.Data;
using TodoApp.Models;
using TodoApp.Services;

namespace Test_TodoApp;

public class Task_ReOrder_Test : IDisposable
{
    private readonly AppDbContext db;
    private readonly HttpClient _client;

    public Task_ReOrder_Test()
    {
        var appFactory = new WebApplicationFactory<Program>();
        _client = appFactory.CreateClient();
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

        var taskService = new TaskService(db);

        var controller = new TasksController(taskService);

        var result = await controller.ReOrderTasks(reorderRequest);

        Assert.IsType<OkResult>(result);

        var sortTasks = await TodoTask.GetSortedTasksAsync(db, TaskOrderBy.SortOrder,false);

        Assert.Equal(reorderRequest[0], sortTasks[0].Id);
        Assert.Equal(reorderRequest[1], sortTasks[1].Id);
        Assert.Equal(reorderRequest[2], sortTasks[2].Id);

        transaction.Rollback();

    }
}