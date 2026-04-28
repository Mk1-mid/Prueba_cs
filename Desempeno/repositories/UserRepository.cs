using Desempeno.Data;
using Desempeno.models;

namespace Desempeno.repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public bool ExistsByIdOrEmail(int id, string email) => _context.Users.Any(u => u.Id == id || u.Email == email);

    public List<Users> GetAll() => _context.Users.ToList();

    public Users? GetById(int id) => _context.Users.FirstOrDefault(u => u.Id == id);

    public void Add(Users user) => _context.Users.Add(user);

    public void Remove(Users user) => _context.Users.Remove(user);

    public void SaveChanges() => _context.SaveChanges();
}
