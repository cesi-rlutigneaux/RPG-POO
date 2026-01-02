using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models
{
    public abstract class Character
    {
        public string Name { get; }
        public int Hp { get; private set; }

        public int Attack { get; protected set; }
        public int Defense { get; protected set; }

        protected Character(string name, int hp, int attack, int defense)
        {
            Name = name;
            Hp = hp;
            Attack = attack;
            Defense = defense;
        }

        public void TakeDamage(int damage)
        {
            int realDamage = damage - Defense;
            if (realDamage < 0) realDamage = 0;

            Hp -= realDamage;
            if (Hp < 0) Hp = 0;
        }

        public override string ToString()
            => $"{Name} | HP:{Hp} ATK:{Attack} DEF:{Defense}";
    }
}
