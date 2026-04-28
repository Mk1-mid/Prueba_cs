using Desempeno.models;
using Desempeno.repositories;

namespace Desempeno.services;

public class ReservationServices
{
    private readonly IReservationRepository _reservationRepository;

    public ReservationServices(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public ServiceResponse<Reserv> RegistrarReserva(Reserv newReserv)
    {
        if (newReserv.Date.Date < DateTime.Today)
            return new ServiceResponse<Reserv> { Success = false, Message = "No se pueden crear reservas en fechas pasadas." };

        if (newReserv.Date.Date == DateTime.Today && newReserv.strat < DateTime.Now.TimeOfDay)
            return new ServiceResponse<Reserv> { Success = false, Message = "La hora de inicio ya pasó." };

        if (newReserv.end <= newReserv.strat)
            return new ServiceResponse<Reserv> { Success = false, Message = "La hora de fin debe ser mayor a la hora de inicio." };

        bool usuarioExiste = _reservationRepository.UserExists(newReserv.IdUser);
        if (!usuarioExiste)
            return new ServiceResponse<Reserv> { Success = false, Message = "Usuario no encontrado." };

        bool espacioExiste = _reservationRepository.SpaceExists(newReserv.IdSpace);
        if (!espacioExiste)
            return new ServiceResponse<Reserv> { Success = false, Message = "Espacio no encontrado." };

        bool solapamientoEspacio = _reservationRepository.HasOverlapForSpace(
            newReserv.IdSpace,
            newReserv.Date,
            newReserv.strat,
            newReserv.end);

        if (solapamientoEspacio)
            return new ServiceResponse<Reserv> { Success = false, Message = "El espacio ya tiene una reserva en ese horario." };

        bool solapamientoUsuario = _reservationRepository.HasOverlapForUser(
            newReserv.IdUser,
            newReserv.Date,
            newReserv.strat,
            newReserv.end);

        if (solapamientoUsuario)
            return new ServiceResponse<Reserv> { Success = false, Message = "El usuario ya tiene una reserva en ese horario." };

        newReserv.status = "Programada";

        _reservationRepository.Add(newReserv);
        _reservationRepository.SaveChanges();

        var usuario = _reservationRepository.GetUserById(newReserv.IdUser);
        Console.WriteLine(
            $"Correo enviado a {usuario?.Email}. Reserva creada para el {newReserv.Date:yyyy-MM-dd} de {newReserv.strat:hh\\:mm} a {newReserv.end:hh\\:mm}.");

        return new ServiceResponse<Reserv> { Success = true, Data = newReserv, Message = "Reserva registrada con éxito." };
    }

    public ServiceResponse<List<Reserv>> ListarReservas()
    {
        var lista = _reservationRepository.GetAllWithRelations();

        return new ServiceResponse<List<Reserv>> { Success = true, Data = lista };
    }

    public ServiceResponse<List<Reserv>> ListarReservasPorUsuario(int idUsuario)
    {
        bool usuarioExiste = _reservationRepository.UserExists(idUsuario);
        if (!usuarioExiste)
            return new ServiceResponse<List<Reserv>> { Success = false, Message = "Usuario no encontrado." };

        var lista = _reservationRepository.GetByUserWithSpace(idUsuario);

        return new ServiceResponse<List<Reserv>> { Success = true, Data = lista };
    }

    public ServiceResponse<List<Reserv>> ListarReservasPorEspacio(int idEspacio)
    {
        bool espacioExiste = _reservationRepository.SpaceExists(idEspacio);
        if (!espacioExiste)
            return new ServiceResponse<List<Reserv>> { Success = false, Message = "Espacio no encontrado." };

        var lista = _reservationRepository.GetBySpaceWithUser(idEspacio);

        return new ServiceResponse<List<Reserv>> { Success = true, Data = lista };
    }

    public ServiceResponse<Reserv> CancelarReserva(int id)
    {
        var reserva = _reservationRepository.GetById(id);
        if (reserva == null)
            return new ServiceResponse<Reserv> { Success = false, Message = "Reserva no encontrada." };

        if (reserva.status == "Cancelada")
            return new ServiceResponse<Reserv> { Success = false, Message = "La reserva ya está cancelada." };

        reserva.status = "Cancelada";
        _reservationRepository.SaveChanges();

        return new ServiceResponse<Reserv> { Success = true, Data = reserva, Message = "Reserva cancelada." };
    }

    public ServiceResponse<Reserv> EditarReserva(int id, Reserv reservaModificada)
    {
        var reservaEdit = _reservationRepository.GetById(id);
        if (reservaEdit == null)
            return new ServiceResponse<Reserv> { Success = false, Message = "Reserva no encontrada." };

        if (reservaEdit.status == "Cancelada")
            return new ServiceResponse<Reserv> { Success = false, Message = "No se puede editar una reserva cancelada." };

        if (reservaModificada.Date.Date < DateTime.Today)
            return new ServiceResponse<Reserv> { Success = false, Message = "No se pueden usar fechas pasadas." };

        if (reservaModificada.end <= reservaModificada.strat)
            return new ServiceResponse<Reserv> { Success = false, Message = "La hora de fin debe ser mayor a la hora de inicio." };

        bool usuarioExiste = _reservationRepository.UserExists(reservaModificada.IdUser);
        if (!usuarioExiste)
            return new ServiceResponse<Reserv> { Success = false, Message = "Usuario no encontrado." };

        bool espacioExiste = _reservationRepository.SpaceExists(reservaModificada.IdSpace);
        if (!espacioExiste)
            return new ServiceResponse<Reserv> { Success = false, Message = "Espacio no encontrado." };

        bool solapamientoEspacio = _reservationRepository.HasOverlapForSpace(
            reservaModificada.IdSpace,
            reservaModificada.Date,
            reservaModificada.strat,
            reservaModificada.end,
            id);

        if (solapamientoEspacio)
            return new ServiceResponse<Reserv> { Success = false, Message = "El espacio ya tiene una reserva en ese horario." };

        bool solapamientoUsuario = _reservationRepository.HasOverlapForUser(
            reservaModificada.IdUser,
            reservaModificada.Date,
            reservaModificada.strat,
            reservaModificada.end,
            id);

        if (solapamientoUsuario)
            return new ServiceResponse<Reserv> { Success = false, Message = "El usuario ya tiene una reserva en ese horario." };

        reservaEdit.IdUser = reservaModificada.IdUser;
        reservaEdit.IdSpace = reservaModificada.IdSpace;
        reservaEdit.status = string.IsNullOrWhiteSpace(reservaModificada.status) ? "Programada" : reservaModificada.status;
        reservaEdit.Date = reservaModificada.Date;
        reservaEdit.strat = reservaModificada.strat;
        reservaEdit.end = reservaModificada.end;

        _reservationRepository.SaveChanges();

        return new ServiceResponse<Reserv> { Success = true, Data = reservaEdit, Message = "Reserva actualizada." };
    }

    public ServiceResponse<bool> EliminarReserva(int id)
    {
        var reserva = _reservationRepository.GetById(id);
        if (reserva == null)
            return new ServiceResponse<bool> { Success = false, Message = "Reserva no encontrada." };

        _reservationRepository.Remove(reserva);
        _reservationRepository.SaveChanges();

        return new ServiceResponse<bool> { Success = true, Message = "Reserva eliminada." };
    }
}
