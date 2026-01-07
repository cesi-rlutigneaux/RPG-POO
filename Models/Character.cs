using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models
{
    public abstract class Character
    {
        public string Name { get; }

        public int MaxHp { get; protected set; }
        public int Hp { get; private set; }

        public int Attack { get; protected set; }
        public int Defense { get; protected set; }

        public static readonly Random Rng = new Random();

        protected Character(string name, int maxHp, int attack, int defense)
        {
            Name = string.IsNullOrWhiteSpace(name) ? "Unknown" : name.Trim();

            MaxHp = maxHp;
            Hp = maxHp;

            Attack = attack;
            Defense = defense;
        }

        public bool IsAlive => Hp > 0;

        public void TakeDamage(int damage)
        {
            int realDamage = damage - Defense;
            if (realDamage < 0) realDamage = 0;

            Hp -= realDamage;
            if (Hp < 0) Hp = 0;
        }

        public void Heal(int amount)
        {
            if (amount < 0) amount = 0;

            Hp += amount;
            if (Hp > MaxHp) Hp = MaxHp;
        }

        public virtual int ComputeDamage()
        {
            // petit aléa 0 à 2 pour éviter toujours les mêmes dégâts
            return Attack + Rng.Next(0, 3);
        }

        protected void RestoreCurrentHp(int hp)
        {
            if (hp < 0) hp = 0;
            if (hp > MaxHp) hp = MaxHp;

            Hp = hp;
        }

        public override string ToString()
            => $"{Name} | HP:{Hp}/{MaxHp} ATK:{Attack} DEF:{Defense}";
    }
}
