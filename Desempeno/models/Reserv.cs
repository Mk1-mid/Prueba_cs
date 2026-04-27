namespace Desempeno.models;

public class Reserv
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public int IdSpace { get; set; }
    public string status { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan strat { get; set; }
    public TimeSpan end { get; set; }
    public Users usuario { get; set; }
    public SportSpace sportSpace { get; set; }
    
}