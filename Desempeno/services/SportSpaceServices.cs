using Desempeno.models;
using Desempeno.Data;

namespace Desempeno.services;

public class SportSpaceServices
{
    private readonly AppDbContext _context;

    public SportSpaceServices(AppDbContext context)
    {
        _context = context;
    }

    public ServiceResponse<SportSpace> RegistrarEspacio(SportSpace newSpace)
    {
        try
        {
            bool existe = _context.SportSpaces.Any(s => s.Id == newSpace.Id || s.Name == newSpace.Name);
            if (existe)
                return new ServiceResponse<SportSpace> { Success = false, Message = "El espacio ya existe (Id o Nombre duplicado)." };

            _context.SportSpaces.Add(newSpace);
            _context.SaveChanges();

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
            var lista = _context.SportSpaces.ToList();
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
            var lista = _context.SportSpaces
                .Where(s => s.Tipe.ToLower() == tipo.ToLower())
                .ToList();

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
            var spaceEdit = _context.SportSpaces.FirstOrDefault(x => x.Id == id);
            if (spaceEdit == null)
                return new ServiceResponse<SportSpace> { Success = false, Message = "Espacio no encontrado." };

            spaceEdit.Name = spaceModificado.Name;
            spaceEdit.Tipe = spaceModificado.Tipe;
            spaceEdit.capacity = spaceModificado.capacity;

            _context.SaveChanges();

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
            var space = _context.SportSpaces.FirstOrDefault(x => x.Id == id);
            if (space == null)
                return new ServiceResponse<bool> { Success = false, Message = "Espacio no encontrado." };

            _context.SportSpaces.Remove(space);
            _context.SaveChanges();

            return new ServiceResponse<bool> { Success = true, Message = "Espacio eliminado." };
        }
        catch (Exception ex)
        {
            return new ServiceResponse<bool> { Success = false, Message = $"Error: {ex.Message}" };
        }
    }
}
