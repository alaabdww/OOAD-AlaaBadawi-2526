namespace ConsoleKlassenOefenblad.Exercises.Classes;

internal class Bestelling
{
    // Properties
    public int BestellingId { get; set; }
    public DateTime Datum { get; set; } = DateTime.Now;
    public string KlantNaam { get; set; }
    public string Status
    {
        get;
        set
        {
            string[] toegelaten = { "Bezig", "Afgerond", "Geannuleerd" };
            if (!toegelaten.Contains(value)) throw new ArgumentException($"Ongeldige status: {value}");
            field = value;
        }
    } = "Bezig";

    public List<Product> Producten { get; set; } = new();

    // Berekende properties
    public decimal TotaalBedrag 
    {
        get 
        {
            decimal totBed = 0;

            foreach (Product p in Producten)
            {
                totBed += p.Prijs;
            }
            return totBed;
        }
    }

    

    // ToString override
    public override string ToString()
    {
        // pas dit aan zodat het aantal producten weergegeven wordt
        return $"#{BestellingId} — {KlantNaam} | {Producten.Count} product(en) | € {TotaalBedrag:F2} | {Status}";
    }
}
