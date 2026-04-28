using Desempeno.models;
using Desempeno.repositories;

namespace Desempeno.services;

public class UserServices : ServiceBase
{
    private readonly IUserRepository _userRepository;

    public UserServices(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public ServiceResponse<Users> RegistrarUsuario(Users newUser)
    {
        return EjecutarConError(() =>
        {
            bool existe = _userRepository.ExistsByIdOrEmail(newUser.Id, newUser.Email);
            
            if (existe)
            {
                return ServiceResponse<Users>.Error("El usuario ya existe (Documento o Email duplicado).");
            }

            _userRepository.Add(newUser);
            _userRepository.SaveChanges();
            
            return ServiceResponse<Users>.Ok(newUser, "Usuario registrado con éxito.");
        });
    }

    public ServiceResponse<List<Users>> ListarUsuarios()
    {
        return EjecutarConError(() =>
        {
            var lista = _userRepository.GetAll();
            return ServiceResponse<List<Users>>.Ok(lista);
        });
    }

    public ServiceResponse<Users> EditarUsuario(int id, Users userModificado)
    {
        return EjecutarConError(() =>
        {
            var userEdit = _userRepository.GetById(id);
            
            if (userEdit == null)
                return ServiceResponse<Users>.Error("Usuario no encontrado.");

            userEdit.Name = userModificado.Name;
            userEdit.Email = userModificado.Email;
            userEdit.Phone = userModificado.Phone;

            _userRepository.SaveChanges();
            
            return ServiceResponse<Users>.Ok(userEdit, "Usuario actualizado.");
        });
    }

    public ServiceResponse<bool> EliminarUsuario(int id)
    {
        return EjecutarConError(() =>
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return ServiceResponse<bool>.Error("Usuario no encontrado.");

            _userRepository.Remove(user);
            _userRepository.SaveChanges();
            
            return ServiceResponse<bool>.Ok(true, "Usuario eliminado.");
        });
    }
}
