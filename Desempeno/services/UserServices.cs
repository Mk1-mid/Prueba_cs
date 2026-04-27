using Desempeno.models;

namespace Desempeno.services;

public class UserServices
{
    private readonly AppDbContext _context;

    public UserServices(AppDbContext _context)
    {
        _context = context;
    }
    public void UserRegistration(User newUser)
    {
        _context.users.Add(newUser);
        _context.SaveChanges();
        
    }

    public void UserLogin()
    {
        
    }

    public List<Users> Userlist()
    {
        return _context.users.ToList();
    }

    public void Useredit()
    {
    }

    public void UserDelete()
    {
    }
}
