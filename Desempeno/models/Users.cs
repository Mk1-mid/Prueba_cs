using System.ComponentModel.DataAnnotations;

namespace Desempeno.models;

public class Users
{
  [Key]
  [Required]
  public int Id { get; set; }

  public string Document { get; set; } = string.Empty;
  [Required]
  [StringLength(100)]
  public string Name { get; set; } = string.Empty;

  [Required]
  [EmailAddress]
  [StringLength(150)]
  public string Email { get; set; } = string.Empty;

  [Required]
  [Range(1000000, 999999999)]
  public int Phone { get; set; }

  public ICollection<Reserv> Reservas { get; set; } = new List<Reserv>();
}
