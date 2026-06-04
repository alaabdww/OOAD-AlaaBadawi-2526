using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;


namespace ConsoleOverervingOefenblad.Exercises.Classes.ValidatieRegel
{
   public class BevatCijferRegel : ValidatieRegel
   {
      public override bool IsGeldig(string waarde)
      {
         foreach (char c in waarde)
         {
            if (char.IsDigit(c))
            {
               return true;
            }
         }
         return false;
      }


      public override string FoutBoodschap => "Waarde moet minstens 1 cijfer bevatten";
   }
}

