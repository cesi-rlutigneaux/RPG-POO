using RPG_POO.Models;
using RPG_POO.Models.Heroes;
using RPG_POO.Models.Items;
using RPG_POO.Models.Monsters;
using RPG_POO.Services;

namespace RPG_POO
{
    internal static class Program
    {
        static void Main()
        {
            Console.WriteLine("TP3 - Interfaces + Composition (inventaire, loot, potions)");
            ConsoleUi.Separator();

            string heroName = ConsoleUi.AskNonEmpty("Nom du héros : ");

            Console.WriteLine("Classe : 1) Warrior  2) Mage  3) Rogue");
            Console.Write("> ");
            string? c = Console.ReadLine()?.Trim();

            Hero hero = c switch
            {
                "2" => new Mage(heroName),
                "3" => new Rogue(heroName),
                _ => new Warrior(heroName)
            };

            // Kit de départ
            hero.Equip(new Weapon("Training Sword", 1));
            hero.Equip(new Armor("Leather Vest", 1));
            hero.Inventory.Add(new Potion("Small Potion", 6));
            hero.Inventory.Add(new Potion("Small Potion", 6));

            Console.WriteLine("Monstre : 1) Goblin  2) Dragon");
            Console.Write("> ");
            string? m = Console.ReadLine()?.Trim();

            Monster monster = m == "2" ? new Dragon(level: 1) : new Goblin(level: 1);

            CombatService.Fight(hero, monster);
        }
    }
}