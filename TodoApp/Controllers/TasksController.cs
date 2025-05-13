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

            var result = await _taskService.ReOrderTaskAsync(taskIds);

            if (!result.Success)
            {
                return result.ErrorCode switch
                {
                    "TaskNotFound" => NotFound(result.ErrorMessage),
                    _ => StatusCode(500, result.ErrorMessage ?? "不明なエラーが発生しました")
                };
            }
            return Ok();
        }
    }
}