using RPG_POO.Interfaces;
using RPG_POO.Models.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models.Monsters
{
    public sealed class Goblin : Monster, ILootable
    {
        public IReadOnlyList<Item> DropTable { get; } = new List<Item>
        {
            new Weapon("Rusty Dagger", 1),
            new Potion("Small Potion", 6),
        };

        public Goblin(int level)
            : base(
                name: "Goblin",
                maxHp: 18 + (level * 4),
                attack: 6 + level,
                defense: 1 + (level / 2))
        {
        }

        public override void SpecialAction(Character target)
        {
            int dmg = ComputeDamage() + 2;
            target.TakeDamage(dmg);
        }
    }
}
