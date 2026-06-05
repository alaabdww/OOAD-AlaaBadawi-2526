using System;
using System.Collections.Generic;
using System.IO;

namespace CLHelpDesk;

/// <summary>
/// IT-medewerker die tickets kan beheren (aggregatie met List&lt;Ticket&gt;).
/// </summary>
public class Medewerker
{
    private const string Bestandsnaam = "helpdesk_medewerkers.csv";
    private const char ScheidingsTeken = ';';
    private const char TicketIdScheiding = '|';

    public int Id { get; set; }
    public string Voornaam { get; set; }
    public List<Ticket> Tickets { get; set; }

    public Medewerker()
    {
        Voornaam = string.Empty;
        Tickets = new List<Ticket>();
    }

    public Medewerker(int id, string voornaam)
    {
        Id = id;
        Voornaam = voornaam;
        Tickets = new List<Ticket>();
    }

    public override string ToString()
    {
        return $"{Voornaam} ({Tickets.Count} ticket(s))";
    }

    public void VoegTicketToe(Ticket ticket)
    {
        for (int i = 0; i < Tickets.Count; i++)
        {
            if (Tickets[i].Id == ticket.Id)
            {
                return;
            }
        }

        Tickets.Add(ticket);
    }

    public void VerwijderTicket(int ticketId)
    {
        for (int i = Tickets.Count - 1; i >= 0; i--)
        {
            if (Tickets[i].Id == ticketId)
            {
                Tickets.RemoveAt(i);
                break;
            }
        }
    }

    public static List<Medewerker> LeesAlle()
    {
        List<Medewerker> medewerkers = new List<Medewerker>();
        string pad = GetBestandPad();

        if (!File.Exists(pad))
        {
            return medewerkers;
        }

        List<Ticket> alleTickets = Ticket.LeesAlle();
        string[] regels = File.ReadAllLines(pad);

        for (int i = 0; i < regels.Length; i++)
        {
            string regel = regels[i].Trim();
            if (regel == string.Empty)
            {
                continue;
            }

            Medewerker? medewerker = MaakMedewerkerUitRegel(regel, alleTickets);
            if (medewerker != null)
            {
                medewerkers.Add(medewerker);
            }
        }

        return medewerkers;
    }

    public static void SchrijfAlle(List<Medewerker> medewerkers)
    {
        List<string> regels = new List<string>();

        for (int i = 0; i < medewerkers.Count; i++)
        {
            regels.Add(medewerkers[i].NaarCsvRegel());
        }

        File.WriteAllLines(GetBestandPad(), regels.ToArray());
    }

    public static void VoegToe(Medewerker medewerker)
    {
        List<Medewerker> medewerkers = LeesAlle();
        medewerkers.Add(medewerker);
        SchrijfAlle(medewerkers);
    }

    public static void Wijzig(Medewerker medewerker)
    {
        List<Medewerker> medewerkers = LeesAlle();
        bool gevonden = false;

        for (int i = 0; i < medewerkers.Count; i++)
        {
            if (medewerkers[i].Id == medewerker.Id)
            {
                medewerkers[i] = medewerker;
                gevonden = true;
                break;
            }
        }

        if (gevonden)
        {
            SchrijfAlle(medewerkers);
        }
    }

    public static Medewerker? ZoekOpId(int id)
    {
        List<Medewerker> medewerkers = LeesAlle();

        for (int i = 0; i < medewerkers.Count; i++)
        {
            if (medewerkers[i].Id == id)
            {
                return medewerkers[i];
            }
        }

        return null;
    }

    public static int GenereerNieuwId()
    {
        List<Medewerker> medewerkers = LeesAlle();
        int hoogsteId = 0;

        for (int i = 0; i < medewerkers.Count; i++)
        {
            if (medewerkers[i].Id > hoogsteId)
            {
                hoogsteId = medewerkers[i].Id;
            }
        }

        return hoogsteId + 1;
    }

    private string NaarCsvRegel()
    {
        List<string> ticketIds = new List<string>();

        for (int i = 0; i < Tickets.Count; i++)
        {
            ticketIds.Add(Tickets[i].Id.ToString());
        }

        string ticketIdTekst = string.Join(TicketIdScheiding.ToString(), ticketIds.ToArray());

        return string.Join(ScheidingsTeken.ToString(), new string[]
        {
            Id.ToString(),
            Voornaam,
            ticketIdTekst
        });
    }

    private static string GetBestandPad()
    {
        string map = AppDomain.CurrentDomain.BaseDirectory;
        return Path.Combine(map, Bestandsnaam);
    }

    private static Medewerker? MaakMedewerkerUitRegel(string regel, List<Ticket> alleTickets)
    {
        string[] velden = regel.Split(ScheidingsTeken);

        if (velden.Length < 2)
        {
            return null;
        }

        int id = int.Parse(velden[0]);
        string voornaam = velden[1];
        Medewerker medewerker = new Medewerker(id, voornaam);

        if (velden.Length >= 3 && velden[2] != string.Empty)
        {
            string[] ticketIds = velden[2].Split(TicketIdScheiding);

            for (int i = 0; i < ticketIds.Length; i++)
            {
                int ticketId = int.Parse(ticketIds[i]);

                for (int j = 0; j < alleTickets.Count; j++)
                {
                    if (alleTickets[j].Id == ticketId)
                    {
                        medewerker.VoegTicketToe(alleTickets[j]);
                        break;
                    }
                }
            }
        }

        return medewerker;
    }
}
