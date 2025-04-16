using System.ComponentModel.DataAnnotations;
using TodoApp.Data;
using Microsoft.EntityFrameworkCore;


namespace TodoApp.Models
{
    /// <summary>
    /// Todoアプリで扱うタスクのデータを保持するモデル
    /// </summary>
    public class TodoTask
    {
        /// <summary>
        /// タスクの一意な識別子
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// タスクのタイトル
        /// </summary>
        [Required(ErrorMessage = "タイトルは必須です")]
        [MaxLength(100, ErrorMessage = "タイトルは100文字以内で入力してください")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// タスクの詳細または説明
        /// </summary>
        [Required(ErrorMessage = "本文は必須です")]
        [MaxLength(1000, ErrorMessage = "本文は1000文字以内で入力してください")]
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private TodoTask() { }

        /// <summary>
        /// 追加するデータの定義、準備
        /// </summary>
        public static TodoTask Create(string title, string body)
        {
            return new TodoTask
            {
                Title = title,
                Body = body
            };
        }

        /// <summary>
        /// DBへの保存
        /// </summary>
        public async Task SaveAsync(AppDbContext context)
        {
            await context.Set<TodoTask>().AddAsync(this);
            await context.SaveChangesAsync();
        }


        /// <summary>
        /// 並び順を指定してDBからタスク一覧を取得
        /// </summary>
        public static async Task<List<TodoTask>> GetSortedTasksAsync(AppDbContext context, string orderBy = "Id", bool descending = true)
        {
            var query = context.TodoTasks.AsQueryable();

            switch (orderBy)
            {
                case "Id":
                    query = descending ? query.OrderByDescending(task => task.Id) : query.OrderBy(task => task.Id);
                    break;
                case "Title":
                    query = descending ? query.OrderByDescending(task => task.Title) : query.OrderBy(task => task.Title);
                    break;
                case "Body":
                    query = descending ? query.OrderByDescending(task => task.Body) : query.OrderBy(task => task.Body);
                    break;
                default:
                    query = descending ? query.OrderByDescending(task => task.Id) : query.OrderBy(task => task.Id);
                    break;
            }

            return await query.ToListAsync();
        }
    }

}
