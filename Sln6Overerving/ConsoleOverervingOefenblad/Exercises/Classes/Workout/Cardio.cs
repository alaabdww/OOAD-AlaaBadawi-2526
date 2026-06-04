namespace ConsoleOverervingOefenblad.Exercises.Classes.Workout;

internal class Cardio : Workout
{
   public double AfstandInKm { get; set; }
   public override int Punten => (int)(AfstandInKm * 6);

}
