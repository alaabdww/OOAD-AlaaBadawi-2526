namespace CLHelpDesk;

/// <summary>
/// Ticket voor hardwareproblemen (printer, laptop, muis, …).
/// </summary>
public class HardwareTicket : Ticket
{
    public string ApparaatType { get; set; }
    public string Serienummer { get; set; }

    public HardwareTicket()
    {
        ApparaatType = string.Empty;
        Serienummer = string.Empty;
    }

    public HardwareTicket(int id, string titel, string melder, TicketPrioriteit prioriteit, string apparaatType, string serienummer)
        : base(id, titel, melder, prioriteit)
    {
        ApparaatType = apparaatType;
        Serienummer = serienummer;
    }

    public override string GeefInfo()
    {
        return GeefBasisInfo() + $@"

Type: Hardware
Apparaat: {ApparaatType}
Serienummer: {Serienummer}";
    }

    protected override string NaarCsvRegel()
    {
        return NaarCsvRegelTekst("Hardware", ApparaatType);
    }
}
