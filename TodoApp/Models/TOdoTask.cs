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
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// タスクの詳細または説明
        /// </summary>
        public string Body { get; set; } = string.Empty;
    }
}
