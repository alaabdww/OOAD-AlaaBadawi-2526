using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CLHelpDesk;

namespace WpfHelpDesk
{
    /// <summary>
    /// Hoofdvenster van de IT-helpdesk: filters, ticketlijst, details en nieuw ticket.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Alle tickets uit het CSV-bestand (in geheugen tijdens deze sessie)
        private List<Ticket> _alleTickets = new List<Ticket>();

        public MainWindow()
        {
            InitializeComponent();
            KoppelEvents();
        }

        // =============
        // EVENTS KOPPELEN
        // =============

        /// <summary>
        /// Koppelt alle events in code-behind (geen inline events in XAML).
        /// </summary>
        private void KoppelEvents()
        {
            Loaded += MainWindow_Loaded;

            // Filters en lijst
            lbxTickets.SelectionChanged += LbxTickets_SelectionChanged;
            cmbFilterPrioriteit.SelectionChanged += Filter_SelectionChanged;
            cmbFilterMelder.SelectionChanged += Filter_SelectionChanged;
            chkAlleenOpen.Checked += Filter_SelectionChanged;
            chkAlleenOpen.Unchecked += Filter_SelectionChanged;

            // Knoppen
            btnTicketAfsluiten.Click += BtnTicketAfsluiten_Click;
            btnToevoegen.Click += BtnToevoegen_Click;

            // Validatie bij wijziging nieuw-ticketformulier
            txtNieuweTitel.TextChanged += NieuwTicketVeld_Gewijzigd;
            txtNieuweToestel.TextChanged += NieuwTicketVeld_Gewijzigd;
            cmbNieuweMelder.SelectionChanged += NieuwTicketVeld_Gewijzigd;
            cmbNieuweMelder.LostFocus += NieuwTicketVeld_Gewijzigd;
            cmbNieuwePrioriteit.SelectionChanged += NieuwTicketVeld_Gewijzigd;
            cmbNieuweType.SelectionChanged += NieuwTicketVeld_Gewijzigd;
        }

