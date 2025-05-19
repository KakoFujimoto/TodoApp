using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;

namespace TodoApp.ViewComponents
{
    public class TaskCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(TodoTask task)
        {
            return View(task);
        }
    }
}
