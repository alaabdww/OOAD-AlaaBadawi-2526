namespace CLHelpDesk;

/// <summary>
/// Ticket voor softwareproblemen (Outlook, Teams, Excel, …).
/// </summary>
public class SoftwareTicket : Ticket
{
    public string ApplicatieNaam { get; set; }
    public string Versie { get; set; }

    public SoftwareTicket()
    {
        ApplicatieNaam = string.Empty;
        Versie = string.Empty;
    }

    public SoftwareTicket(int id, string titel, string melder, TicketPrioriteit prioriteit, string applicatieNaam, string versie)
        : base(id, titel, melder, prioriteit)
    {
        ApplicatieNaam = applicatieNaam;
        Versie = versie;
    }

    public override string GeefInfo()
    {
        return GeefBasisInfo() + $@"

Type: Software
Applicatie: {ApplicatieNaam}
Versie: {Versie}";
    }

    protected override string NaarCsvRegel()
    {
        return NaarCsvRegelTekst("Software", ApplicatieNaam);
    }
}
