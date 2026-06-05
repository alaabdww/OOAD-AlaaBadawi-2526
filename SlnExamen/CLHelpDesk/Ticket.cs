using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CLHelpDesk;

/// <summary>
/// Basisklasse voor helpdesk-tickets. Bevat ook CSV-lezen en -schrijven.
/// </summary>
public abstract class Ticket
{
    private const string Bestandsnaam = "helpdesk_tickets.csv";
    private const char ScheidingsTeken = ';';
    private const string DatumFormaat = "dd/MM/yyyy HH:mm";

    public int Id { get; set; }
    public string Titel { get; set; }
    public string Melder { get; set; }
    public TicketPrioriteit Prioriteit { get; set; }
    public bool IsAfgesloten { get; set; }
    public DateTime DatumAangemaakt { get; set; }
    public DateTime? DatumAfgesloten { get; set; }

    protected Ticket()
    {
        Titel = string.Empty;
        Melder = string.Empty;
        Prioriteit = TicketPrioriteit.Normaal;
        DatumAangemaakt = DateTime.Now;
    }

    protected Ticket(int id, string titel, string melder, TicketPrioriteit prioriteit)
    {
        Id = id;
        Titel = titel;
        Melder = melder;
        Prioriteit = prioriteit;
        IsAfgesloten = false;
        DatumAangemaakt = DateTime.Now;
        DatumAfgesloten = null;
    }

    public abstract string GeefInfo();

    public override string ToString()
    {
        string status = IsAfgesloten ? "Afgesloten" : "Open";
        return $"#{Id} — {Titel} ({Prioriteit}, {status})";
    }

    public void SluitAf()
    {
        IsAfgesloten = true;
        DatumAfgesloten = DateTime.Now;
    }

    // =============
    // CSV — LEZEN EN SCHRIJVEN
    // =============

    public static List<Ticket> LeesAlle()
    {
        List<Ticket> tickets = new List<Ticket>();
        string pad = GetBestandPad();

        if (!File.Exists(pad))
        {
            return tickets;
        }

        string[] regels;
        try
        {
            regels = File.ReadAllLines(pad);
        }
        catch (IOException)
        {
            return tickets;
        }

        for (int i = 0; i < regels.Length; i++)
        {
            string regel = regels[i].Trim().Trim('"').Trim('\uFEFF');
            if (regel == string.Empty)
            {
                continue;
            }

            Ticket ticket = MaakTicketUitRegel(regel);
            if (ticket != null)
            {
                tickets.Add(ticket);
            }
        }

        return tickets;
    }

    public static void SchrijfAlle(List<Ticket> tickets)
    {
        List<string> regels = new List<string>();
        regels.Add(MaakCsvHeader());

        for (int i = 0; i < tickets.Count; i++)
        {
            regels.Add(tickets[i].NaarCsvRegel());
        }

        try
        {
            File.WriteAllLines(GetBestandPad(), regels.ToArray());
        }
        catch (IOException)
        {
            // Schrijffout — geen actie mogelijk zonder UI; caller merkt het via ontbrekende wijziging
        }
    }

    public static void VoegToe(Ticket ticket)
    {
        List<Ticket> tickets = LeesAlle();
        tickets.Add(ticket);
        SchrijfAlle(tickets);
    }

    public static void Wijzig(Ticket ticket)
    {
        List<Ticket> tickets = LeesAlle();
        bool gevonden = false;

        for (int i = 0; i < tickets.Count; i++)
        {
            if (tickets[i].Id == ticket.Id)
            {
                tickets[i] = ticket;
                gevonden = true;
                break;
            }
        }

        if (gevonden)
        {
            SchrijfAlle(tickets);
        }
    }

    public static Ticket ZoekOpId(int id)
    {
        List<Ticket> tickets = LeesAlle();

        for (int i = 0; i < tickets.Count; i++)
        {
            if (tickets[i].Id == id)
            {
                return tickets[i];
            }
        }

        return null;
    }

    public static int GenereerNieuwId()
    {
        List<Ticket> tickets = LeesAlle();
        int hoogsteId = 0;

        for (int i = 0; i < tickets.Count; i++)
        {
            if (tickets[i].Id > hoogsteId)
            {
                hoogsteId = tickets[i].Id;
            }
        }

        return hoogsteId + 1;
    }

    /// <summary>
    /// Maakt een nieuw Hardware- of Software-ticket aan op basis van het type.
    /// </summary>
    public static Ticket MaakNieuwTicket(int id, string titel, string melder, TicketPrioriteit prioriteit, string type, string toestel)
    {
        if (type == "Hardware")
        {
            return new HardwareTicket(id, titel, melder, prioriteit, toestel, string.Empty);
        }

        return new SoftwareTicket(id, titel, melder, prioriteit, toestel, string.Empty);
    }

    /// <summary>
    /// Zet combobox- of CSV-tekst om naar TicketPrioriteit.
    /// </summary>
    public static TicketPrioriteit ParsePrioriteit(string waarde)
    {
        if (waarde == "Laag")
        {
            return TicketPrioriteit.Laag;
        }

        if (waarde == "Hoog")
        {
            return TicketPrioriteit.Hoog;
        }

        return TicketPrioriteit.Normaal;
    }

