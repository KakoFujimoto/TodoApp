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

        [BindProperty]
        public IndexPageFormData FormData { get; set; } = new();

        // 並び順セレクトボックス用のプロパティ
        [BindProperty(SupportsGet = true)]
        public string SelectedOrderBy { get; set; } = "Id";

        [BindProperty(SupportsGet = true)]
        public bool SelectedDescending { get; set; } = true;


        /// <summary>
        /// GETリクエスト時に呼ばれ、DBからタスクリストを読み込む
        /// </summary>
        public async Task OnGetAsync()
        {
            Console.WriteLine($"SelectedOrderBy : {SelectedOrderBy}");
            Console.WriteLine($"SelectedDescending : {SelectedDescending}");

            TaskOrderBy orderBy = Enum.TryParse<TaskOrderBy>(SelectedOrderBy, out var parsedOrderBy) ? parsedOrderBy : TaskOrderBy.Id;
            Tasks = await TodoTask.GetSortedTasksAsync(_context, orderBy, SelectedDescending, isCompleted: false);
        }

        /// <summary>
        /// POSTリクエスト時に呼ばれ、新しいタスクをDBに保存する
        /// 入力されたタイトルが空でない場合のみ追加する
        /// </summary>
        /// <returns>リダイレクトしてページを再表示</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // 追加するデータの定義、準備
                var todoTask = TodoTask.Create(FormData.Title, FormData.Body, FormData.Priority);
                // DBへの保存
                await todoTask.SaveAsync(_context);
                return RedirectToPage();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCompleteAsync(int id)
        {
            var task = await _context.TodoTasks.FindAsync(id);
            if (task != null)
            {
                task.SetCompleted();
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
