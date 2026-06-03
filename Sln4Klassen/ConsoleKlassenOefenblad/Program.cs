using ConsoleKlassenOefenblad.Exercises;
using System.Runtime.InteropServices;

namespace ConsoleKlassenOefenblad
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // voor euroteken
            bool stoppen = false;
            while (!stoppen)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"
=== Oefenblad: Klassen ===

1. Open 'Assignments/index.html' in de browser; daar vind je de uitleg en opdrachten.
2. Elke oefening vind je in de map 'Exercises'.
3. De bijhorende klassen werk je uit in de map 'Exercises/Classes'
4. Start het programma en kies het nummer van de oefening om de testcode uit te voeren.
5. Controleer of de output exact klopt met de screenshot!");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine("Kies een oefening om uit te voeren:");
                Console.WriteLine();
                Console.WriteLine("1 - Lege class (Knikker)");
                Console.WriteLine("2 - Properties (Recept)");
                Console.WriteLine("3 - Niet-automatische properties, validatie, methodes (Werknemer)");
                Console.WriteLine("4 - ToString() override (Product)");
                Console.WriteLine("5 - Compositie (Bestelling)");
                Console.WriteLine("6 - Constructors (ProfielInfo)");
                Console.WriteLine();
                Console.Write("Je keuze (enter om te stoppen): ");
                char choice = Console.ReadKey().KeyChar;
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();

                switch (choice)
                {
                    case '1': Ex01LegeClass.Run(); break;
                    case '2': Ex02Properties.Run(); break;
                    case '3': Ex03ValidatieMethodes.Run(); break;
                    case '4': Ex04ToString.Run(); break;
                    case '5': Ex05Compositie.Run(); break;
                    case '6': Ex06Constructors.Run(); break;
                    default: stoppen = true; break;
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("druk een toets om verder te gaan...");
                Console.ReadKey();
            }
        }
    }
}
