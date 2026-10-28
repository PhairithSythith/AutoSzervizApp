namespace AutoSzervizApp.Models;

public class Jarmu
{
    public int Id { get; set; }
    public string Rendszam { get; set; } = "";
    public string Marka { get; set; } = "";
    public string Modell { get; set; } = "";
    public int Evjarat { get; set; }

    public int UgyfelId { get; set; }
    public Ugyfel? Ugyfel { get; set; }
}