namespace ConsoleKlassenOefenblad.Exercises.Classes
{
    public class ProfielInfo
    {
        // Properties (verplichte info)
        public int Id { get; set; }
        public string Gebruikersnaam { get; set; }
        public string Email { get; set; }
        public DateTime AanmaakDatum { get; private set; }

        // Properties (optionele info)
        public string Voornaam { get; set; } = "";
        public string Achternaam { get; set; } = "";
        public string Biografie { get; set; } = "";
        public string Website { get; set; } = "";
        public bool IsPubliek { get; set; } = true;

        // Berekende property IsVolledig
        public bool IsVolledig 
        {
            get
            {
                return Id > 0 &&
                    string.IsNullOrEmpty(Gebruikersnaam) &&
                    string.IsNullOrEmpty(Email);
            }
        }

        // Verplichte constructor — minimale gegevens om een geldig profiel te maken
        public ProfielInfo(int id, string nm, string mail)
        {
            Id = id;
            Gebruikersnaam = nm;
            Email = mail;
        }



        // Uitgebreide constructor — verplichte én optionele gegevens in één keer
        public ProfielInfo(int id, string nm, string mail, string vnm, string anm, string bio, string site, bool pub)
            : this(id, nm, mail)
        {
            Voornaam = vnm;
            Achternaam = anm;
            Biografie = bio;
            Website = site;
            IsPubliek = pub;
        }

        // ToString override
        public override string ToString()
        {
            return $"{Gebruikersnaam} - {(IsPubliek ? "Publiek" : "Privé")} | Profiel is {(IsVolledig ? "volledig" : "onvolledig")}";
        }
    }
}
