using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models
{
    public abstract class Monster : Character
    {
        protected Monster(string name, int maxHp, int attack, int defense)
            : base(name, maxHp, attack, defense)
        {
        }

        public virtual void SpecialAction(Character target)
        {
            // Par défaut : rien de spécial
        }
    }
}