        /// <summary>
        /// Startinitialisatie na laden van het venster.
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            VulPrioriteitComboboxen();
            VulTypeCombobox();
            LaadTickets();
            HerstelKnoppen();
        }

        // =============
        // COMBOBOX HULP
        // =============

        /// <summary>
        /// Voegt één ComboBoxItem toe aan een ComboBox.
        /// </summary>
        private void VoegComboBoxItemToe(ComboBox comboBox, string tekst)
        {
            ComboBoxItem item = new ComboBoxItem();
            item.Content = tekst;
            comboBox.Items.Add(item);
        }

        /// <summary>
        /// Leest de tekst van het geselecteerde ComboBoxItem.
        /// </summary>
        private string LeesComboBoxTekst(ComboBox comboBox)
        {
            if (comboBox.SelectedItem == null)
            {
                return null;
            }

            ComboBoxItem selectie = (ComboBoxItem)comboBox.SelectedItem;
            return selectie.Content.ToString();
        }

        /// <summary>
        /// Selecteert een ComboBoxItem op basis van de inhoud.
        /// </summary>
        private void SelecteerComboBoxItem(ComboBox comboBox, string tekst)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                ComboBoxItem item = (ComboBoxItem)comboBox.Items[i];
                if (item.Content.ToString() == tekst)
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }

            comboBox.SelectedIndex = 0;
        }

        // =============
        // COMBOBOXEN VULLEN
        // =============

        /// <summary>
        /// Vult de prioriteit-comboboxen voor filter en nieuw ticket.
        /// </summary>
        private void VulPrioriteitComboboxen()
        {
            cmbFilterPrioriteit.Items.Clear();
            VoegComboBoxItemToe(cmbFilterPrioriteit, "Alle");
            VoegComboBoxItemToe(cmbFilterPrioriteit, "Laag");
            VoegComboBoxItemToe(cmbFilterPrioriteit, "Normaal");
            VoegComboBoxItemToe(cmbFilterPrioriteit, "Hoog");
            cmbFilterPrioriteit.SelectedIndex = 0;

            cmbNieuwePrioriteit.Items.Clear();
            VoegComboBoxItemToe(cmbNieuwePrioriteit, "Laag");
            VoegComboBoxItemToe(cmbNieuwePrioriteit, "Normaal");
            VoegComboBoxItemToe(cmbNieuwePrioriteit, "Hoog");
            cmbNieuwePrioriteit.SelectedIndex = 1;
        }

        /// <summary>
        /// Vult de type-combobox voor nieuw ticket (Hardware / Software).
        /// </summary>
        private void VulTypeCombobox()
        {
            cmbNieuweType.Items.Clear();
            VoegComboBoxItemToe(cmbNieuweType, "Hardware");
            VoegComboBoxItemToe(cmbNieuweType, "Software");
            cmbNieuweType.SelectedIndex = 0;
        }

        /// <summary>
        /// Laadt tickets uit de class library en ververst de UI.
        /// </summary>
        private void LaadTickets()
        {
            _alleTickets = Ticket.LeesAlle();
            VulMelderComboboxen();
            PasFiltersToe();
        }

        /// <summary>
        /// Vult melder-comboboxen met unieke melders uit de ticketlijst.
        /// </summary>
        private void VulMelderComboboxen()
        {
            List<string> melders = HaalUniekeMelders(_alleTickets);

            string geselecteerdeFilterMelder = LeesComboBoxTekst(cmbFilterMelder);

            cmbFilterMelder.Items.Clear();
            VoegComboBoxItemToe(cmbFilterMelder, "Alle");
            for (int i = 0; i < melders.Count; i++)
            {
                VoegComboBoxItemToe(cmbFilterMelder, melders[i]);
            }

            if (geselecteerdeFilterMelder != null)
            {
                SelecteerComboBoxItem(cmbFilterMelder, geselecteerdeFilterMelder);
            }
            else
            {
                cmbFilterMelder.SelectedIndex = 0;
            }

            string geselecteerdeNieuweMelder = cmbNieuweMelder.Text;
            cmbNieuweMelder.Items.Clear();
            for (int i = 0; i < melders.Count; i++)
            {
                VoegComboBoxItemToe(cmbNieuweMelder, melders[i]);
            }

            if (geselecteerdeNieuweMelder != string.Empty)
            {
                cmbNieuweMelder.Text = geselecteerdeNieuweMelder;
            }
        }

        // =============
        // FILTERS EN LIJST
        // =============

        /// <summary>
        /// Past actieve filters toe en ververst de ListBox.
        /// </summary>
        private void PasFiltersToe()
        {
            int geselecteerdId = -1;
            Ticket geselecteerdTicket = HaalGeselecteerdTicket();
            if (geselecteerdTicket != null)
            {
                geselecteerdId = geselecteerdTicket.Id;
            }

            List<Ticket> gefilterd = FilterTickets(_alleTickets);

            lbxTickets.Items.Clear();
            for (int i = 0; i < gefilterd.Count; i++)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = gefilterd[i];
                lbxTickets.Items.Add(item);
            }

            // Probeer vorige selectie te behouden na filterwijziging
            if (geselecteerdId >= 0)
            {
                for (int i = 0; i < lbxTickets.Items.Count; i++)
                {
                    ListBoxItem item = (ListBoxItem)lbxTickets.Items[i];
                    Ticket ticket = (Ticket)item.Content;
                    if (ticket.Id == geselecteerdId)
                    {
                        lbxTickets.SelectedIndex = i;
                        break;
                    }
                }
            }

            if (lbxTickets.SelectedItem == null)
            {
                ToonTicketDetails(null);
            }

            HerstelKnoppen();
        }

        /// <summary>
        /// Filtert tickets op prioriteit, melder en open/gesloten status.
        /// </summary>
        private List<Ticket> FilterTickets(List<Ticket> bron)
        {
            List<Ticket> resultaat = new List<Ticket>();
            string gekozenPrioriteit = LeesComboBoxTekst(cmbFilterPrioriteit);
            string gekozenMelder = LeesComboBoxTekst(cmbFilterMelder);
            bool alleenOpen = chkAlleenOpen.IsChecked == true;

            for (int i = 0; i < bron.Count; i++)
            {
                Ticket ticket = bron[i];

                if (alleenOpen && ticket.IsAfgesloten)
                {
                    continue;
                }

                if (gekozenPrioriteit != null && gekozenPrioriteit != "Alle"
                    && ticket.Prioriteit.ToString() != gekozenPrioriteit)
                {
                    continue;
                }

                if (gekozenMelder != null && gekozenMelder != "Alle"
                    && ticket.Melder != gekozenMelder)
                {
                    continue;
                }

                resultaat.Add(ticket);
            }

            return resultaat;
        }

        /// <summary>
        /// Geeft het geselecteerde ticket uit de ListBox terug.
        /// </summary>
        private Ticket HaalGeselecteerdTicket()
        {
            if (lbxTickets.SelectedItem == null)
            {
                return null;
            }

            ListBoxItem selectie = (ListBoxItem)lbxTickets.SelectedItem;
            return (Ticket)selectie.Content;
        }

        private void LbxTickets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToonTicketDetails(HaalGeselecteerdTicket());
            HerstelKnoppen();
        }

        private void Filter_SelectionChanged(object sender, RoutedEventArgs e)
        {
            PasFiltersToe();
        }

        // =============
        // DETAILS WEERGAVE
        // =============

        /// <summary>
        /// Toont ticketdetails en werkt validatiemelding bij geselecteerd ticket bij.
        /// </summary>
        private void ToonTicketDetails(Ticket ticket)
        {
            if (ticket == null)
            {
                txtDetails.Text = "Selecteer een ticket in de lijst.";
                txtValidatieTicket.Text = string.Empty;
                return;
            }

            txtDetails.Text = ticket.GeefInfo();

            if (ticket.IsAfgesloten)
            {
                ToonValidatie(txtValidatieTicket, "Dit ticket is afgesloten en kan niet opnieuw worden gesloten.", false);
            }
            else
            {
                ToonValidatie(txtValidatieTicket, "Open ticket — je kan dit ticket afsluiten.", true);
            }
        }

        /// <summary>
        /// Geeft alle unieke melders uit een ticketlijst terug.
        /// </summary>
        private List<string> HaalUniekeMelders(List<Ticket> tickets)
        {
            List<string> melders = new List<string>();

            for (int i = 0; i < tickets.Count; i++)
            {
                string melder = tickets[i].Melder;
                if (melder == string.Empty)
                {
                    continue;
                }

                if (!ZitMelderInLijst(melders, melder))
                {
                    melders.Add(melder);
                }
            }

            return melders;
        }

        /// <summary>
        /// Controleert of een melder al in de lijst staat.
        /// </summary>
        private bool ZitMelderInLijst(List<string> melders, string melder)
        {
            for (int i = 0; i < melders.Count; i++)
            {
                if (melders[i] == melder)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Toont een validatiemelding in groen (geldig) of rood (fout).
        /// </summary>
        private void ToonValidatie(TextBlock textBlock, string tekst, bool isGeldig)
        {
            textBlock.Text = tekst;
            textBlock.Foreground = isGeldig ? Brushes.DarkGreen : Brushes.DarkRed;
        }

        // =============
        // VALIDATIE
        // =============

        /// <summary>
        /// Controleert of het nieuw-ticketformulier volledig en geldig is ingevuld.
        /// Geeft een foutmelding terug; lege string betekent geldig.
        /// </summary>
        private string ValideerNieuwTicketFormulier()
        {
            string titel = txtNieuweTitel.Text.Trim();
            string melder = cmbNieuweMelder.Text.Trim();
            string toestel = txtNieuweToestel.Text.Trim();
            string prioriteitTekst = LeesComboBoxTekst(cmbNieuwePrioriteit);
            string type = LeesComboBoxTekst(cmbNieuweType);

            if (titel == string.Empty)
            {
                return "Titel is verplicht.";
            }

            if (titel.Length < 3)
            {
                return "Titel moet minstens 3 tekens bevatten.";
            }

            if (melder == string.Empty)
            {
                return "Melder is verplicht.";
            }

            if (melder.Length < 2)
            {
                return "Melder moet minstens 2 tekens bevatten.";
            }

            if (toestel == string.Empty)
            {
                return "Toestel is verplicht.";
            }

            if (toestel.Length < 2)
            {
                return "Toestel moet minstens 2 tekens bevatten.";
            }

            if (prioriteitTekst == null)
            {
                return "Kies een prioriteit.";
            }

            if (type == null)
            {
                return "Kies een type (Hardware of Software).";
            }

            if (type != "Hardware" && type != "Software")
            {
                return "Type moet Hardware of Software zijn.";
            }

            return string.Empty;
        }

        /// <summary>
        /// Bepaalt of de knop Ticket afsluiten actief mag zijn.
        /// Enkel enabled bij een geselecteerd, nog open ticket.
        /// </summary>
        private bool MagTicketAfsluiten()
        {
            Ticket ticket = HaalGeselecteerdTicket();
            if (ticket == null)
            {
                return false;
            }

            return !ticket.IsAfgesloten;
        }

        /// <summary>
        /// Bepaalt of de knop Toevoegen actief mag zijn.
        /// Enkel enabled als alle velden geldig zijn ingevuld.
        /// </summary>
        private bool MagTicketToevoegen()
        {
            return ValideerNieuwTicketFormulier() == string.Empty;
        }

        /// <summary>
        /// Werkt validatiemeldingen en enabled-state van alle knoppen bij.
        /// </summary>
        private void HerstelKnoppen()
        {
            // Knop afsluiten: enabled enkel bij open, geselecteerd ticket
            btnTicketAfsluiten.IsEnabled = MagTicketAfsluiten();

            // Knop toevoegen: enabled enkel bij geldig formulier
            string foutNieuw = ValideerNieuwTicketFormulier();
            btnToevoegen.IsEnabled = foutNieuw == string.Empty;

            if (foutNieuw == string.Empty)
            {
                ToonValidatie(txtValidatieNieuw, "Formulier is geldig — je kan het ticket toevoegen.", true);
            }
            else
            {
                ToonValidatie(txtValidatieNieuw, foutNieuw, false);
            }
        }

        /// <summary>
        /// Event bij wijziging van een veld in het nieuw-ticketformulier.
        /// </summary>
        private void NieuwTicketVeld_Gewijzigd(object sender, RoutedEventArgs e)
        {
            HerstelKnoppen();
        }

        // =============
        // ACTIES
        // =============

        /// <summary>
        /// Sluit het geselecteerde ticket af en slaat op in CSV.
        /// </summary>
        private void BtnTicketAfsluiten_Click(object sender, RoutedEventArgs e)
        {
            Ticket ticket = HaalGeselecteerdTicket();

            // Dubbele validatie (knop zou disabled moeten zijn, maar voor de zekerheid)
            if (ticket == null)
            {
                txtValidatieTicket.Text = "Selecteer eerst een ticket.";
                btnTicketAfsluiten.IsEnabled = false;
                return;
            }

            if (ticket.IsAfgesloten)
            {
                txtValidatieTicket.Text = "Dit ticket is al afgesloten.";
                btnTicketAfsluiten.IsEnabled = false;
                return;
            }

            ticket.SluitAf();
            Ticket.Wijzig(ticket);
            LaadTickets();

            ToonValidatie(txtValidatieTicket, "Ticket #" + ticket.Id + " is succesvol afgesloten.", true);

            HerstelKnoppen();
        }

        /// <summary>
        /// Maakt een nieuw ticket aan na validatie en slaat op in CSV.
        /// </summary>
        private void BtnToevoegen_Click(object sender, RoutedEventArgs e)
        {
            if (!MagTicketToevoegen())
            {
                ToonValidatie(txtValidatieNieuw, ValideerNieuwTicketFormulier(), false);
                btnToevoegen.IsEnabled = false;
                return;
            }

            string titel = txtNieuweTitel.Text.Trim();
            string melder = cmbNieuweMelder.Text.Trim();
            string toestel = txtNieuweToestel.Text.Trim();
            string prioriteitTekst = LeesComboBoxTekst(cmbNieuwePrioriteit);
            string type = LeesComboBoxTekst(cmbNieuweType);
            TicketPrioriteit prioriteit = Ticket.ParsePrioriteit(prioriteitTekst);
            int nieuwId = Ticket.GenereerNieuwId();

            Ticket nieuwTicket = Ticket.MaakNieuwTicket(nieuwId, titel, melder, prioriteit, type, toestel);
            Ticket.VoegToe(nieuwTicket);

            txtNieuweTitel.Clear();
            txtNieuweToestel.Clear();
            cmbNieuweMelder.Text = string.Empty;

            LaadTickets();

            ToonValidatie(txtValidatieNieuw, "Ticket #" + nieuwId + " is succesvol toegevoegd.", true);

            HerstelKnoppen();
        }
    }
}
