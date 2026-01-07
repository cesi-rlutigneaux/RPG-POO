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
            SaveService.EnsureDbCreated();

            while (true)
            {
                ConsoleUi.Separator();
                Console.WriteLine("RPG - TP5 (sauvegarde)");
                Console.WriteLine("1) Nouvelle partie");
                Console.WriteLine("2) Reprendre (dernier point de sauvegarde)");
                Console.WriteLine("3) Supprimer la sauvegarde");
                Console.WriteLine("4) Quitter");
                Console.Write("> ");

                string? choice = Console.ReadLine()?.Trim();

                if (choice == "1")
                {
                    Hero hero = CreateNewHero();
                    SaveService.Save(hero); // première sauvegarde
                    GameService.Run(hero);
                }
                else if (choice == "2")
                {
                    var hero = SaveService.Load();
                    if (hero == null)
                    {
                        Console.WriteLine("Aucune sauvegarde trouvée.");
                    }
                    else
                    {
                        Console.WriteLine($"Sauvegarde chargée : {hero.Name}");
                        GameService.Run(hero);
                    }
                }
                else if (choice == "3")
                {
                    SaveService.DeleteSave();
                    Console.WriteLine("Sauvegarde supprimée.");
                }
                else if (choice == "4")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Choix invalide.");
                }
            }
        }

        private static Hero CreateNewHero()
        {
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

            return hero;
        }
    }
}