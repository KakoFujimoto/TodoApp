using System.ComponentModel.DataAnnotations;
using System.Net;

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
        /// 安全に改行をHTMLとして表示するためのプロパティ
        /// </summary>
        public string FormattedBody
        {
            get
            {
                var encoded = WebUtility.HtmlEncode(Body ?? "");
                return encoded.Replace("\r\n", "<br>").Replace("\n","<br>");
            }
        }

    }
}
