using System.ComponentModel.DataAnnotations;
using TodoApp.Models;
using TodoApp.Validation;

namespace TodoApp.Pages
{
    /// <summary>
    /// Indexページのフォーム入力データを保持するDTOクラス
    /// </summary>
    public class IndexPageFormData
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "タイトルは必須です")]
        [StringLength(100, ErrorMessage = "タイトルは100文字以内で入力してください")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "本文は必須です")]
        [StringLength(1000, ErrorMessage = "本文は1000文字以内で入力してください")]
        public string Body { get; set; } = string.Empty;

        [PriorityValidation(ErrorMessage = "優先度を選択してください")]
        public Priority Priority { get; set; } = Priority.None;

    }
}