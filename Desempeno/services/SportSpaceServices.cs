using Desempeno.models;
using Desempeno.repositories;

namespace Desempeno.services;

public class SportSpaceServices
{
    private readonly ISportSpaceRepository _sportSpaceRepository;

    public SportSpaceServices(ISportSpaceRepository sportSpaceRepository)
    {
        _sportSpaceRepository = sportSpaceRepository;
    }

    public ServiceResponse<SportSpace> RegistrarEspacio(SportSpace newSpace)
    {
        try
        {
            bool existe = _sportSpaceRepository.ExistsByIdOrName(newSpace.Id, newSpace.Name);
            if (existe)
                return new ServiceResponse<SportSpace> { Success = false, Message = "El espacio ya existe (Id o Nombre duplicado)." };

            _sportSpaceRepository.Add(newSpace);
            _sportSpaceRepository.SaveChanges();

            return new ServiceResponse<SportSpace> { Success = true, Data = newSpace, Message = "Espacio registrado con éxito." };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<SportSpace> { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    public ServiceResponse<List<SportSpace>> ListarEspacios()
    {
        try
        {
            var lista = _sportSpaceRepository.GetAll();
            return new ServiceResponse<List<SportSpace>> { Success = true, Data = lista };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<List<SportSpace>> { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    public ServiceResponse<List<SportSpace>> FiltrarPorTipo(string tipo)
    {
        try
        {
            var lista = _sportSpaceRepository.FilterByType(tipo);

            if (!lista.Any())
                return new ServiceResponse<List<SportSpace>> { Success = false, Message = $"No hay espacios de tipo '{tipo}'." };

            return new ServiceResponse<List<SportSpace>> { Success = true, Data = lista };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<List<SportSpace>> { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    public ServiceResponse<SportSpace> EditarEspacio(int id, SportSpace spaceModificado)
    {
        try
        {
            var spaceEdit = _sportSpaceRepository.GetById(id);
            if (spaceEdit == null)
                return new ServiceResponse<SportSpace> { Success = false, Message = "Espacio no encontrado." };

            spaceEdit.Name = spaceModificado.Name;
            spaceEdit.Tipe = spaceModificado.Tipe;
            spaceEdit.capacity = spaceModificado.capacity;

            _sportSpaceRepository.SaveChanges();

            return new ServiceResponse<SportSpace> { Success = true, Data = spaceEdit, Message = "Espacio actualizado." };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<SportSpace> { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    public ServiceResponse<bool> EliminarEspacio(int id)
    {
        try
        {
            var space = _sportSpaceRepository.GetById(id);
            if (space == null)
                return new ServiceResponse<bool> { Success = false, Message = "Espacio no encontrado." };

            _sportSpaceRepository.Remove(space);
            _sportSpaceRepository.SaveChanges();

            return new ServiceResponse<bool> { Success = true, Message = "Espacio eliminado." };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<bool> { Success = false, Message = $"Error: {ex.Message}" };
        }
    }
}
