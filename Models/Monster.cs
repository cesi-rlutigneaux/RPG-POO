using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models
{
    public class Monster : Character
    {
        public Monster(string name)
            : base(name, hp: 20, attack: 6, defense: 1)
        {
        }
    }
}
