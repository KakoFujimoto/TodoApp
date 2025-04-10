using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Pages
{
    /// <summary>
    /// Todoアプリのメインページ用のページモデル
    /// タスクの一覧表示と新規タスクの追加を処理する
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        /// <summary>
        /// IndexModelのコンストラクタ
        /// </summary>
        /// <param name="context">アプリケーションのDBコンテキスト</param>
        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 表示するTodoのタスクのリスト
        /// </summary>
        public List<TodoTask> Tasks { get; set; } = new();

        /// <summary>
        /// 新規タスクの詳細
        /// </summary>
        [BindProperty]
        public string NewTask { get; set; } = string.Empty;

        /// <summary>
        /// 新規タスクの詳細
        /// </summary>
        [BindProperty]
        public string NewTaskBody { get; set; } = string.Empty;

        /// <summary>
        /// GETリクエスト時に呼ばれ、DBからタスクリストを読み込む
        /// </summary>
        public void OnGet()
        {
            Tasks = _context.TodoTasks.ToList();
        }
        /// <summary>
        /// POSTリクエスト時に呼ばれ、新しいタスクをDBに保存する
        /// 入力されたタイトルが空でない場合のみ追加する
        /// </summary>
        /// <returns>リダイレクトしてページを再表示</returns>
        public IActionResult OnPost()
        {
            if (!string.IsNullOrWhiteSpace(NewTask))
            {
                _context.TodoTasks.Add(new TodoTask
                {
                    Title = NewTask,
                    Body = NewTaskBody
                });
                _context.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}
