using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleKlassenOefenblad.Exercises.Classes
{
    internal class Werknemer
    {
        public int Id { get; set; }
        public string Naam { get; set; }

        public decimal Salaris
        {
            get;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Salaris kan niet negatief zijn");
                }
                field = value;
            }
        }
        public DateOnly InDienstSinds
        {
            get;
            set
            {
                if (value > DateOnly.FromDateTime(DateTime.Today))
                {
                    throw new ArgumentOutOfRangeException("Datum indiensttreding kan niet in de toekomst liggen");
                }
                field = value;
            }
        }

        public int Ancienniteit
        {
            get { return DateOnly.FromDateTime(DateTime.Today).Year - InDienstSinds.Year;  }
        }

        public string Seniority
        {
            get
            {
                if (Ancienniteit < 2)
                {
                    return "Junior";
                } else if (Ancienniteit < 5)
                {
                    return "Medior";
                } else
                {
                    return "Senior";
                }
            }
        }

        public void GeefOpslag(int perc)
        {
            this.Salaris = this.Salaris + (this.Salaris * perc / 100);
        }
    }
}
