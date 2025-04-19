using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Pages
{
    public class ArchiveModel : PageModel
    {
        private readonly AppDbContext _context;

        public ArchiveModel(AppDbContext context)
        {
            _context = context;
        }

        public List<TodoTask> ArchivedTasks { get; set; } = new();

        public async Task OnGetAsync()
        {
            ArchivedTasks = await TodoTask.GetSortedTasksAsync(_context, TaskOrderBy.Id, descending: true, isCompleted: true);
        }
    }
}
