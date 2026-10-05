namespace RPG_POO.Api;

public sealed record NewGameRequest(string Name, string HeroClass);
public sealed record CombatActionRequest(string Action);

public sealed record GameSnapshot(
    bool HasSave,
    int Zone,
    string Phase,
    HeroDto? Hero,
    MonsterDto? Monster,
    IReadOnlyList<string> Log);

public sealed record HeroDto(
    string Name,
    string Class,
    int Hp,
    int MaxHp,
    int Attack,
    int Defense,
    int Level,
    int Xp,
    int XpToNext,
    int? Mana,
    int? MaxMana,
    string? Weapon,
    string? Armor,
    IReadOnlyList<ItemDto> Inventory);

public sealed record MonsterDto(string Name, int Hp, int MaxHp, int Attack, int Defense);
public sealed record ItemDto(string Name, string Type, int Value);
