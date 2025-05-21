using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Common;


namespace TodoApp.Services
{
    public class TaskService
    {
        private readonly AppDbContext _db;

        public TaskService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// タスクの並び替えを実行する
        /// </summary>
        /// <param name="taslIds">新しい順序のタスクIdリスト</param>
        /// <returns>成功したかどうか</returns>
        public async Task<ServiceResult> ReOrderTaskAsync(List<int> taskIds)
        {
            var tasks = await _db.TodoTasks.Where(t => taskIds.Contains(t.Id)).ToListAsync();

            if (tasks.Count != taskIds.Count)
            {
                var error = ErrorMessages.Get(ErrorCode.TaskNotFound);
                return ServiceResult.Fail(error.Code, error.Message);

            }

            for (int i = 0; i < taskIds.Count; i++)
            {
                var task = tasks.First(t => t.Id == taskIds[i]);
                task.SortOrder = i + 1;
            }

            await _db.SaveChangesAsync();
            return ServiceResult.Ok();
        }
    }
}