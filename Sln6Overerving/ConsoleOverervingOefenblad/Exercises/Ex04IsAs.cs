using ConsoleOverervingOefenblad.Exercises.Classes.Workout;

namespace ConsoleOverervingOefenblad.Exercises;

/// <summary>
/// Oefening 4 — is en as: type controle en veilige cast bij overerving
/// </summary>
internal static class Ex04IsAs
{
   public static void Run()
   {
      Console.WriteLine("Oefening 4: is en as");
      Console.WriteLine("-------------");

      List<Workout> workouts = new List<Workout>
        {
            new Cardio
            {
                Naam = "Ochtendrun",
                Beschrijving = "Rustig tempo door het park",
                AfstandInKm = 5.2
            },
            new Krachttraining
            {
                Naam = "Bench press",
                Beschrijving = "Borstspieren",
                Gewicht = 60,
                Reps = 12
            },
            new Stretching
            {
                Naam = "Rugstretching",
                Beschrijving = "Na het tillen",
                LichaamsDeel = LichaamsDeel.Rug
            },
            new Cardio
            {
                Naam = "Fietstocht",
                Beschrijving = "Intervaltraining",
                AfstandInKm = 22.0
            },
            new Krachttraining
            {
                Naam = "Squat",
                Beschrijving = "Beenspieren",
                Gewicht = 80,
                Reps = 8
            },
            new Stretching
            {
                Naam = "Nekrol",
                Beschrijving = "Ontspanning na beeldschermwerk",
                LichaamsDeel = LichaamsDeel.Nek
            },
        };

      // TODO 1: toon info per workout met is/as of pattern matching

      Console.WriteLine("Overzicht workouts:");
      foreach (Workout wrkout in workouts)
      {
         if (wrkout is Cardio)
         {
            Cardio cd = wrkout as Cardio;
            Console.WriteLine($"[Cardio]        {cd.Naam} - {cd.AfstandInKm} km");
         } if (wrkout is Krachttraining)
         {
            Krachttraining kt = wrkout as Krachttraining;
            Console.WriteLine($"[Krachttraining]{kt.Naam} - {kt.Gewicht} kg x {kt.Reps} reps");
         } if (wrkout is Stretching)
         {
            Stretching st = wrkout as Stretching;
            Console.WriteLine($"[Stretching]    {st.Naam} - {st.LichaamsDeel}");
         }
      }

      // TODO 2: bereken en toon de totale punten per type (Cardio, Krachttraining, Stretching)
      int CardioPunten = 0;
      int KrachtPunten = 0;
      int StretchPunten = 0;

      foreach (Workout wt in workouts)
      {
         if (wt is Cardio)
         {
            Cardio cd = wt as Cardio;
            CardioPunten += cd.Punten;
         } if (wt is Krachttraining)
         {
            Krachttraining kt = wt as Krachttraining;
            KrachtPunten += kt.Punten;
         } if (wt is Stretching)
         {
            Stretching st = wt as Stretching;
            StretchPunten += st.Punten;
         }
      }
      Console.WriteLine();
      Console.WriteLine("Totale punten per type:");
      Console.WriteLine($"  Cardio:         {CardioPunten}");
      Console.WriteLine($"  Krachttraining: {KrachtPunten}");
      Console.WriteLine($"  Stretching:     {StretchPunten}");
   }
}
