namespace TodoApp.Common
{
    public static class ErrorMessages
    {
        public static readonly ErrorMessage TaskNotFound = new("TaskNotFound", "指定されたタスクの一部が見つかりませんでした");

    }
    public record ErrorMessage(string Code, string Message);
}