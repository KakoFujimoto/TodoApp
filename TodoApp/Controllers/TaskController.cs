using Microsoft.AspNetCore.Mvc;
using TodoApp.Services;


namespace TodoApp.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// タスクの並び替えを行うAPI
        /// </summary>
        /// <param name="taskIds">並び順に並べたタスクIDリスト</param>
        /// <return>HTTP 200 OKまたはエラー</return>
        [HttpPost("reorder")]
        public async Task<IActionResult> ReOrderTasks([FromBody] List<int> taskIds)
        {
            if (taskIds == null || !taskIds.Any())
            {
                return BadRequest("タスクリストが空です");
            }

            bool success = await _taskService.ReOrderTaskAsync(taskIds);

            if (!success)
            {
                return NotFound("指定されたタスクの一部が見つかりませんでした");
            }

            return Ok();
        }
    }
}