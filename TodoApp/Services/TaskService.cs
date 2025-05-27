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
        public async Task<ServiceResult> ReOrderTaskAsync(int taskId, int newSortOrder)
        {
            if (newSortOrder < 0)
            {
                var error = ErrorMessages.Get(ErrorCode.InvalidSortOrder);
                return ServiceResult.Fail(error.Code, error.Message);
            }
            
            var task = await _db.TodoTasks.FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                var error = ErrorMessages.Get(ErrorCode.TaskNotFound);
                return ServiceResult.Fail(error.Code, error.Message);

            }

            var allTasks = await _db.TodoTasks.Where(t => t.Id != taskId).OrderBy(t => t.SortOrder).ToListAsync();
            allTasks.Insert(newSortOrder, task);

            for (int i = 0; i < allTasks.Count; i++)
            {
                allTasks[i].SortOrder = i + 1;
            }

            await _db.SaveChangesAsync();
            return ServiceResult.Ok();
        }
    }
}