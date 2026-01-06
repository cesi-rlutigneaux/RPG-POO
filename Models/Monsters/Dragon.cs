using RPG_POO.Interfaces;
using RPG_POO.Models.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models.Monsters
{
    public sealed class Dragon : Monster, ILootable
    {
        public IReadOnlyList<Item> DropTable { get; } = new List<Item>
        {
            new Weapon("Dragon Fang", 3),
            new Armor("Dragon Scale", 2),
            new Potion("Greater Potion", 12),
        };

        public Dragon(int level)
            : base(
                name: "Dragon",
                maxHp: 40 + (level * 8),
                attack: 10 + (2 * level),
                defense: 4 + level)
        {
        }

        public override int ComputeDamage()
            => Attack + Character.Rng.Next(2, 7);

        public override void SpecialAction(Character target)
        {
            int dmg = 18 + Character.Rng.Next(0, 5);
            target.TakeDamage(dmg);
        }
    }
}
