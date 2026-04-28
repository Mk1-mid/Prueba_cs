using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Desempeno.models;

public class Reserv
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(usuario))]
    public int IdUser { get; set; }

    [Required]
    [ForeignKey(nameof(sportSpace))]
    public int IdSpace { get; set; }

    [Required]
    [StringLength(20)]
    public string status { get; set; } = "Programada";

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public TimeSpan strat { get; set; }

    [Required]
    public TimeSpan end { get; set; }

    public Users usuario { get; set; } = null!;
    public SportSpace sportSpace { get; set; } = null!;
}
