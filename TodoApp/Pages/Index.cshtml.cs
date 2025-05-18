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
        [FromQuery]
        public TaskOrderBy? SelectedOrderBy { get; set; }

        [FromQuery]
        public bool? SelectedDescending { get; set; }


        /// <summary>
        /// GETリクエスト時に呼ばれ、DBからタスクリストを読み込む
        /// </summary>
        public async Task OnGetAsync()
        {
            Console.WriteLine($"SelectedOrderBy : {SelectedOrderBy}");
            Console.WriteLine($"SelectedDescending : {SelectedDescending}");

            TaskOrderBy orderBy = SelectedOrderBy ?? TaskOrderBy.SortOrder;
            if (orderBy == TaskOrderBy.None)
            {
                orderBy = TaskOrderBy.Id;
            }
            bool descending = SelectedDescending ?? false;
            Tasks = await TodoTask.GetSortedTasksAsync(_context, orderBy, descending, isCompleted: false);
        }

        /// <summary>
        /// POSTリクエスト時に呼ばれ、新しいタスクをDBに保存する
        /// 入力されたタイトルが空でない場合のみ追加する
        /// </summary>
        /// <returns>リダイレクトしてページを再表示</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"ModelState.IsValid = {ModelState.IsValid}");

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    var errors = entry.Value.Errors;

                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Error in '{key}:{error.ErrorMessage}");
                    }
                }
                return Page();
            }
            var todoTask = TodoTask.Create(FormData.Title, FormData.Body, FormData.Priority);
            await todoTask.SaveAsync(_context);
            return RedirectToPage();

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

        public async Task<IActionResult> OnPostUpdateOrderAsync([FromBody] List<int> order)
        {
            if (order == null || order.Count == 0)
            {
                return BadRequest("Invalid order list");
            }

            for (int i = 0; i < order.Count; i++)
            {
                var task = await _context.TodoTasks.FindAsync(order[i]);
                if (task != null)
                {
                    task.SortOrder = i;
                }
            }
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
    }
}
