namespace TodoApp.Common
{
    public static class ErrorMessages
    {
        private static readonly Dictionary<ErrorCode, string> _messages = new()
        {
            { ErrorCode.TaskNotFound, "指定されたタスクの一部が見つかりませんでした"},
            { ErrorCode.UpdateOrderFailed, "順序保存に失敗しました"},
            { ErrorCode.EmptyTaskList, "タスクリストが空です"},
            { ErrorCode.UnknownError, "不明なエラーが発生しました"},
            { ErrorCode.InvalidSortOrder, "無効な並び順が指定されました" }
        };

        public static ErrorMessage Get(ErrorCode code)
        {
            return new ErrorMessage(code, _messages.GetValueOrDefault(code, "未定義のエラーです"));
        }
    }

    public record ErrorMessage(ErrorCode Code, string Message); 
}