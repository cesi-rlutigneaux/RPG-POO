using RPG_POO.Models.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models
{
    public abstract class Hero : Character
    {
        public Inventory Inventory { get; } = new Inventory();

        public Weapon? EquippedWeapon { get; private set; }
        public Armor? EquippedArmor { get; private set; }

        protected Hero(string name, int maxHp, int attack, int defense)
            : base(name, maxHp, attack, defense)
        {
        }

        public void Equip(Weapon weapon)
        {
            if (EquippedWeapon != null)
                Attack -= EquippedWeapon.AttackBonus;

            EquippedWeapon = weapon;
            Attack += weapon.AttackBonus;
        }

        public void Equip(Armor armor)
        {
            if (EquippedArmor != null)
                Defense -= EquippedArmor.DefenseBonus;

            EquippedArmor = armor;
            Defense += armor.DefenseBonus;
        }

        public bool UsePotion()
        {
            foreach (var item in Inventory.Items)
            {
                if (item is Potion p)
                {
                    Heal(p.HealAmount);
                    Inventory.Remove(p);
                    return true;
                }
            }
            return false;
        }

        public virtual void SpecialAction(Character target)
        {
            // Par défaut : rien
        }
    }
}
