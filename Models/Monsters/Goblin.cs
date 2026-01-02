using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models.Monsters
{
    public sealed class Goblin : Monster
    {
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
            // Coup sale : +2 dégâts bruts
            int dmg = ComputeDamage() + 2;
            target.TakeDamage(dmg);
        }
    }
}
