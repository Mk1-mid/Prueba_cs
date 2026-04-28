using Desempeno.Data;
using Desempeno.models;
using Microsoft.EntityFrameworkCore;

namespace Desempeno.repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public bool UserExists(int userId) => _context.Users.Any(u => u.Id == userId);

    public bool SpaceExists(int spaceId) => _context.SportSpaces.Any(s => s.Id == spaceId);

    public bool HasOverlapForSpace(int spaceId, DateTime date, TimeSpan start, TimeSpan end, int? excludeReservationId = null) =>
        _context.Reservas.Any(r =>
            r.IdSpace == spaceId &&
            r.Date == date &&
            r.status != "Cancelada" &&
            r.strat < end &&
            r.end > start &&
            (!excludeReservationId.HasValue || r.Id != excludeReservationId.Value));

    public bool HasOverlapForUser(int userId, DateTime date, TimeSpan start, TimeSpan end, int? excludeReservationId = null) =>
        _context.Reservas.Any(r =>
            r.IdUser == userId &&
            r.Date == date &&
            r.status != "Cancelada" &&
            r.strat < end &&
            r.end > start &&
            (!excludeReservationId.HasValue || r.Id != excludeReservationId.Value));

    public List<Reserv> GetAllWithRelations() => _context.Reservas
        .Include(r => r.usuario)
        .Include(r => r.sportSpace)
        .ToList();

    public List<Reserv> GetByUserWithSpace(int userId) => _context.Reservas
        .Include(r => r.sportSpace)
        .Where(r => r.IdUser == userId)
        .ToList();

    public List<Reserv> GetBySpaceWithUser(int spaceId) => _context.Reservas
        .Include(r => r.usuario)
        .Where(r => r.IdSpace == spaceId)
        .ToList();

    public Reserv? GetById(int id) => _context.Reservas.FirstOrDefault(r => r.Id == id);

    public Users? GetUserById(int id) => _context.Users.FirstOrDefault(u => u.Id == id);

    public void Add(Reserv reserv) => _context.Reservas.Add(reserv);

    public void Remove(Reserv reserv) => _context.Reservas.Remove(reserv);

    public void SaveChanges() => _context.SaveChanges();
}
