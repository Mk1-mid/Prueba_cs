using Desempeno.models;

namespace Desempeno.repositories;

public interface IReservationRepository
{
    bool UserExists(int userId);
    bool SpaceExists(int spaceId);
    bool HasOverlapForSpace(int spaceId, DateTime date, TimeSpan start, TimeSpan end, int? excludeReservationId = null);
    bool HasOverlapForUser(int userId, DateTime date, TimeSpan start, TimeSpan end, int? excludeReservationId = null);
    List<Reserv> GetAllWithRelations();
    List<Reserv> GetByUserWithSpace(int userId);
    List<Reserv> GetBySpaceWithUser(int spaceId);
    Reserv? GetById(int id);
    Users? GetUserById(int id);
    void Add(Reserv reserv);
    void Remove(Reserv reserv);
    void SaveChanges();
}
