namespace RPG_POO.Models.Heroes
{
    public sealed class Rogue : Hero
    {
        public Rogue(string name) : base(name, maxHp: 32, attack: 8, defense: 3) { }

        public override int ComputeDamage()
        {
            // Critique 25%
            bool crit = Character.Rng.Next(0, 4) == 0;
            int baseDmg = Attack + Character.Rng.Next(0, 3); // 0..2
            return crit ? baseDmg * 2 : baseDmg;
        }

        public override void SpecialAction(Character target)
        {
            // Double frappe
            target.TakeDamage(ComputeDamage());
            target.TakeDamage(ComputeDamage());
        }
    }
}
