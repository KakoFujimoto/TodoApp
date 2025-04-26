using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Data;

namespace TodoApp.Pages
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EditPageFormData FormData { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var task = await _context.TodoTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            FormData = new EditPageFormData
            {
                Id = task.Id,
                Title = task.Title,
                Body = task.Body,
                Priority = task.Priority
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var taskInDb = await _context.TodoTasks.FindAsync(FormData.Id);

            if (taskInDb == null)
            {
                return NotFound();
            }

            taskInDb.Update(FormData.Title, FormData.Body);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var taskToDelete = await _context.TodoTasks.FindAsync(FormData.Id);
            if (taskToDelete == null)
            {
                return NotFound();
            }

            await taskToDelete.DeleteAsync(_context);
            return RedirectToPage("/Index");
        }
    }
}
