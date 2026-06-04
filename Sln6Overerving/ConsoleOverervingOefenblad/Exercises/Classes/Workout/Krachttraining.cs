namespace ConsoleOverervingOefenblad.Exercises.Classes.Workout;

internal class Krachttraining : Workout
{
   public double Gewicht { get; set; }
   public int Reps { get; set; }

   public override int Punten => (Reps * (int)Gewicht) / 5;
}
