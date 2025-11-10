using System.Collections.Generic;

namespace AnalisisVentas.Application.Result
{
    // 1. Tu clase original (para métodos que no devuelven datos)
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // Métodos de ayuda
        public static ServiceResult SuccessResult(string message) // <-- Nombre corregido
        {
            return new ServiceResult { IsSuccess = true, Message = message };
        }

        public static ServiceResult ErrorResult(string message) // <-- Nombre corregido
        {
            return new ServiceResult { IsSuccess = false, Message = message };
        }
    }

    // 2. ¡LA NUEVA CLASE GENÉRICA!
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }

        // Métodos de ayuda que COINCIDEN con tu VentasServices
        public static ServiceResult<T> SuccessResult(T data, string message = "Datos obtenidos") // <-- NOMBRE CORREGIDO
        {
            return new ServiceResult<T> { IsSuccess = true, Data = data, Message = message };
        }

        public static ServiceResult<T> ErrorResult(string message) // <-- NOMBRE CORREGIDO
        {
            return new ServiceResult<T> { IsSuccess = false, Data = default(T), Message = message };
        }
    }
}