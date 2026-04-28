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
        if (newReserv.Date.Date < DateTime.Today)
            return new ServiceResponse<Reserv> { Success = false, Message = "No se pueden crear reservas en fechas pasadas." };

        if (newReserv.Date.Date == DateTime.Today && newReserv.strat < DateTime.Now.TimeOfDay)
            return new ServiceResponse<Reserv> { Success = false, Message = "La hora de inicio ya pasó." };

        if (newReserv.end <= newReserv.strat)
            return new ServiceResponse<Reserv> { Success = false, Message = "La hora de fin debe ser mayor a la hora de inicio." };

        bool usuarioExiste = _context.Users.Any(u => u.Id == newReserv.IdUser);
        if (!usuarioExiste)
            return new ServiceResponse<Reserv> { Success = false, Message = "Usuario no encontrado." };

        bool espacioExiste = _context.SportSpaces.Any(s => s.Id == newReserv.IdSpace);
        if (!espacioExiste)
            return new ServiceResponse<Reserv> { Success = false, Message = "Espacio no encontrado." };

        bool solapamientoEspacio = _context.Reservas.Any(r =>
            r.IdSpace == newReserv.IdSpace &&
            r.Date == newReserv.Date &&
            r.status != "Cancelada" &&
            r.strat < newReserv.end &&
            r.end > newReserv.strat);

        if (solapamientoEspacio)
            return new ServiceResponse<Reserv> { Success = false, Message = "El espacio ya tiene una reserva en ese horario." };

        bool solapamientoUsuario = _context.Reservas.Any(r =>
            r.IdUser == newReserv.IdUser &&
            r.Date == newReserv.Date &&
            r.status != "Cancelada" &&
            r.strat < newReserv.end &&
            r.end > newReserv.strat);

        if (solapamientoUsuario)
            return new ServiceResponse<Reserv> { Success = false, Message = "El usuario ya tiene una reserva en ese horario." };

        newReserv.status = "Programada";

        _context.Reservas.Add(newReserv);
        _context.SaveChanges();

        var usuario = _context.Users.FirstOrDefault(u => u.Id == newReserv.IdUser);
        Console.WriteLine(
            $"Correo enviado a {usuario?.Email}. Reserva creada para el {newReserv.Date:yyyy-MM-dd} de {newReserv.strat:hh\\:mm} a {newReserv.end:hh\\:mm}.");

        return new ServiceResponse<Reserv> { Success = true, Data = newReserv, Message = "Reserva registrada con éxito." };
    }

    public ServiceResponse<List<Reserv>> ListarReservas()
    {
        var lista = _context.Reservas
            .Include(r => r.usuario)
            .Include(r => r.sportSpace)
            .ToList();

        return new ServiceResponse<List<Reserv>> { Success = true, Data = lista };
    }

    public ServiceResponse<List<Reserv>> ListarReservasPorUsuario(int idUsuario)
    {
        bool usuarioExiste = _context.Users.Any(u => u.Id == idUsuario);
        if (!usuarioExiste)
            return new ServiceResponse<List<Reserv>> { Success = false, Message = "Usuario no encontrado." };

        var lista = _context.Reservas
            .Include(r => r.sportSpace)
            .Where(r => r.IdUser == idUsuario)
            .ToList();

        return new ServiceResponse<List<Reserv>> { Success = true, Data = lista };
    }

    public ServiceResponse<List<Reserv>> ListarReservasPorEspacio(int idEspacio)
    {
        bool espacioExiste = _context.SportSpaces.Any(s => s.Id == idEspacio);
        if (!espacioExiste)
            return new ServiceResponse<List<Reserv>> { Success = false, Message = "Espacio no encontrado." };

        var lista = _context.Reservas
            .Include(r => r.usuario)
            .Where(r => r.IdSpace == idEspacio)
            .ToList();

        return new ServiceResponse<List<Reserv>> { Success = true, Data = lista };
    }

    public ServiceResponse<Reserv> CancelarReserva(int id)
    {
        var reserva = _context.Reservas.FirstOrDefault(x => x.Id == id);
        if (reserva == null)
            return new ServiceResponse<Reserv> { Success = false, Message = "Reserva no encontrada." };

        if (reserva.status == "Cancelada")
            return new ServiceResponse<Reserv> { Success = false, Message = "La reserva ya está cancelada." };

        reserva.status = "Cancelada";
        _context.SaveChanges();

        return new ServiceResponse<Reserv> { Success = true, Data = reserva, Message = "Reserva cancelada." };
    }

    public ServiceResponse<Reserv> EditarReserva(int id, Reserv reservaModificada)
    {
        var reservaEdit = _context.Reservas.FirstOrDefault(x => x.Id == id);
        if (reservaEdit == null)
            return new ServiceResponse<Reserv> { Success = false, Message = "Reserva no encontrada." };

        if (reservaEdit.status == "Cancelada")
            return new ServiceResponse<Reserv> { Success = false, Message = "No se puede editar una reserva cancelada." };

        if (reservaModificada.Date.Date < DateTime.Today)
            return new ServiceResponse<Reserv> { Success = false, Message = "No se pueden usar fechas pasadas." };

        if (reservaModificada.end <= reservaModificada.strat)
            return new ServiceResponse<Reserv> { Success = false, Message = "La hora de fin debe ser mayor a la hora de inicio." };

        bool usuarioExiste = _context.Users.Any(u => u.Id == reservaModificada.IdUser);
        if (!usuarioExiste)
            return new ServiceResponse<Reserv> { Success = false, Message = "Usuario no encontrado." };

        bool espacioExiste = _context.SportSpaces.Any(s => s.Id == reservaModificada.IdSpace);
        if (!espacioExiste)
            return new ServiceResponse<Reserv> { Success = false, Message = "Espacio no encontrado." };

        bool solapamientoEspacio = _context.Reservas.Any(r =>
            r.Id != id &&
            r.IdSpace == reservaModificada.IdSpace &&
            r.Date == reservaModificada.Date &&
            r.status != "Cancelada" &&
            r.strat < reservaModificada.end &&
            r.end > reservaModificada.strat);

        if (solapamientoEspacio)
            return new ServiceResponse<Reserv> { Success = false, Message = "El espacio ya tiene una reserva en ese horario." };

        bool solapamientoUsuario = _context.Reservas.Any(r =>
            r.Id != id &&
            r.IdUser == reservaModificada.IdUser &&
            r.Date == reservaModificada.Date &&
            r.status != "Cancelada" &&
            r.strat < reservaModificada.end &&
            r.end > reservaModificada.strat);

        if (solapamientoUsuario)
            return new ServiceResponse<Reserv> { Success = false, Message = "El usuario ya tiene una reserva en ese horario." };

        reservaEdit.IdUser = reservaModificada.IdUser;
        reservaEdit.IdSpace = reservaModificada.IdSpace;
        reservaEdit.status = string.IsNullOrWhiteSpace(reservaModificada.status) ? "Programada" : reservaModificada.status;
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
