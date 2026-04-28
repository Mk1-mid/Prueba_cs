using Desempeno.models;
using Desempeno.repositories;

namespace Desempeno.services;

public class SportSpaceServices : ServiceBase
{
    private readonly ISportSpaceRepository _sportSpaceRepository;

    public SportSpaceServices(ISportSpaceRepository sportSpaceRepository)
    {
        _sportSpaceRepository = sportSpaceRepository;
    }

    public ServiceResponse<SportSpace> RegistrarEspacio(SportSpace newSpace)
    {
        return EjecutarConError(() =>
        {
            bool existe = _sportSpaceRepository.ExistsByIdOrName(newSpace.Id, newSpace.Name);
            if (existe)
                return ServiceResponse<SportSpace>.Error("El espacio ya existe (Id o Nombre duplicado).");

            _sportSpaceRepository.Add(newSpace);
            _sportSpaceRepository.SaveChanges();

            return ServiceResponse<SportSpace>.Ok(newSpace, "Espacio registrado con éxito.");
        });
    }

    public ServiceResponse<List<SportSpace>> ListarEspacios()
    {
        return EjecutarConError(() =>
        {
            var lista = _sportSpaceRepository.GetAll();
            return ServiceResponse<List<SportSpace>>.Ok(lista);
        });
    }

    public ServiceResponse<List<SportSpace>> FiltrarPorTipo(string tipo)
    {
        return EjecutarConError(() =>
        {
            var lista = _sportSpaceRepository.FilterByType(tipo);

            if (!lista.Any())
                return ServiceResponse<List<SportSpace>>.Error($"No hay espacios de tipo '{tipo}'.");

            return ServiceResponse<List<SportSpace>>.Ok(lista);
        });
    }

    public ServiceResponse<SportSpace> EditarEspacio(int id, SportSpace spaceModificado)
    {
        return EjecutarConError(() =>
        {
            var spaceEdit = _sportSpaceRepository.GetById(id);
            if (spaceEdit == null)
                return ServiceResponse<SportSpace>.Error("Espacio no encontrado.");

            spaceEdit.Name = spaceModificado.Name;
            spaceEdit.Tipe = spaceModificado.Tipe;
            spaceEdit.capacity = spaceModificado.capacity;

            _sportSpaceRepository.SaveChanges();

            return ServiceResponse<SportSpace>.Ok(spaceEdit, "Espacio actualizado.");
        });
    }

    public ServiceResponse<bool> EliminarEspacio(int id)
    {
        return EjecutarConError(() =>
        {
            var space = _sportSpaceRepository.GetById(id);
            if (space == null)
                return ServiceResponse<bool>.Error("Espacio no encontrado.");

            _sportSpaceRepository.Remove(space);
            _sportSpaceRepository.SaveChanges();

            return ServiceResponse<bool>.Ok(true, "Espacio eliminado.");
        });
    }
}
