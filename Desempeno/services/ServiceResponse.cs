namespace Desempeno.services;

public class ServiceResponse<T>
{
    public T Data { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }

    public static ServiceResponse<T> Ok(T data, string message = null) => new ServiceResponse<T> { Success = true, Data = data, Message = message };
    public static ServiceResponse<T> Error(string message) => new ServiceResponse<T> { Success = false, Message = message };
}