namespace Desempeno.services;

public class ServiceBase
{
    protected ServiceResponse<T> EjecutarConError<T>(Func<ServiceResponse<T>> accion)
    {
        try
        {
            return accion();
        }
        catch (Exception ex)
        {
            return ServiceResponse<T>.Error(ex.Message);
        }
    }

    protected async Task<ServiceResponse<T>> EjecutarConErrorAsync<T>(Func<Task<ServiceResponse<T>>> accion)
    {
        try
        {
            return await accion();
        }
        catch (Exception ex)
        {
            return ServiceResponse<T>.Error(ex.Message);
        }
    }
}