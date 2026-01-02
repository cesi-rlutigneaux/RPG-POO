using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models
{
    public class Hero : Character
    {
        public Hero(string name)
            : base(name, maxHp: 30, attack: 8, defense: 2)
        {
        }
    }
}
