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
        /// <returns>HTTP 200 OKまたはエラー</returns>
        [HttpPost("reorder")]
        public async Task<IActionResult> ReOrderTasks([FromBody] List<int> taskIds)
        {
            if (taskIds == null || !taskIds.Any())
            {
                var error = ErrorMessages.Get(ErrorCode.UpdateOrderFailed);
                Console.Error.WriteLine(error.Message);
                return BadRequest(error);
            }

            var result = await _taskService.ReOrderTaskAsync(taskIds);

            if (!result.Success)
            {
                var errorCode = result.ErrorCode ?? ErrorCode.UnknownError;
                var error = ErrorMessages.Get(errorCode);

                Console.Error.WriteLine(error.Message);
                return StatusCode(500, error);
            }

            return Ok();
        }
    }
}