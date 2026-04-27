using Desempeno.models;
using Desempeno.services;
namespace Desempeno.services;

public class UserServices
{
    private readonly AppDbContext _context;

    // Corrección del constructor: ahora es correcto
    public UserServices(AppDbContext context)
    {
        _context = context;
    }

    public ServiceResponse<Users> RegistrarUsuario(Users newUser)
    {
        bool existe = _context.Users.Any(u => u.Documento == newUser.Documento || u.Email == newUser.Email);
        
        if (existe)
        {
            return new ServiceResponse<Users> { Success = false, Message = "El usuario ya existe (Documento o Email duplicado)." };
        }

        _context.Users.Add(newUser);
        _context.SaveChanges();
        
        return new ServiceResponse<Users> { Success = true, Data = newUser, Message = "Usuario registrado con éxito." };
    }

    public ServiceResponse<List<Users>> ListarUsuarios()
    {
        var lista = _context.Users.ToList();
        return new ServiceResponse<List<Users>> { Success = true, Data = lista };
    }

    public ServiceResponse<Users> EditarUsuario(int id, Users userModificado)
    {
        var userEdit = _context.Users.FirstOrDefault(x => x.Id == id);
        
        if (userEdit == null)
            return new ServiceResponse<Users> { Success = false, Message = "Usuario no encontrado." };

        
        userEdit.Nombre = userModificado.Name;
        userEdit.Email = userModificado.Email;
        userEdit.Telefono = userModificado.Phone;

        _context.SaveChanges();
        
        return new ServiceResponse<Users> { Success = true, Data = userEdit, Message = "Usuario actualizado." };
    }

    public ServiceResponse<bool> EliminarUsuario(int id)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id == id);
        if (user == null)
            return new ServiceResponse<bool> { Success = false, Message = "Usuario no encontrado." };

        _context.Users.Remove(user);
        _context.SaveChanges();
        
        return new ServiceResponse<bool> { Success = true, Message = "Usuario eliminado." };
    }
}
