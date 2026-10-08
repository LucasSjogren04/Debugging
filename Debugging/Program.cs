using System;
using System.IO;
using System.Numerics;

class Program
{
    static void Main(string[] args)
    {
        bool isRunning = true;

        try
        {
            while (isRunning)
            {
                Console.WriteLine("\nVälkommen till huvudmenyn!");
                Console.WriteLine("Skriv en siffra för att välja en funktion.");

                int choice = NumberChecker.ReadNumber<int>(
                    "0: Avsluta\n1: Visa pris för en person\n2: Visa pris för flera personer\n3: Upprepa text tio gånger\n4: Visa det tredje ordet",
                    value => true);

                switch (choice)
                {
                    case 0:
                        isRunning = false;
                        break;
                    case 1:
                        NumberChecker.ShowPrice();
                        break;
                    case 2:
                        NumberChecker.ShowTotalPrice();
                        break;
                    case 3:
                        RepeatText();
                        break;
                    case 4:
                        ShowThirdWord();
                        break;
                    default:
                        Console.WriteLine("Ogiltigt menyval. Välj en siffra mellan 0 och 4.");
                        break;
                }
            }
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("\nInmatningen stängdes. Programmet avslutas.");
        }
    }

    static void RepeatText()
    {
        string text = NumberChecker.ReadText("Skriv en text som ska upprepas:");

        for (int i = 1; i <= 10; i++)
        {
            Console.WriteLine($"{i}. {text}");
        }
        Console.WriteLine();
    }

    static void ShowThirdWord()
    {
        while (true)
        {
            string sentence = NumberChecker.ReadText("Skriv en mening med minst tre ord:");

            var words = sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length >= 3)
            {
                Console.WriteLine($"Det tredje ordet är: {words[2]}");
                return;
            }

            Console.WriteLine("Meningen måste innehålla minst tre ord. Försök igen.");
        }
    }
}

static class NumberChecker
{
    public static int CheckPrice(int age)
    {
        if (age < 5 || age > 100)
        {
            Console.WriteLine("Gratis inträde: 0kr");
            return 0;
        }

        if (age < 20)
        {
            Console.WriteLine("Ungdomspris: 80kr");
            return 80;
        }
        else
        {
            if (age > 64)
            {
                Console.WriteLine("Pensionärspris: 90kr");
                return 90;
            }
            else
            {
                Console.WriteLine("Standardpris: 120kr");
                return 120;
            }
        }
    }

    public static T ReadNumber<T>(string prompt, Func<T, bool> isValid)
        where T : struct, INumber<T>
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine() ?? throw new EndOfStreamException();

            if (T.TryParse(input, null, out T value) && isValid(value))
            {
                return value;
            }

            Console.WriteLine("Ogiltig inmatning, försök igen.");
        }
    }

    public static string ReadText(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine() ?? throw new EndOfStreamException();
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            Console.WriteLine("Texten får inte vara tom. Försök igen.");
        }
    }

    public static void ShowPrice()
    {
        int age = ReadNumber<int>("Skriv din ålder (0–130):", value => value >= 0 && value <= 130);
        CheckPrice(age);
    }

    public static void ShowTotalPrice()
    {
        int people = ReadNumber<int>("Hur många personer ska gå på bio?", value => value >= 1);

        long totalPrice = 0;
        for (int i = 0; i < people; i++)
        {
            int age = ReadNumber<int>($"Skriv åldern för person {i + 1} (0–130):",
                value => value >= 0 && value <= 130);
            totalPrice += CheckPrice(age);
        }

        Console.WriteLine($"Antal personer: {people}");
        Console.WriteLine($"Totalkostnad: {totalPrice}kr");
    }
}
