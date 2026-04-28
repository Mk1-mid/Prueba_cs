using Desempeno.models;
using Desempeno.repositories;

namespace Desempeno.services;

public class UserServices
{
    private readonly IUserRepository _userRepository;

    public UserServices(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public ServiceResponse<Users> RegistrarUsuario(Users newUser)
    {
        bool existe = _userRepository.ExistsByIdOrEmail(newUser.Id, newUser.Email);
        
        if (existe)
        {
            return new ServiceResponse<Users> { Success = false, Message = "El usuario ya existe (Documento o Email duplicado)." };
        }

        _userRepository.Add(newUser);
        _userRepository.SaveChanges();
        
        return new ServiceResponse<Users> { Success = true, Data = newUser, Message = "Usuario registrado con éxito." };
    }

    public ServiceResponse<List<Users>> ListarUsuarios()
    {
        var lista = _userRepository.GetAll();
        return new ServiceResponse<List<Users>> { Success = true, Data = lista };
    }

    public ServiceResponse<Users> EditarUsuario(int id, Users userModificado)
    {
        var userEdit = _userRepository.GetById(id);
        
        if (userEdit == null)
            return new ServiceResponse<Users> { Success = false, Message = "Usuario no encontrado." };

        
        userEdit.Name = userModificado.Name;
        userEdit.Email = userModificado.Email;
        userEdit.Phone = userModificado.Phone;

        _userRepository.SaveChanges();
        
        return new ServiceResponse<Users> { Success = true, Data = userEdit, Message = "Usuario actualizado." };
    }

    public ServiceResponse<bool> EliminarUsuario(int id)
    {
        var user = _userRepository.GetById(id);
        if (user == null)
            return new ServiceResponse<bool> { Success = false, Message = "Usuario no encontrado." };

        _userRepository.Remove(user);
        _userRepository.SaveChanges();
        
        return new ServiceResponse<bool> { Success = true, Message = "Usuario eliminado." };
    }
}
