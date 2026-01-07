using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Models
{
    public sealed class HeroProgression
    {
        public int Level { get; private set; } = 1;
        public int Xp { get; private set; }

        public int XpToNext => 20 + (Level * 10);

        public bool AddXp(int amount)
        {
            if (amount < 0) amount = 0;
            Xp += amount;

            if (Xp >= XpToNext)
            {
                Xp -= XpToNext;
                Level++;
                return true;
            }

            return false;
        }

        public void Restore(int level, int xp)
        {
            if (level < 1) level = 1;
            if (xp < 0) xp = 0;

            Level = level;
            Xp = xp;
        }
    }
}
