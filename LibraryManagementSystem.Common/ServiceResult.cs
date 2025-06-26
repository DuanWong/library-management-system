namespace LibraryManagementSystem.Common
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Field { get; set; }

        public static ServiceResult Ok(string message = "Operation successful", string field = "")
        {
            return new ServiceResult { Success = true, Message = message, Field = field };
        }

        public static ServiceResult Fail(string message, string field = "")
        {
            return new ServiceResult { Success = false, Message = message, Field = field };
        }
    }
}
