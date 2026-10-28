namespace AutoSzervizApp.Models;

public class SzervizTipus
{
    public int Id { get; set; }
    public string Nev { get; set; } = "";
    public string? Leiras { get; set; }
    public decimal AlapAr { get; set; }
    public int BecsultIdotartamPerc { get; set; }
}