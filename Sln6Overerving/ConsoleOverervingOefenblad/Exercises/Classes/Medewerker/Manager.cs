using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleOverervingOefenblad.Exercises.Classes.Medewerker
{
    internal class Manager : Medewerker
    {
        public int TeamGrootte { get; set; }
        public Manager(string naam, string afdeling, int teamgrootte) : base(naam, afdeling)
        {
            TeamGrootte = teamgrootte;
        }

        public override string ToString() => base.ToString() + $", team: {TeamGrootte} personen";

    }
}
