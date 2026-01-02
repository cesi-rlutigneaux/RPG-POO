namespace RPG_POO.Models.Heroes
{
    public sealed class Mage : Hero
    {
        public int Mana { get; private set; }
        public int MaxMana { get; }

        public Mage(string name) : base(name, maxHp: 28, attack: 7, defense: 2)
        {
            MaxMana = 10;
            Mana = MaxMana;
        }

        public override int ComputeDamage()
        {
            // dégâts plus variables
            return Attack + Character.Rng.Next(0, 6); // 0..5
        }

        public override void SpecialAction(Character target)
        {
            // Sort simple : consomme du mana
            if (Mana >= 3)
            {
                Mana -= 3;
                int dmg = 12 + Character.Rng.Next(0, 4); // 12..15
                target.TakeDamage(dmg);
            }
            else
            {
                // pas de mana => attaque normale
                target.TakeDamage(ComputeDamage());
            }
        }

        public override string ToString()
            => base.ToString() + $" MANA:{Mana}/{MaxMana}";
    }
}
