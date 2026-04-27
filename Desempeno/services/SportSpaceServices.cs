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
        bool existe = _context.SportSpaces.Any(s => s.Id == newSpace.Id || s.Name == newSpace.Name);
        if (existe)
        {
            return new ServiceResponse<SportSpace>
            {
                Success = false,
                Message = "El espacio ya existe (Id o Nombre duplicado)."
            };
        }

        _context.SportSpaces.Add(newSpace);
        _context.SaveChanges();

        return new ServiceResponse<SportSpace>
        {
            Success = true,
            Data = newSpace,
            Message = "Espacio registrado con exito."
        };
    }

    public ServiceResponse<List<SportSpace>> ListarEspacios()
    {
        var lista = _context.SportSpaces.ToList();
        return new ServiceResponse<List<SportSpace>> { Success = true, Data = lista };
    }

    public ServiceResponse<SportSpace> EditarEspacio(int id, SportSpace spaceModificado)
    {
        var spaceEdit = _context.SportSpaces.FirstOrDefault(x => x.Id == id);
        if (spaceEdit == null)
            return new ServiceResponse<SportSpace> { Success = false, Message = "Espacio no encontrado." };

        spaceEdit.Name = spaceModificado.Name;
        spaceEdit.Tipe = spaceModificado.Tipe;
        spaceEdit.capacity = spaceModificado.capacity;

        _context.SaveChanges();

        return new ServiceResponse<SportSpace>
        {
            Success = true,
            Data = spaceEdit,
            Message = "Espacio actualizado."
        };
    }

    public ServiceResponse<bool> EliminarEspacio(int id)
    {
        var space = _context.SportSpaces.FirstOrDefault(x => x.Id == id);
        if (space == null)
            return new ServiceResponse<bool> { Success = false, Message = "Espacio no encontrado." };

        _context.SportSpaces.Remove(space);
        _context.SaveChanges();

        return new ServiceResponse<bool> { Success = true, Message = "Espacio eliminado." };
    }
}
