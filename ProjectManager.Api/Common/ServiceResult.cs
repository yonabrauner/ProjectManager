namespace ProjectManager.Api.Common
{
    /// <summary>
    /// Standard wrapper for service-layer results.
    /// Ensures consistent success/failure handling in controllers.
    /// </summary>
    public class ServiceResult<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public T? Data { get; private set; }

        private ServiceResult() { }

        public static ServiceResult<T> Ok(T data, string message = "") =>
            new ServiceResult<T> { Success = true, Data = data, Message = message };

        public static ServiceResult<T> Fail(string message) =>
            new ServiceResult<T> { Success = false, Message = message };
    }
}