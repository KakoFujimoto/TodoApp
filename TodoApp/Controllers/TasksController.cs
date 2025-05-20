using Microsoft.AspNetCore.Mvc;
using TodoApp.Services;
using TodoApp.Common;

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
                Console.Error.WriteLine(ErrorMessages.UpdateOrderFailed.Message);
                return StatusCode(500, ErrorMessages.UpdateOrderFailed);
            }

            var result = await _taskService.ReOrderTaskAsync(taskIds);

            if (!result.Success)
            {
                var error = result.ErrorCode switch
                {
                    "TaskNotFound" => ErrorMessages.TaskNotFound,
                    _ => ErrorMessages.UnknownError
                };

                Console.Error.WriteLine(error.Message);
                return StatusCode(500, error);
            }

            return Ok();
        }
    }
}