    protected string GeefBasisInfo()
    {
        string afgeslotenOp = DatumAfgesloten.HasValue
            ? DatumAfgesloten.Value.ToString(DatumFormaat, CultureInfo.InvariantCulture)
            : "—";

        return $@"Ticket #{Id}
Titel: {Titel}
Melder: {Melder}
Prioriteit: {Prioriteit}
Status: {(IsAfgesloten ? "Afgesloten" : "Open")}
Aangemaakt op: {DatumAangemaakt.ToString(DatumFormaat, CultureInfo.InvariantCulture)}
Afgesloten op: {afgeslotenOp}";
    }

    protected abstract string NaarCsvRegel();

    protected static string GetBestandPad()
    {
        // CSV staat naast de .exe (bin\Debug) — gekopieerd vanuit de solution-map
        string map = AppDomain.CurrentDomain.BaseDirectory;
        return Path.Combine(map, Bestandsnaam);
    }

    // =============
    // CSV — PARSEN
    // =============

    private static string MaakCsvHeader()
    {
        return "\"id;titel;melderVoornaam;melderAchternaam;melderId;prioriteit;isAfgesloten;type;extraInfo;datumAangemaakt;datumAfgesloten\"";
    }

    private static Ticket MaakTicketUitRegel(string regel)
    {
        string[] velden = regel.Split(ScheidingsTeken);

        if (velden.Length < 10 || velden[0] == "id")
        {
            return null;
        }

        int id = int.Parse(velden[0]);
        string titel = velden[1];
        string melder = velden[2] + " " + velden[3];
        TicketPrioriteit prioriteit = ParsePrioriteit(velden[5]);
        bool isAfgesloten = bool.Parse(velden[6]);
        string type = velden[7];
        string extraInfo = velden[8];
        DateTime datumAangemaakt = ParseCsvDatum(velden[9]);
        DateTime? datumAfgesloten = null;

        if (velden.Length >= 11 && velden[10] != string.Empty)
        {
            datumAfgesloten = ParseCsvDatum(velden[10]);
        }

        if (type == "Hardware")
        {
            HardwareTicket ticket = new HardwareTicket(id, titel, melder, prioriteit, extraInfo, string.Empty);
            StelTicketVeldenIn(ticket, isAfgesloten, datumAangemaakt, datumAfgesloten);
            return ticket;
        }

        if (type == "Software")
        {
            SoftwareTicket ticket = new SoftwareTicket(id, titel, melder, prioriteit, extraInfo, string.Empty);
            StelTicketVeldenIn(ticket, isAfgesloten, datumAangemaakt, datumAfgesloten);
            return ticket;
        }

        return null;
    }

    private static void StelTicketVeldenIn(Ticket ticket, bool isAfgesloten, DateTime datumAangemaakt, DateTime? datumAfgesloten)
    {
        ticket.IsAfgesloten = isAfgesloten;
        ticket.DatumAangemaakt = datumAangemaakt;
        ticket.DatumAfgesloten = datumAfgesloten;
    }

    protected string NaarCsvRegelTekst(string type, string extraInfo)
    {
        string[] melderDelen = SplitMelder(Melder);
        string voornaam = melderDelen[0];
        string achternaam = melderDelen[1];

        string melderId = MaakMelderId(voornaam, achternaam);
        string datumAangemaakt = NaarCsvDatum(DatumAangemaakt);
        string datumAfgesloten = DatumAfgesloten.HasValue ? NaarCsvDatum(DatumAfgesloten.Value) : string.Empty;

        string inhoud = string.Join(ScheidingsTeken.ToString(), new string[]
        {
            Id.ToString(),
            Titel,
            voornaam,
            achternaam,
            melderId,
            Prioriteit.ToString(),
            IsAfgesloten.ToString().ToLower(),
            type,
            extraInfo,
            datumAangemaakt,
            datumAfgesloten
        });

        return "\"" + inhoud + "\"";
    }

    private static string[] SplitMelder(string melder)
    {
        melder = melder.Trim();
        int spatieIndex = melder.IndexOf(' ');

        if (spatieIndex < 0)
        {
            return new string[] { melder, string.Empty };
        }

        return new string[] { melder.Substring(0, spatieIndex), melder.Substring(spatieIndex + 1) };
    }

    private static string MaakMelderId(string voornaam, string achternaam)
    {
        if (voornaam == string.Empty)
        {
            return string.Empty;
        }

        string id = voornaam.Substring(0, 1).ToLower();

        if (achternaam != string.Empty)
        {
            id = id + achternaam.ToLower();
        }

        return id;
    }

    private static DateTime ParseCsvDatum(string waarde)
    {
        waarde = waarde.Trim();
        if (waarde == string.Empty)
        {
            return DateTime.Now;
        }

        string[] delen = waarde.Split(' ');
        string datumDeel = delen[0];
        string tijdDeel = "0000";

        if (delen.Length >= 2)
        {
            tijdDeel = delen[1].PadLeft(4, '0');
        }

        int uur = int.Parse(tijdDeel.Substring(0, 2));
        int minuut = int.Parse(tijdDeel.Substring(2, 2));
        DateTime datum = DateTime.ParseExact(datumDeel, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        return new DateTime(datum.Year, datum.Month, datum.Day, uur, minuut, 0);
    }

    private static string NaarCsvDatum(DateTime datum)
    {
        return datum.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            + " "
            + datum.ToString("HHmm", CultureInfo.InvariantCulture);
    }
}
