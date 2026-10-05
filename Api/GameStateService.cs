using RPG_POO.Models;
using RPG_POO.Models.Heroes;
using RPG_POO.Models.Items;
using RPG_POO.Models.Monsters;
using RPG_POO.Services;

namespace RPG_POO.Api;

public sealed class GameStateService
{
    private readonly object _gate = new();
    private Hero? _hero;
    private Monster? _monster;
    private int _zone = 1;
    private readonly List<string> _log = [];

    public GameSnapshot GetSnapshot()
    {
        lock (_gate) return Snapshot();
    }

    public GameSnapshot NewGame(NewGameRequest request)
    {
        lock (_gate)
        {
            _hero = request.HeroClass.Trim().ToLowerInvariant() switch
            {
                "mage" => new Mage(request.Name),
                "rogue" => new Rogue(request.Name),
                _ => new Warrior(request.Name)
            };

            var weapon = new Weapon("Épée d'entraînement", 1);
            var armor = new Armor("Gilet de cuir", 1);
            _hero.Inventory.Add(weapon);
            _hero.Inventory.Add(armor);
            _hero.Inventory.Add(new Potion("Petite potion", 6));
            _hero.Inventory.Add(new Potion("Petite potion", 6));
            _hero.Equip(weapon);
            _hero.Equip(armor);

            _zone = 1;
            _monster = null;
            _log.Clear();
            _log.Add($"{_hero.Name} entre dans les Terres oubliées.");
            SaveService.Save(_hero);
            return Snapshot();
        }
    }

    public GameSnapshot? Load()
    {
        lock (_gate)
        {
            _hero = SaveService.Load();
            if (_hero is null) return null;

            _zone = Math.Max(1, _hero.Progression.Level);
            _monster = null;
            _log.Clear();
            _log.Add($"La destinée de {_hero.Name} reprend son cours.");
            return Snapshot();
        }
    }

    public GameSnapshot Explore()
    {
        lock (_gate)
        {
            EnsureHero();
            if (!_hero!.IsAlive) throw new InvalidOperationException("Le héros ne peut plus explorer.");
            if (_monster?.IsAlive == true) throw new InvalidOperationException("Un combat est déjà en cours.");

            _log.Clear();
            _log.Add($"Exploration de la zone {_zone}…");
            if (Character.Rng.Next(0, 100) >= 70)
            {
                _monster = null;
                _log.Add("Le vent souffle entre les ruines. Aucun ennemi en vue.");
                _zone++;
                return Snapshot();
            }

            _monster = Character.Rng.Next(0, 10) < 2 ? new Dragon(_zone) : new Goblin(_zone);
            _log.Add($"Un {_monster.Name} surgit de l'ombre !");
            return Snapshot();
        }
    }

    public GameSnapshot PlayTurn(string action)
    {
        lock (_gate)
        {
            EnsureHero();
            if (_monster?.IsAlive != true) throw new InvalidOperationException("Aucun combat n'est en cours.");

            _log.Clear();
            switch (action.Trim().ToLowerInvariant())
            {
                case "potion":
                    var hpBefore = _hero!.Hp;
                    _log.Add(_hero.UsePotion()
                        ? $"{_hero.Name} récupère {_hero.Hp - hpBefore} PV."
                        : "Aucune potion disponible.");
                    break;
                case "special":
                    var monsterHpBefore = _monster.Hp;
                    _hero!.SpecialAction(_monster);
                    _log.Add($"{_hero.Name} lance sa technique spéciale : {monsterHpBefore - _monster.Hp} dégâts.");
                    break;
                default:
                    var targetHpBefore = _monster.Hp;
                    _monster.TakeDamage(_hero!.ComputeDamage());
                    _log.Add($"{_hero.Name} attaque : {targetHpBefore - _monster.Hp} dégâts.");
                    break;
            }

            if (!_monster.IsAlive)
            {
                ResolveVictory();
                return Snapshot();
            }

            var heroHpBefore = _hero.Hp;
            if (Character.Rng.Next(0, 5) == 0)
            {
                _monster.SpecialAction(_hero);
                _log.Add($"{_monster.Name} utilise une attaque spéciale : {heroHpBefore - _hero.Hp} dégâts.");
            }
            else
            {
                _hero.TakeDamage(_monster.ComputeDamage());
                _log.Add($"{_monster.Name} riposte : {heroHpBefore - _hero.Hp} dégâts.");
            }

            if (!_hero.IsAlive)
                _log.Add($"{_hero.Name} tombe au combat. La partie est terminée.");

            return Snapshot();
        }
    }

    public GameSnapshot Save()
    {
        lock (_gate)
        {
            EnsureHero();
            SaveService.Save(_hero!);
            _log.Clear();
            _log.Add("Votre progression a été sauvegardée.");
            return Snapshot();
        }
    }

    public void DeleteSave()
    {
        lock (_gate)
        {
            SaveService.DeleteSave();
            _hero = null;
            _monster = null;
            _zone = 1;
            _log.Clear();
        }
    }

    private void ResolveVictory()
    {
        _log.Add($"Victoire sur {_monster!.Name} !");
        var drop = LootService.TryDrop(_monster);
        if (drop is not null)
        {
            LootService.GiveToHero(_hero!, drop);
            _log.Add($"Butin obtenu : {drop.Name}.");
        }
        else
        {
            _log.Add("Aucun butin cette fois.");
        }

        var xp = 12 + (_zone * 4);
        if (_hero!.Progression.AddXp(xp))
        {
            _hero.ApplyLevelUp();
            _log.Add($"Niveau {_hero.Progression.Level} atteint ! Vos forces sont restaurées.");
        }
        else
        {
            _log.Add($"+{xp} points d'expérience.");
        }

        _zone++;
        SaveService.Save(_hero);
    }

    private GameSnapshot Snapshot()
    {
        var phase = _hero is null ? "welcome"
            : !_hero.IsAlive ? "gameover"
            : _monster?.IsAlive == true ? "combat"
            : "exploration";

        return new GameSnapshot(
            SaveService.HasSave(),
            _zone,
            phase,
            _hero is null ? null : ToDto(_hero),
            _monster is null ? null : new MonsterDto(
                _monster.Name, _monster.Hp, _monster.MaxHp, _monster.Attack, _monster.Defense),
            _log.ToArray());
    }

    private static HeroDto ToDto(Hero hero)
    {
        var mage = hero as Mage;
        return new HeroDto(
            hero.Name,
            hero.GetType().Name,
            hero.Hp,
            hero.MaxHp,
            hero.Attack,
            hero.Defense,
            hero.Progression.Level,
            hero.Progression.Xp,
            hero.Progression.XpToNext,
            mage?.Mana,
            mage?.MaxMana,
            hero.EquippedWeapon?.Name,
            hero.EquippedArmor?.Name,
            hero.Inventory.Items.Select(item => item switch
            {
                Weapon weapon => new ItemDto(item.Name, "Arme", weapon.AttackBonus),
                Armor armor => new ItemDto(item.Name, "Armure", armor.DefenseBonus),
                Potion potion => new ItemDto(item.Name, "Potion", potion.HealAmount),
                _ => new ItemDto(item.Name, "Objet", 0)
            }).ToArray());
    }

    private void EnsureHero()
    {
        if (_hero is null) throw new InvalidOperationException("Commencez ou chargez une partie.");
    }
}
