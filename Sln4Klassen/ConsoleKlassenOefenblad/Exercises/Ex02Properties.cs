using ConsoleKlassenOefenblad.Exercises.Classes;

namespace ConsoleKlassenOefenblad.Exercises;

internal class Ex02Properties
{
    public static void Run()
    {
        Console.WriteLine("\nOefening 2: properties, standaardwaarden, object initializer syntax");
        Console.WriteLine("-------------");
        // 1. maak in "Exercises/Classes" een klasse "Recept" met volgende properties:
        //   - Titel
        //   - Rating (int)
        //   - IsVegetarisch (standaardwaarde is false)
        //   - Ingredienten (List van strings, standaard lege lijst)

        // 2. maak volgend recept aan met de lege constructor (... = new Recept()) en stel dan één voor één de properties in:
        //   - Pasta Carbonara (Rating 4, IsVegetarisch true, Ingrediënten: Pasta, Eieren, Spek, Parmezaanse kaas)
        Recept PastaCarbonara = new Recept();
        PastaCarbonara.Rating = 4;
        PastaCarbonara.IsVegetarisch = true;
        PastaCarbonara.Ingredienten = new List<string> { "Pasta", "Eieren", "Spek", "Parmezaanse kaas" };

        // 3. maak volgende recepten aan met de object initializer syntax:
        //   - Lasagne (Rating 5, IsVegetarisch false, Ingrediënten: Lasagnebladen, Tomatensaus, Courgette, Aubergine, Mozzarella)
        //   - Salade Niçoise (Rating 4, IsVegetarisch true, Ingrediënten: Sla, Tonijn, Eieren, Pindakaas, Olijven, Tomaten)
        Recept Lasagne = new Recept()
        {
            Rating = 5,
            IsVegetarisch = false,
            Ingredienten = new List<string> { "Lasagnebladen", "Tomatensaus", "Courgette", "Aubergine", "Mozzarella"}
        };

        Recept SaladeNicoise = new Recept()
        {
            Rating = 4,
            IsVegetarisch = true,
            Ingredienten = new List<string> { "Sla", "Tonijn", "Eieren", "Pindakaas", "Olijven", "Tomaten" }
        };


        // 4. pas het recept van de salade niçoise aan:
        //  - verwijder de pindakaas
        //  - zet IsVegetarisch op false
        SaladeNicoise.Ingredienten.Remove("Pindakaas");
        SaladeNicoise.IsVegetarisch = false;

        // 5. maak een lijst "kookboek" aan en voeg de drie recepten toe
        List<Recept> kookboek = new() {PastaCarbonara, Lasagne, SaladeNicoise };


        // 6. toon het aantal vegetarische recepten (zie screenshot) en de gemiddelde rating
        int aantalRecepten = 0;
        int totaalRating = 0;

        foreach (Recept rc in kookboek)
        {
            if (rc.IsVegetarisch == true)
            {
                aantalRecepten++;
                totaalRating = totaalRating + rc.Rating;
            }
        }

        Console.WriteLine($"Er zijn {aantalRecepten} recepten en een gemiddelde rating van {totaalRating / aantalRecepten}");
    }
}
