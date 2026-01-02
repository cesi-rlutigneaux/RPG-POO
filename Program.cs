using RPG_POO.Models;
using RPG_POO.Services;

namespace RPG_POO
{
    internal static class Program
    {
        static void Main()
        {
            Console.WriteLine("TP1 - Fondations POO");
            ConsoleUi.Separator();

            string heroName = ConsoleUi.AskNonEmpty("Nom du héros : ");
            Hero hero = new Hero(heroName);

            Console.Write("Niveau du monstre (1..3) : ");
            string? lvlText = Console.ReadLine();
            int level = 1;
            if (!int.TryParse(lvlText, out level)) level = 1;
            if (level < 1) level = 1;
            if (level > 3) level = 3;

            Monster monster = new Monster("Goblin", level);

            ConsoleUi.Separator();
            Console.WriteLine("Créés :");
            Console.WriteLine(hero);
            Console.WriteLine(monster);

            ConsoleUi.Separator();
            Console.WriteLine("Test : 1 attaque du héros, puis 1 attaque du monstre");
            ConsoleUi.Separator();

            int heroDmg = hero.ComputeDamage();
            monster.TakeDamage(heroDmg);
            Console.WriteLine($"{hero.Name} attaque et inflige {heroDmg} dégâts bruts.");
            Console.WriteLine(monster);

            if (monster.IsAlive)
            {
                int monsterDmg = monster.ComputeDamage();
                hero.TakeDamage(monsterDmg);
                Console.WriteLine($"{monster.Name} attaque et inflige {monsterDmg} dégâts bruts.");
                Console.WriteLine(hero);
            }

            ConsoleUi.Separator();
            Console.WriteLine("Fin TP1.");
        }
    }
}