using RPG_POO.Models;

namespace RPG_POO
{
    internal static class Program
    {
        static void Main()
        {
            var slime = new Monster("Slime");

            Console.WriteLine(slime);

            slime.TakeDamage(10);

            Console.WriteLine("Après 10 dégâts :");
            Console.WriteLine(slime);
        }
    }
}