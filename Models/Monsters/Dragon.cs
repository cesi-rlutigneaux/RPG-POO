using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models.Monsters
{
    public sealed class Dragon : Monster
    {
        public Dragon(int level)
            : base(
                name: "Dragon",
                maxHp: 40 + (level * 8),
                attack: 10 + (2 * level),
                defense: 4 + level)
        {
        }

        public override int ComputeDamage()
        {
            // plus gros dégâts que la moyenne
            return Attack + Character.Rng.Next(2, 7); // 2..6
        }

        public override void SpecialAction(Character target)
        {
            // Souffle : très gros dégâts
            int dmg = 18 + (2 * 1) + Character.Rng.Next(0, 5); // niveau implicite = 1 ici (simple)
            target.TakeDamage(dmg);
        }
    }
}
