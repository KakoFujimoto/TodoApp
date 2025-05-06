using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;
using TodoApp.Pages;

namespace Test_TodoApp;

public class IndexPage_Test : IDisposable
{
    private readonly AppDbContext db;
    public IndexPage_Test()
    {
        db = TestDbHelper.CreateDbContext();
    }

    public void Dispose()
    {
        db.Dispose();
    }

    [Fact]
    public async Task OnGetAsync_Ok_Test()
    {
        using var transaction = db.Database.BeginTransaction();

        db.TodoTasks.AddRange(new[]
        {
            TodoTask.Create("Task A", "Body A"),
            TodoTask.Create("Task B", "Body B"),
            TodoTask.Create("Task C", "Body C"),
        });

        await db.SaveChangesAsync();

        var pageModel = new IndexModel(db);

        pageModel.SelectedOrderBy = TaskOrderBy.Id;
        pageModel.SelectedDescending = true;

        await pageModel.OnGetAsync();

        var ids = pageModel.Tasks.Select(t => t.Id).ToList();

        for (int i = 0; i < ids.Count - 1; i++)
        {
            Assert.True(ids[i] > ids[i + 1]);
        }

        transaction.Rollback();
    }

}
