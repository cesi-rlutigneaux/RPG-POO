using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models.Items
{
    public sealed class Armor : Item
    {
        public int DefenseBonus { get; }

        public Armor(string name, int defenseBonus) : base(name)
        {
            DefenseBonus = defenseBonus;
        }
    }
}
