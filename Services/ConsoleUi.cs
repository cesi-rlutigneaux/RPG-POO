namespace RPG_POO.Services
{
    public static class ConsoleUi
    {
        public static string AskNonEmpty(string label)
        {
            while (true)
            {
                Console.Write(label);
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("Entrée invalide.");
            }
        }

        public static void Separator()
        {
            Console.WriteLine(new string('-', 50));
        }
    }
}
