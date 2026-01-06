using RPG_POO.Interfaces;
using RPG_POO.Models;
using RPG_POO.Models.Items;

namespace RPG_POO.Services
{
    public static class LootService
    {
        public static Item? TryDrop(Character monster)
        {
            if (monster is not ILootable lootable) return null;
            if (lootable.DropTable.Count == 0) return null;

            // 60% chance de loot
            if (Character.Rng.Next(0, 100) >= 60) return null;

            int idx = Character.Rng.Next(0, lootable.DropTable.Count);
            return lootable.DropTable[idx];
        }

        public static void GiveToHero(Hero hero, Item item)
        {
            hero.Inventory.Add(item);

            if (item is Models.Items.Weapon w) hero.Equip(w);
            if (item is Armor a) hero.Equip(a);
        }
    }
}
