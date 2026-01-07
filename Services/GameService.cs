using RPG_POO.Models;
using RPG_POO.Models.Monsters;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Services
{
    public static class GameService
    {
        public static void Run(Hero hero)
        {
            int zoneLevel = 1;
            bool running = true;

            while (running && hero.IsAlive)
            {
                ConsoleUi.Separator();
                Console.WriteLine("MENU");
                Console.WriteLine("1) Explorer");
                Console.WriteLine("2) Afficher héros");
                Console.WriteLine("3) Inventaire");
                Console.WriteLine("4) Quitter");
                Console.Write("> ");

                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        Explore(hero, zoneLevel);
                        if (hero.IsAlive) zoneLevel++;
                        break;

                    case "2":
                        ConsoleUi.Separator();
                        Console.WriteLine(hero);
                        Console.WriteLine($"Arme: {(hero.EquippedWeapon?.Name ?? "(aucune)")}");
                        Console.WriteLine($"Armure: {(hero.EquippedArmor?.Name ?? "(aucune)")}");
                        break;

                    case "3":
                        ShowInventory(hero);
                        break;

                    case "4":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
            }

            ConsoleUi.Separator();
            Console.WriteLine(hero.IsAlive ? "Fin de partie." : "GAME OVER.");
            ConsoleUi.Separator();

            if (hero.IsAlive)
            {
                SaveService.Save(hero);
                Console.WriteLine("Sauvegarde effectuée.");
            }
        }

        private static void Explore(Hero hero, int zoneLevel)
        {
            ConsoleUi.Separator();
            Console.WriteLine($"Exploration de la zone {zoneLevel}...");

            // 70% rencontre
            bool encounter = Character.Rng.Next(0, 100) < 70;
            if (!encounter)
            {
                Console.WriteLine("Aucune rencontre.");
                return;
            }

            Monster monster = CreateRandomMonster(zoneLevel);
            bool won = CombatService.Fight(hero, monster);

            if (!won) return;

            int xpGained = 12 + (zoneLevel * 4);
            Console.WriteLine($"XP gagnée : {xpGained}");

            bool leveledUp = hero.Progression.AddXp(xpGained);
            if (leveledUp)
            {
                Console.WriteLine("NIVEAU SUPERIEUR !");
                hero.ApplyLevelUp();
                Console.WriteLine(hero);
            }

            SaveService.Save(hero);
            Console.WriteLine("Sauvegarde effectuée.");
        }

        private static Monster CreateRandomMonster(int zoneLevel)
        {
            // 80% Goblin / 20% Dragon
            bool dragon = Character.Rng.Next(0, 10) < 2;
            return dragon ? new Dragon(zoneLevel) : new Goblin(zoneLevel);
        }

        private static void ShowInventory(Hero hero)
        {
            ConsoleUi.Separator();
            Console.WriteLine("INVENTAIRE");

            if (hero.Inventory.Items.Count == 0)
            {
                Console.WriteLine("- (vide)");
                return;
            }

            for (int i = 0; i < hero.Inventory.Items.Count; i++)
            {
                Console.WriteLine($"- {hero.Inventory.Items[i]}");
            }
        }
    }
}
