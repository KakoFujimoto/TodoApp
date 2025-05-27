using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoApp.Controllers;
using TodoApp.Data;
using TodoApp.DTO;
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

        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].SortOrder = i + 1;
        }

        db.TodoTasks.AddRange(tasks);
        db.SaveChanges();

        var taskService = new TaskService(db);
        var controller = new TasksController(taskService);

        var move1 = new ReOrderRequestDto
        {
            TaskId = tasks[2].Id,
            NewIndex = 0
        };

        var result1 = await controller.ReOrderTasks(move1);
        Assert.IsType<OkResult>(result1);

        var move2 = new ReOrderRequestDto
        {
            TaskId = tasks[0].Id,
            NewIndex = 1
        };

        var result2 = await controller.ReOrderTasks(move2);
    
        Assert.IsType<OkResult>(result2);

        var sortedTasks = await TodoTask.GetSortedTasksAsync(db, TaskOrderBy.SortOrder, false);


        Assert.Equal(tasks[0].Id, sortedTasks[1].Id); //A
        Assert.Equal(tasks[1].Id, sortedTasks[2].Id); //B
        Assert.Equal(tasks[2].Id, sortedTasks[0].Id); //C

        transaction.Rollback();

    }

    [Fact]
    public async Task Reorder_タスクを指定位置に移動できる()
    {
        using var transaction = db.Database.BeginTransaction();

        var tasks = new[]
        {
            TodoTask.Create("Task A","Body A"),
            TodoTask.Create("Task B","Body B"),
            TodoTask.Create("Task C","Body C"),
            TodoTask.Create("Task D","Body D"),
            TodoTask.Create("Task E","Body E"),
            TodoTask.Create("Task F","Body F"),
            TodoTask.Create("Task G","Body G"),
        };

        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].SortOrder = i + 1;
        }

        db.TodoTasks.AddRange(tasks);
        db.SaveChanges();

        var taskService = new TaskService(db);
        var controller = new TasksController(taskService);

        var reorderDto = new ReOrderRequestDto
        {
            TaskId = tasks[6].Id,
            NewIndex = 5
        };

        var result = await controller.ReOrderTasks(reorderDto);

        Assert.IsType<OkResult>(result);

        var sortTasks = await TodoTask.GetSortedTasksAsync(db, TaskOrderBy.SortOrder, false);

        Assert.Equal(tasks[0].Id, sortTasks[0].Id);
        Assert.Equal(tasks[1].Id, sortTasks[1].Id);
        Assert.Equal(tasks[2].Id, sortTasks[2].Id);
        Assert.Equal(tasks[3].Id, sortTasks[3].Id);
        Assert.Equal(tasks[4].Id, sortTasks[4].Id);
        Assert.Equal(tasks[5].Id, sortTasks[6].Id);
        Assert.Equal(tasks[6].Id, sortTasks[5].Id);

        transaction.Rollback();

    }
}