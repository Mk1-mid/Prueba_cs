using Desempeno.Data;
using Desempeno.models;

namespace Desempeno.repositories;

public class SportSpaceRepository : ISportSpaceRepository
{
    private readonly AppDbContext _context;

    public SportSpaceRepository(AppDbContext context)
    {
        _context = context;
    }

    public bool ExistsByIdOrName(int id, string name) => _context.SportSpaces.Any(s => s.Id == id || s.Name == name);

    public List<SportSpace> GetAll() => _context.SportSpaces.ToList();

    public List<SportSpace> FilterByType(string type) =>
        _context.SportSpaces.Where(s => s.Tipe.ToLower() == type.ToLower()).ToList();

    public SportSpace? GetById(int id) => _context.SportSpaces.FirstOrDefault(s => s.Id == id);

    public void Add(SportSpace sportSpace) => _context.SportSpaces.Add(sportSpace);

    public void Remove(SportSpace sportSpace) => _context.SportSpaces.Remove(sportSpace);

    public bool ExistsById(int id) => _context.SportSpaces.Any(s => s.Id == id);

    public void SaveChanges() => _context.SaveChanges();
}
