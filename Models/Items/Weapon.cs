using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models.Items
{
    public sealed class Weapon : Item
    {
        public int AttackBonus { get; }

        public Weapon(string name, int attackBonus) : base(name)
        {
            AttackBonus = attackBonus;
        }
    }
}
