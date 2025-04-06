using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace TodoApp.Pages
{
    public class IndexModel : PageModel
    {
        public List<string> Tasks { get; set; } = new List<string>(); // プロパティを初期化

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
    }
}