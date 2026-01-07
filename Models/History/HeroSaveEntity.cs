using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models.History
{
    public sealed class HeroSaveEntity
    {
        public int Id { get; set; } = 1; 

        public string HeroName { get; set; } = "";
        public string HeroClass { get; set; } = "";     // "Warrior" / "Mage" / "Rogue"

        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        public int ProgressLevel { get; set; }
        public int ProgressXp { get; set; }

        public int? EquippedWeaponItemId { get; set; }
        public int? EquippedArmorItemId { get; set; }

        public List<ItemSaveEntity> Items { get; set; } = new();
    }
}
