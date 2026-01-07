# TP0 — Models (lite)

## But
Créer une base très simple pour démarrer le RPG :
- une classe abstraite `Character`
- une classe concrète `Monster` qui hérite de `Character`

## À vérifier
- `Hp` ne peut pas être modifié directement (`private set`)
- `TakeDamage` applique la défense et évite les HP négatifs

## Exécution
Lancer le projet : un monstre est créé et subit des dégâts.


# TP1 — Fondations POO (RPG Console)

## Objectif
Faire évoluer la base du projet pour introduire :
- `Character` (classe abstraite) améliorée : `MaxHp`, `Hp`, `Heal`, `IsAlive`, `ComputeDamage`
- `Hero` (classe concrète)
- `Monster` (classe concrète, stats dépendantes du niveau)
- Un test simple en console (pas encore de boucle de combat)

## Règles
- `Hp` ne doit jamais être négatif.
- `Hp` ne peut pas être modifié directement depuis l’extérieur (`private set`).
- La réduction des dégâts se fait par la défense : `damage - Defense`, minimum 0.
- `Heal` ne dépasse jamais `MaxHp`.

## Travail demandé
1) Créer la classe `Hero` qui hérite de `Character`.
2) Modifier `Monster` pour qu’il prenne un `level` et adapte ses statistiques.
3) Modifier `Character` pour ajouter :
   - `MaxHp`
   - `Heal(int amount)`
   - `IsAlive`
   - `ComputeDamage()` (attaque + petit aléa)
4) Dans `Program.cs`, créer un héros et un monstre, afficher leurs stats, simuler 2 attaques.

## Extension (facultatif)
- Ajouter une méthode `PrintStatus()` dans `Character` (ou utiliser `ToString()`).
- Empêcher un `maxHp` trop bas (ex: forcer min 1).


# TP2 — Combat + Polymorphisme

## Objectif
Faire un vrai combat tour par tour en utilisant :
- `virtual/override` (polymorphisme)
- des classes spécialisées (Warrior/Mage/Rogue, Goblin/Dragon)
- un `CombatService` pour ne pas tout mettre dans `Program.cs`

## Travail demandé
1) Transformer `Hero` et `Monster` en classes abstraites.
2) Créer 3 héros :
   - `Warrior` : action spéciale = coup puissant (+4 dégâts bruts)
   - `Mage` : mana + sort si mana
   - `Rogue` : critique 25% + action spéciale double frappe
3) Créer 2 monstres :
   - `Goblin` : action spéciale (+2 dégâts bruts)
   - `Dragon` : dégâts plus élevés + souffle
4) Implémenter `CombatService.Fight(hero, monster)` :
   - boucle tant que les 2 sont vivants
   - choix héros (attaque ou spécial)
   - monstre : 20% chance d’utiliser spécial
5) Dans `Program.cs` :
   - demander le nom
   - demander la classe du héros
   - demander le monstre
   - lancer le combat

## Contraintes
- Pas de `if (hero is Mage) ...` pour gérer les comportements :
  on veut du polymorphisme (`override`).
- Le combat doit être jouable et afficher des logs simples.

## Extension (facultatif)
- Afficher un récapitulatif du tour
- Ajouter une option "se soigner" (sera fait au TP3 avec les potions)


# TP3 — Interfaces + Composition (Inventaire / Items / Loot)

## Objectif
Ajouter des fonctionnalités “jeu” tout en restant POO :
- Composition : `Inventory`, `Item`, `Weapon`, `Armor`, `Potion`
- Interfaces : `ILootable` (monstres qui peuvent drop)
- Équipement : arme/armure qui modifient les stats
- Potions : soin consommable pendant un combat
- Loot : obtenir un objet après victoire

## Travail demandé
1) Créer un dossier `Models/Items` et les classes :
   - `Item` (abstraite, `Name`)
   - `Weapon` (`AttackBonus`)
   - `Armor` (`DefenseBonus`)
   - `Potion` (`HealAmount`)
2) Créer `Inventory` (liste d’items, Add/Remove, Items en lecture seule).
3) Modifier `Hero` :
   - ajoute un `Inventory`
   - ajoute `Equip(Weapon)` et `Equip(Armor)`
   - ajoute `UsePotion()` (consomme la première potion trouvée)
4) Créer l’interface `ILootable` :
   - `IReadOnlyList<Item> DropTable`
5) Modifier `Goblin` et `Dragon` pour implémenter `ILootable` et définir une `DropTable`.
6) Créer `LootService` :
   - `TryDrop(monster)` : 60% de chance, retourne un item aléatoire de la DropTable
   - `GiveToHero(hero, item)` : ajoute à l’inventaire et auto-équipe armes/armures
7) Modifier `CombatService` :
   - ajoute l’option (P)otion
   - si victoire, essayer de loot et afficher le résultat

## Contraintes
- `Inventory.Items` doit être en lecture seule (IReadOnlyList) côté extérieur.
- `Hp` reste encapsulé : on soigne via `Heal` et on consomme potion via `UsePotion`.
- Ne pas mettre toute la logique dans `Program.cs`.

## Extensions (facultatif)
- Afficher le contenu de l’inventaire à la fin
- Ajouter une limite d’inventaire (ex: 10 items)
- Ajouter une potion “Greater” plus rare


# TP4 — Mini RPG jouable (menu + exploration + XP)

## Objectif
Avoir un jeu console jouable avec :
- un menu (explorer / afficher héros / inventaire / quitter)
- exploration avec rencontres aléatoires
- enchaînement de combats
- gain d’XP + montée de niveau

## Travail demandé
1) Créer `HeroProgression` :
   - `Level`, `Xp`, `XpToNext`
   - `AddXp(amount)` renvoie `true` si le héros monte de niveau
2) Modifier `Hero` :
   - ajouter la propriété `Progression`
   - ajouter `ApplyLevelUp()` (augmente stats et soigne)
   - afficher niveau et XP dans `ToString()`
3) Créer `GameService` :
   - `Run(hero)` : boucle de menu tant que le héros est vivant
   - `Explore(hero, zoneLevel)` :
     - 70% rencontre
     - crée un monstre aléatoire (Goblin/Dragon) avec `zoneLevel`
     - lance `CombatService.Fight`
     - si victoire : donner de l’XP et gérer le level up
4) Modifier `CombatService` pour renvoyer un `bool` :
   - `true` si le héros gagne, `false` sinon
5) Modifier `Program.cs` :
   - créer le héros + kit de départ
   - lancer `GameService.Run(hero)`

## Contraintes
- Le menu doit rester lisible.
- `Program.cs` ne doit pas contenir toute la logique.
- Le projet doit être compilable et jouable du début à la fin.

## Extensions (facultatif)
- Ajouter une option “se reposer” pour récupérer un peu de HP
- Ajouter un affichage plus clair des tours
- Ajouter une rareté sur les loots (plus rare pour les meilleurs objets)


# TP5 — Sauvegarde / Chargement (EF Core + SQLite)

## Objectif
Sauvegarder en base de données :
- le héros (nom, classe, stats)
- la progression (niveau + XP)
- l’inventaire (armes, armures, potions)
Et permettre de reprendre la partie depuis la dernière sauvegarde.

## Outils
- EF Core
- SQLite (fichier `rpg_save.db`)

## Installation (si besoin)
### Ajouter les packages