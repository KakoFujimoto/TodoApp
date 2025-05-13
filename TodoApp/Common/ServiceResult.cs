namespace TodoApp.Common
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string? ErrorCode { get; set; }

        public string? ErrorMessage { get; set; }

        public static ServiceResult Ok()
        {
            return new ServiceResult { Success = true };
        }

        public static ServiceResult Fail(string code, string message)
        {
            return new ServiceResult
            {
                Success = false,
                ErrorCode = code,
                ErrorMessage = message
            };
        }
    }
}