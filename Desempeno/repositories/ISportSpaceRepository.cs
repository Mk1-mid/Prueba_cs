using Desempeno.models;

namespace Desempeno.repositories;

public interface ISportSpaceRepository
{
    bool ExistsByIdOrName(int id, string name);
    List<SportSpace> GetAll();
    List<SportSpace> FilterByType(string type);
    SportSpace? GetById(int id);
    void Add(SportSpace sportSpace);
    void Remove(SportSpace sportSpace);
    bool ExistsById(int id);
    void SaveChanges();
}
