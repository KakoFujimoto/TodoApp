using System.ComponentModel.DataAnnotations;

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
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// タスクの詳細または説明
        /// </summary>
        [Required]
        [MaxLength(1000)]
        public string Body { get; set; } = string.Empty;
    }
}
