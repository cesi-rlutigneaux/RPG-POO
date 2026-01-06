using RPG_POO.Models;
using RPG_POO.Models.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Services
{
    public static class CombatService
    {
        public static void Fight(Hero hero, Monster monster)
        {
            ConsoleUi.Separator();
            Console.WriteLine("COMBAT !");
            Console.WriteLine(hero);
            Console.WriteLine(monster);
            ConsoleUi.Separator();

            int turn = 1;

            while (hero.IsAlive && monster.IsAlive)
            {
                Console.WriteLine($"Tour {turn}");

                // Tour héros
                Console.WriteLine("(A)ttaque  (S)pécial  (P)otion");
                Console.Write("> ");
                string? choice = Console.ReadLine()?.Trim().ToUpperInvariant();

                if (choice == "P")
                {
                    bool used = hero.UsePotion();
                    Console.WriteLine(used ? "Potion utilisée." : "Aucune potion disponible.");
                }
                else if (choice == "S")
                {
                    hero.SpecialAction(monster);
                    Console.WriteLine($"{hero.Name} utilise une action spéciale !");
                }
                else
                {
                    int dmg = hero.ComputeDamage();
                    monster.TakeDamage(dmg);
                    Console.WriteLine($"{hero.Name} attaque pour {dmg} dégâts bruts.");
                }

                Console.WriteLine(monster);
                if (!monster.IsAlive) break;

                // Tour monstre (20% spécial)
                bool doSpecial = Character.Rng.Next(0, 5) == 0;

                if (doSpecial)
                {
                    monster.SpecialAction(hero);
                    Console.WriteLine($"{monster.Name} utilise une action spéciale !");
                }
                else
                {
                    int dmg = monster.ComputeDamage();
                    hero.TakeDamage(dmg);
                    Console.WriteLine($"{monster.Name} attaque pour {dmg} dégâts bruts.");
                }

                Console.WriteLine(hero);
                ConsoleUi.Separator();
                turn++;
            }

            bool heroWon = hero.IsAlive;
            Console.WriteLine(heroWon ? "Victoire du héros !" : "Défaite du héros...");
            ConsoleUi.Separator();

            if (heroWon)
            {
                Item? drop = LootService.TryDrop(monster);
                if (drop != null)
                {
                    LootService.GiveToHero(hero, drop);
                    Console.WriteLine($"Butin obtenu : {drop}");
                }
                else
                {
                    Console.WriteLine("Aucun butin cette fois.");
                }

                Console.WriteLine($"Inventaire : {hero.Inventory.Items.Count} objet(s).");
                ConsoleUi.Separator();
            }
        }
    }
}
