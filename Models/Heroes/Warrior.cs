namespace RPG_POO.Models.Heroes
{
    public sealed class Warrior : Hero
    {
        public Warrior(string name) : base(name, maxHp: 40, attack: 9, defense: 4) { }

        public override void SpecialAction(Character target)
        {
            // Coup puissant : +4 dégâts bruts
            int dmg = ComputeDamage() + 4;
            target.TakeDamage(dmg);
        }
    }
}
