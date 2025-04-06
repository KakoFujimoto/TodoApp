using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace TodoApp.Pages
{
    public class IndexModel : PageModel
    {
        public List<string> Tasks { get; set; } = new List<string>(); // タスクリスト
        public string NewTask { get; set; }  // 新しいタスク

        public void OnGet()
        {
            // 初期データとしてタスクリストを作成
            Tasks = new List<string>
            {
                "買い物に行く",
                "メールを返信する",
                "散歩をする"
            };
        }

        // タスクを追加する処理
        public void OnPost()
        {
            if (!string.IsNullOrEmpty(NewTask))
            {
                Tasks.Add(NewTask);  // 新しいタスクをリストに追加
                NewTask = string.Empty;  // フォームをクリア
            }
        }
    }
}
