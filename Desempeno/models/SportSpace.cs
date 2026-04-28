using System.ComponentModel.DataAnnotations;

namespace Desempeno.models;

public class SportSpace
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Tipe { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int capacity { get; set; }

    public ICollection<Reserv> Reservas { get; set; } = new List<Reserv>();
}
