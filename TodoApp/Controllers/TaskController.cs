using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;


namespace TodoApp.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TasksController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// タスクの並び替えを行うAPI
        /// </summary>
        /// <param name="taskIds">並び順に並べたタスクIDリスト</param>
        /// <return>HTTP 200 OK</return>
        [HttpPost("reorder")]
        public async Task<IActionResult> ReOrderTasks([FromBody] List<int> taskIds)
        {
            if (taskIds == null || !taskIds.Any())
            {
                return BadRequest("タスクリストが空です");
            }

            var tasks = await _db.TodoTasks.Where(t => taskIds.Contains(t.Id)).ToListAsync();

            if (tasks.Count != taskIds.Count)
            {
                return NotFound("指定されたタスクの一部が見つかりませんでした");
            }

            // 並び順に応じてSortOrderを更新
            for (int i = 0; i < taskIds.Count; i++)
            {
                var task = tasks.First(t => t.Id == taskIds[i]);
                task.SortOrder = i + 1;
            }

            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}