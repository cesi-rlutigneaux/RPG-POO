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