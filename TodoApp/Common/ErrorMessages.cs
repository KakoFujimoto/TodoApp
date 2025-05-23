namespace TodoApp.Common
{
    public static class ErrorMessages
    {
        public static readonly ErrorMessage TaskNotFound = new("TaskNotFound", "指定されたタスクの一部が見つかりませんでした");

        public static readonly ErrorMessage UpdateOrderFailed = new("UpdateOrderFailed", "順序保存に失敗しました");

        public static readonly ErrorMessage UnknownError = new("UnknownError", "不明なエラーが発生しました");

        public static readonly ErrorMessage InvalidSortOrder = new("InvalidSortOrder", "無効な並び順が指定されました");

    }
    public record ErrorMessage(string Code, string Message);
}