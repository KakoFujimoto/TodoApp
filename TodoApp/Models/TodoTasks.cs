using System.ComponentModel.DataAnnotations;
using TodoApp.Data;
using Microsoft.EntityFrameworkCore;
using TodoApp.Common;

namespace TodoApp.Models
{
    public enum TaskOrderBy
    {
        None,
        Id,
        Priority,
        // Title,
        // Body
        SortOrder,
        DueDate,
        Category
    }

    public enum Priority
    {
        None = 0,
        Urgent = 1,
        Normal = 2,
        Low = 3
    }

    public enum CategoryType
    {
        None = 0,
        Work = 1,
        Private = 2
    }

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
        /// タスクの完了状態を持つフラグ
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// タスクの優先度
        /// </summary>
        public Priority Priority { get; set; } = Priority.Normal;

        /// <summary>
        /// タスクの表示順序を表す
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// タスクの期日
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// タスクのカテゴリ
        /// </summary>
        public CategoryType Category { get; set; } = CategoryType.None;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        private TodoTask() { }

        /// <summary>
        /// 追加するデータの定義、準備
        /// </summary>
        public static TodoTask Create(
            string title,
            string body,
            Priority priority = Priority.Normal,
            DateTime? dueDate = null,
            CategoryType category = CategoryType.None)
        {
            if (dueDate.HasValue && dueDate.Value.Date < DateTime.Now.Date)
            {
                throw new InvalidOperationException("期日には過去の日付を指定できません。");
            }
            return new TodoTask
            {
                Title = title,
                Body = body,
                Priority = priority,
                DueDate = dueDate,
                Category = category
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
        public static async Task<List<TodoTask>> GetSortedTasksAsync(
            AppDbContext context,
            TaskOrderBy orderBy = TaskOrderBy.SortOrder,
            bool descending = false,
            bool? isCompleted = null)
        {
            IQueryable<TodoTask> query = context.TodoTasks;

            if (isCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == isCompleted.Value);
            }

            query = (orderBy, descending) switch
            {
                (TaskOrderBy.Id, true) => query.OrderByDescending(t => t.Id),
                (TaskOrderBy.Id, false) => query.OrderBy(t => t.Id),

                (TaskOrderBy.Priority, true) => query.OrderByDescending(t => t.Priority),
                (TaskOrderBy.Priority, false) => query.OrderBy(t => t.Priority),

                (TaskOrderBy.SortOrder, true) => query.OrderByDescending(t => t.SortOrder),
                (TaskOrderBy.SortOrder, false) => query.OrderBy(t => t.SortOrder),

                (TaskOrderBy.DueDate, true) => query.OrderByDescending(t => t.DueDate),
                (TaskOrderBy.DueDate, false) => query.OrderBy(t => t.DueDate),

                (TaskOrderBy.Category, true) => query.OrderByDescending(t => t.Category),
                (TaskOrderBy.Category, false) => query.OrderBy(t => t.Category),
                _ => query.OrderByDescending(t => t.SortOrder)
            };

            return await query.ToListAsync();
        }

        /// <summary>
        /// DBから削除
        /// </summary>
        public async Task DeleteAsync(AppDbContext context)
        {
            context.TodoTasks.Remove(this);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// 外部から与えられたデータで更新する
        /// </summary>
        public void Update(
            string title,
            string body,
            Priority priority,
            DateTime? dueDate,
            CategoryType category)
        {
            Title = title;
            Body = body;
            Priority = priority;
            DueDate = dueDate;
            Category = category;
        }

        /// <summary>
        /// 選択したタスクを完了状態にする
        /// </summary>
        public void SetCompleted()
        {
            this.IsCompleted = true;
        }
    }
}
