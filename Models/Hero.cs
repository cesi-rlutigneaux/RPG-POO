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

        public HeroProgression Progression { get; } = new HeroProgression();

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

        public void ApplyLevelUp()
        {
            MaxHp += 5;
            Attack += 1;
            Defense += 1;

            // soin complet après montée de niveau
            Heal(999);
        }

        public virtual void SpecialAction(Character target)
        {
            // Par défaut : rien
        }

        public void RestoreStateFromSave(int maxHp, int hp, int attack, int defense, int progLevel, int progXp)
        {
            if (maxHp < 1) maxHp = 1;

            MaxHp = maxHp;
            Attack = attack < 0 ? 0 : attack;
            Defense = defense < 0 ? 0 : defense;

            RestoreCurrentHp(hp);

            // Progression
            Progression.Restore(progLevel, progXp);
        }

        public override string ToString()
            => base.ToString() + $" | Lvl:{Progression.Level} XP:{Progression.Xp}/{Progression.XpToNext}";
    }
}
