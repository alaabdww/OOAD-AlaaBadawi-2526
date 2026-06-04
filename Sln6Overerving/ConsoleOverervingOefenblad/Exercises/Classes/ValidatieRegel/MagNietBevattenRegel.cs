using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleOverervingOefenblad.Exercises.Classes.ValidatieRegel
{
   internal class MagNietBevattenRegel : ValidatieRegel

   {
      private readonly List<string> _verbodenWoorden;

      public MagNietBevattenRegel(List<string> verbodenWoorden)
      {
         _verbodenWoorden = verbodenWoorden;
      }

      public override bool IsGeldig(string waarde)
      {
         foreach (string woord in _verbodenWoorden)
         {
            if (waarde.ToLower().Contains(woord.ToLower()))
            {
               return false;
            }
         }
         return true;
      }

      public override string FoutBoodschap => $"Waarde mag geen van de volgende woorden bevatten: {string.Join(", ", _verbodenWoorden)}";
   }
}
