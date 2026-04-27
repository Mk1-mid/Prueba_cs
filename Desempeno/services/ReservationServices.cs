using Desempeno.models;
using Desempeno.Data;
using Microsoft.EntityFrameworkCore;

namespace Desempeno.services;

public class ReservationServices
{
    private readonly AppDbContext _context;

    public ReservationServices(AppDbContext context)
    {
        _context = context;
    }

    public ServiceResponse<Reserv> RegistrarReserva(Reserv newReserv)
    {
        bool usuarioExiste = _context.Users.Any(u => u.Id == newReserv.IdUser);
        if (!usuarioExiste)
            return new ServiceResponse<Reserv> { Success = false, Message = "Usuario no encontrado." };

        bool espacioExiste = _context.SportSpaces.Any(s => s.Id == newReserv.IdSpace);
        if (!espacioExiste)
            return new ServiceResponse<Reserv> { Success = false, Message = "Espacio no encontrado." };

        bool existe = _context.Reservas.Any(r =>
            r.IdUser == newReserv.IdUser &&
            r.IdSpace == newReserv.IdSpace &&
            r.Date == newReserv.Date &&
            r.strat == newReserv.strat &&
            r.end == newReserv.end);

        if (existe)
            return new ServiceResponse<Reserv> { Success = false, Message = "La reserva ya existe." };

        _context.Reservas.Add(newReserv);
        _context.SaveChanges();

        return new ServiceResponse<Reserv> { Success = true, Data = newReserv, Message = "Reserva registrada con exito." };
    }

    public ServiceResponse<List<Reserv>> ListarReservas()
    {
        var lista = _context.Reservas
            .Include(r => r.usuario)
            .Include(r => r.sportSpace)
            .ToList();

        return new ServiceResponse<List<Reserv>> { Success = true, Data = lista };
    }

    public ServiceResponse<Reserv> EditarReserva(int id, Reserv reservaModificada)
    {
        var reservaEdit = _context.Reservas.FirstOrDefault(x => x.Id == id);
        if (reservaEdit == null)
            return new ServiceResponse<Reserv> { Success = false, Message = "Reserva no encontrada." };

        bool usuarioExiste = _context.Users.Any(u => u.Id == reservaModificada.IdUser);
        if (!usuarioExiste)
            return new ServiceResponse<Reserv> { Success = false, Message = "Usuario no encontrado." };

        bool espacioExiste = _context.SportSpaces.Any(s => s.Id == reservaModificada.IdSpace);
        if (!espacioExiste)
            return new ServiceResponse<Reserv> { Success = false, Message = "Espacio no encontrado." };

        reservaEdit.IdUser = reservaModificada.IdUser;
        reservaEdit.IdSpace = reservaModificada.IdSpace;
        reservaEdit.status = reservaModificada.status;
        reservaEdit.Date = reservaModificada.Date;
        reservaEdit.strat = reservaModificada.strat;
        reservaEdit.end = reservaModificada.end;

        _context.SaveChanges();

        return new ServiceResponse<Reserv> { Success = true, Data = reservaEdit, Message = "Reserva actualizada." };
    }

    public ServiceResponse<bool> EliminarReserva(int id)
    {
        var reserva = _context.Reservas.FirstOrDefault(x => x.Id == id);
        if (reserva == null)
            return new ServiceResponse<bool> { Success = false, Message = "Reserva no encontrada." };

        _context.Reservas.Remove(reserva);
        _context.SaveChanges();

        return new ServiceResponse<bool> { Success = true, Message = "Reserva eliminada." };
    }
}
