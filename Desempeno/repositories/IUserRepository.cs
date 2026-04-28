using Desempeno.models;

namespace Desempeno.repositories;

public interface IUserRepository
{
    bool ExistsByIdOrEmail(int id, string email);
    List<Users> GetAll();
    Users? GetById(int id);
    void Add(Users user);
    void Remove(Users user);
    void SaveChanges();
}
