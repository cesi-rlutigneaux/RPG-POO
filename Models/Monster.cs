using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models
{
    public class Monster : Character
    {
        public Monster(string name, int level)
            : base(
                name,
                maxHp: 18 + (level * 4),
                attack: 6 + level,
                defense: 1 + (level / 2))
        {
        }
    }
}
