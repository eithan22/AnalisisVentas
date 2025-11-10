
namespace AnalisisVentas.Application.Result
{
    
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

       
        public static ServiceResult SuccessResult(string message) 
        {
            return new ServiceResult { IsSuccess = true, Message = message };
        }

        public static ServiceResult ErrorResult(string message) 
        {
            return new ServiceResult { IsSuccess = false, Message = message };
        }
    }

  
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }

       

        /*
        public static ServiceResult<T> SuccessResult(T data, string message = "Datos obtenidos") 
        {
            return new ServiceResult<T> { IsSuccess = true, Data = data, Message = message };
        }

        public static ServiceResult<T> ErrorResult(string message) 
        {
            return new ServiceResult<T> { IsSuccess = false, Data = default(T), Message = message };
        }
        */
    }
}