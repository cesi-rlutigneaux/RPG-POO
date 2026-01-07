using Microsoft.EntityFrameworkCore;
using RPG_POO.Data;
using RPG_POO.Models;
using RPG_POO.Models.Heroes;
using RPG_POO.Models.History;
using RPG_POO.Models.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Services
{
    public static class SaveService
    {
        public static void EnsureDbCreated()
        {
            using var db = new SaveDbContext();
            db.Database.Migrate(); // crée la DB + applique migrations
        }

        public static bool HasSave()
        {
            using var db = new SaveDbContext();
            return db.HeroSaves.Any(x => x.Id == 1);
        }

        public static void Save(Hero hero)
        {
            using var db = new SaveDbContext();

            var existing = db.HeroSaves
                .Include(x => x.Items)
                .FirstOrDefault(x => x.Id == 1);

            if (existing == null)
            {
                existing = new HeroSaveEntity { Id = 1 };
                db.HeroSaves.Add(existing);
            }

            existing.HeroName = hero.Name;
            existing.HeroClass = hero.GetType().Name;

            existing.MaxHp = hero.MaxHp;
            existing.Hp = hero.Hp;
            existing.Attack = hero.Attack;
            existing.Defense = hero.Defense;

            existing.ProgressLevel = hero.Progression.Level;
            existing.ProgressXp = hero.Progression.Xp;

            // reset items
            existing.Items.Clear();
            foreach (var item in hero.Inventory.Items)
            {
                existing.Items.Add(ToItemEntity(item));
            }

            // équipement : repérage par index dans Items (simple) => on stocke l'Id entity après SaveChanges
            // pour simplifier : on ne sauvegarde pas l’équipement ici par Id,
            // on le restaurera en auto-equip (arme/armure) au chargement.
            existing.EquippedWeaponItemId = null;
            existing.EquippedArmorItemId = null;

            db.SaveChanges();
        }

        public static Hero? Load()
        {
            using var db = new SaveDbContext();

            var save = db.HeroSaves
                .Include(x => x.Items)
                .FirstOrDefault(x => x.Id == 1);

            if (save == null) return null;

            Hero hero = CreateHero(save.HeroClass, save.HeroName);

            hero.RestoreStateFromSave(
                maxHp: save.MaxHp,
                hp: save.Hp,
                attack: save.Attack,
                defense: save.Defense,
                progLevel: save.ProgressLevel,
                progXp: save.ProgressXp
            );

            // restaure inventaire
            foreach (var it in save.Items)
            {
                hero.Inventory.Add(FromItemEntity(it));
            }

            // restauration équipement : auto-equip la meilleure arme + meilleure armure trouvées
            var bestWeapon = hero.Inventory.Items.OfType<Weapon>()
                .OrderByDescending(w => w.AttackBonus)
                .FirstOrDefault();

            var bestArmor = hero.Inventory.Items.OfType<Armor>()
                .OrderByDescending(a => a.DefenseBonus)
                .FirstOrDefault();

            if (bestWeapon != null) hero.Equip(bestWeapon);
            if (bestArmor != null) hero.Equip(bestArmor);

            return hero;
        }

        public static void DeleteSave()
        {
            using var db = new SaveDbContext();
            var save = db.HeroSaves.Include(x => x.Items).FirstOrDefault(x => x.Id == 1);
            if (save == null) return;

            db.HeroSaves.Remove(save);
            db.SaveChanges();
        }

        private static Hero CreateHero(string heroClass, string name)
        {
            return heroClass switch
            {
                nameof(Mage) => new Mage(name),
                nameof(Rogue) => new Rogue(name),
                _ => new Warrior(name)
            };
        }

        private static ItemSaveEntity ToItemEntity(Item item)
        {
            return item switch
            {
                Weapon w => new ItemSaveEntity { ItemType = "Weapon", Name = w.Name, Value = w.AttackBonus },
                Armor a => new ItemSaveEntity { ItemType = "Armor", Name = a.Name, Value = a.DefenseBonus },
                Potion p => new ItemSaveEntity { ItemType = "Potion", Name = p.Name, Value = p.HealAmount },
                _ => new ItemSaveEntity { ItemType = "Unknown", Name = item.Name, Value = 0 }
            };
        }

        private static Item FromItemEntity(ItemSaveEntity entity)
        {
            return entity.ItemType switch
            {
                "Weapon" => new Weapon(entity.Name, entity.Value),
                "Armor" => new Armor(entity.Name, entity.Value),
                "Potion" => new Potion(entity.Name, entity.Value),
                _ => new Potion(entity.Name, 0) // fallback simple
            };
        }
    }
}
