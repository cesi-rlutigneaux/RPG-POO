using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models.History
{
    public sealed class ItemSaveEntity
    {
        public int Id { get; set; }

        public int HeroSaveEntityId { get; set; }
        public HeroSaveEntity? HeroSave { get; set; }

        public string ItemType { get; set; } = ""; // "Weapon" / "Armor" / "Potion"
        public string Name { get; set; } = "";

        public int Value { get; set; }             // AttackBonus / DefenseBonus / HealAmount selon type
    }
}
