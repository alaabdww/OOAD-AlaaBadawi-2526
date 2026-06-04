namespace ConsoleOverervingOefenblad.Exercises.Classes.ValidatieRegel;

public abstract class ValidatieRegel
{
   public abstract bool IsGeldig(string waarde);
    public abstract string FoutBoodschap { get; }


}
