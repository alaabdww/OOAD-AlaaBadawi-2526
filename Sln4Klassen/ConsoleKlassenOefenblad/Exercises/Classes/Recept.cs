using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ConsoleKlassenOefenblad.Exercises.Classes
{
    internal class Recept
    {
        public string Titel { get; set; }

        private int _rating;
        public int Rating
        {
            get { return _rating; }
            set
            {
                if (value > 5 || value < 0)
                {
                    throw new ArgumentOutOfRangeException("Rating moet tussen 0 en 5 liggen.");
                }
                _rating = value;
            }
        }

        public bool IsVegetarisch { get; set; } = false;

        public List<string> Ingredienten { get; set; } = new List<string>();
    }
}
