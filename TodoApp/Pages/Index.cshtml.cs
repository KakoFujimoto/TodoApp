using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<TodoTask> Tasks { get; set; } = new();
        [BindProperty]
        public string NewTask { get; set; } = string.Empty;

        public void OnGet()
        {
            Tasks = _context.TodoTasks.ToList();
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrWhiteSpace(NewTask))
            {
                _context.TodoTasks.Add(new TodoTask { Title = NewTask });
                _context.SaveChanges();
            }

            return RedirectToPage(); // リダイレクトで再表示
        }
    }
}
