using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleOverervingOefenblad.Exercises.Classes.Klant
{
    internal class ProfessioneleKlant : Klant
    {
        public string Bedrijfsnaam { get; set; }
        public string BtwNummer { get; set; }

        public override string ToString()
        {
            return base.ToString() + $" - {Bedrijfsnaam} (BTW: {BtwNummer})";
        }
    }
}
