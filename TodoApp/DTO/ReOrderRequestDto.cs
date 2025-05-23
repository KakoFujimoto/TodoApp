using System.ComponentModel.DataAnnotations;

namespace TodoApp.DTO
{
    /// <summary>
    /// タスク並び替えリクエスト用のDTO
    /// </summary>

    public class ReOrderRequestDto
    {
        [Required]
        public int TaskId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "新しいインデックスは0以上で指定してください")]
        public int NewIndex { get; set; }
    }
}