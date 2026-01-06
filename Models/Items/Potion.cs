using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models.Items
{
    public sealed class Potion : Item
    {
        public int HealAmount { get; }

        public Potion(string name, int healAmount) : base(name)
        {
            HealAmount = healAmount;
        }
    }
}
