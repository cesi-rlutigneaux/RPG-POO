import React, { useEffect, useState } from "react";
import { createRoot } from "react-dom/client";

const h = React.createElement;

async function api(path, options = {}) {
  const response = await fetch(`/api/game${path}`, {
    headers: { "Content-Type": "application/json" },
    ...options
  });
  if (!response.ok) {
    const error = await response.json().catch(() => ({}));
    throw new Error(error.message || "Une erreur est survenue.");
  }
  return response.status === 204 ? null : response.json();
}

function Icon({ children }) {
  return h("span", { className: "icon", "aria-hidden": true }, children);
}

function Bar({ label, value, max, tone = "health" }) {
  const ratio = Math.max(0, Math.min(100, (value / max) * 100));
  return h("div", { className: "bar-group" },
    h("div", { className: "bar-label" },
      h("span", null, label),
      h("strong", null, `${value} / ${max}`)
    ),
    h("div", { className: `bar ${tone}` },
      h("span", { style: { width: `${ratio}%` } })
    )
  );
}

function Welcome({ hasSave, onStart, onLoad, busy }) {
  const [name, setName] = useState("");
  const [heroClass, setHeroClass] = useState("Warrior");
  const classes = [
    ["Warrior", "⚔", "Guerrier", "Endurant et redoutable au corps-à-corps"],
    ["Mage", "✦", "Mage", "Puissant, imprévisible et maître des arcanes"],
    ["Rogue", "◈", "Voleur", "Rapide, rusé et capable de coups critiques"]
  ];

  return h("main", { className: "welcome" },
    h("p", { className: "eyebrow" }, "Un récit interactif"),
    h("h1", null, "Les Chroniques", h("span", null, "d’Obsidienne")),
    h("p", { className: "intro" }, "Les brumes se lèvent sur les Terres oubliées. Forgez votre héros, affrontez les créatures de l’ombre et écrivez votre légende."),
    h("section", { className: "creation-card" },
      h("div", { className: "ornament" }, "◆"),
      h("label", { htmlFor: "hero-name" }, "Nom de votre héros"),
      h("input", {
        id: "hero-name",
        value: name,
        onChange: event => setName(event.target.value),
        onKeyDown: event => event.key === "Enter" && name.trim() && onStart(name.trim(), heroClass),
        placeholder: "Entrez un nom…",
        maxLength: 24
      }),
      h("p", { className: "field-label" }, "Choisissez votre voie"),
      h("div", { className: "class-grid" },
        classes.map(([value, glyph, title, copy]) =>
          h("button", {
            key: value,
            type: "button",
            className: `class-card ${heroClass === value ? "selected" : ""}`,
            onClick: () => setHeroClass(value)
          },
            h(Icon, null, glyph),
            h("strong", null, title),
            h("small", null, copy)
          )
        )
      ),
      h("button", {
        className: "primary wide",
        disabled: busy || !name.trim(),
        onClick: () => onStart(name.trim(), heroClass)
      }, busy ? "Ouverture du grimoire…" : "Commencer l’aventure", h("span", null, "→")),
      hasSave && h("button", { className: "text-button", disabled: busy, onClick: onLoad }, "Reprendre la dernière partie")
    )
  );
}

function HeroPanel({ hero }) {
  return h("aside", { className: "hero-panel panel" },
    h("div", { className: "portrait" }, h("span", null, hero.class === "Mage" ? "✦" : hero.class === "Rogue" ? "◈" : "⚔")),
    h("div", { className: "hero-heading" },
      h("p", { className: "eyebrow" }, hero.class),
      h("h2", null, hero.name),
      h("span", { className: "level" }, `Niveau ${hero.level}`)
    ),
    h(Bar, { label: "Points de vie", value: hero.hp, max: hero.maxHp }),
    hero.maxMana && h(Bar, { label: "Mana", value: hero.mana, max: hero.maxMana, tone: "mana" }),
    h(Bar, { label: "Expérience", value: hero.xp, max: hero.xpToNext, tone: "xp" }),
    h("div", { className: "stat-grid" },
      h("div", null, h("span", null, "ATQ"), h("strong", null, hero.attack)),
      h("div", null, h("span", null, "DEF"), h("strong", null, hero.defense))
    ),
    h("div", { className: "equipment" },
      h("p", null, h(Icon, null, "⚔"), h("span", null, "Arme", h("strong", null, hero.weapon || "Aucune"))),
      h("p", null, h(Icon, null, "⬡"), h("span", null, "Armure", h("strong", null, hero.armor || "Aucune")))
    )
  );
}

function Inventory({ items }) {
  return h("section", { className: "panel inventory" },
    h("div", { className: "section-title" },
      h("h3", null, "Inventaire"),
      h("span", null, `${items.length} objets`)
    ),
    items.length === 0
      ? h("p", { className: "muted" }, "Votre sac est vide.")
      : h("div", { className: "item-list" },
          items.map((item, index) =>
            h("div", { className: "item", key: `${item.name}-${index}` },
              h(Icon, null, item.type === "Potion" ? "✚" : item.type === "Arme" ? "⚔" : "⬡"),
              h("span", null, h("strong", null, item.name), h("small", null, item.type)),
              h("b", null, `+${item.value}`)
            )
          )
        )
  );
}

function Adventure({ game, onAction, onExplore, onSave, onReset, busy }) {
  const { hero, monster, phase, zone, log } = game;
  return h("div", { className: "game-shell" },
    h("header", null,
      h("div", { className: "brand" }, h("span", null, "◆"), "Chroniques d’Obsidienne"),
      h("div", { className: "header-actions" },
        h("button", { className: "ghost", onClick: onSave, disabled: busy }, "Sauvegarder"),
        h("button", { className: "ghost danger", onClick: onReset, disabled: busy }, "Quitter")
      )
    ),
    h("div", { className: "game-grid" },
      h("div", { className: "left-column" },
        h(HeroPanel, { hero }),
        h(Inventory, { items: hero.inventory })
      ),
      h("main", { className: "scene" },
        h("div", { className: "zone-label" }, h("span", null), `Zone ${zone}`, h("span", null)),
        phase === "combat"
          ? h("section", { className: "encounter" },
              h("p", { className: "eyebrow danger-text" }, "Combat en cours"),
              h("div", { className: `monster-sigil ${monster.name.toLowerCase()}` }, monster.name === "Dragon" ? "♜" : "♟"),
              h("h1", null, monster.name),
              h("p", { className: "monster-stats" }, `ATQ ${monster.attack} · DEF ${monster.defense}`),
              h(Bar, { label: "Vitalité de l’ennemi", value: monster.hp, max: monster.maxHp }),
              h("div", { className: "actions" },
                h("button", { className: "primary", disabled: busy, onClick: () => onAction("attack") }, h(Icon, null, "⚔"), h("span", null, "Attaquer", h("small", null, "Frappe classique"))),
                h("button", { className: "secondary", disabled: busy, onClick: () => onAction("special") }, h(Icon, null, "✦"), h("span", null, "Technique", h("small", null, "Action spéciale"))),
                h("button", { className: "secondary", disabled: busy, onClick: () => onAction("potion") }, h(Icon, null, "✚"), h("span", null, "Potion", h("small", null, "Récupérer des PV")))
              )
            )
          : phase === "gameover"
            ? h("section", { className: "empty-state" },
                h("div", { className: "compass" }, "☠"),
                h("p", { className: "eyebrow danger-text" }, "La flamme s’est éteinte"),
                h("h1", null, "Votre aventure prend fin"),
                h("p", null, "Les Terres oubliées se souviendront de votre passage."),
                h("button", { className: "primary", onClick: onReset }, "Recommencer")
              )
            : h("section", { className: "empty-state" },
                h("div", { className: "compass" }, "✦"),
                h("p", { className: "eyebrow" }, "La route vous appelle"),
                h("h1", null, "Que cache la brume ?"),
                h("p", null, "Explorez les environs. Vous pourriez trouver un trésor… ou réveiller une créature ancienne."),
                h("button", { className: "primary explore", disabled: busy, onClick: onExplore }, busy ? "Dans la brume…" : "Explorer la zone", h("span", null, "→"))
              ),
        h("section", { className: "log panel", "aria-live": "polite" },
          h("div", { className: "section-title" }, h("h3", null, "Chronique récente")),
          log.length ? log.map((entry, index) => h("p", { key: index }, h("span", null, "◆"), entry)) : h("p", { className: "muted" }, "Une page blanche attend vos exploits.")
        )
      )
    )
  );
}

function App() {
  const [game, setGame] = useState(null);
  const [busy, setBusy] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    api("/").then(setGame).catch(error => setError(error.message)).finally(() => setBusy(false));
  }, []);

  const run = async (operation) => {
    setBusy(true);
    setError("");
    try { setGame(await operation()); }
    catch (error) { setError(error.message); }
    finally { setBusy(false); }
  };

  const reset = async () => {
    if (!window.confirm("Quitter cette partie et supprimer la sauvegarde ?")) return;
    setBusy(true);
    try {
      await api("/save", { method: "DELETE" });
      setGame({ hasSave: false, phase: "welcome", hero: null, log: [] });
    } catch (error) {
      setError(error.message);
    } finally {
      setBusy(false);
    }
  };

  if (!game) return h("div", { className: "loader" }, h("span", null, "◆"), "Ouverture du grimoire…");

  return h(React.Fragment, null,
    error && h("div", { className: "toast", role: "alert" }, error, h("button", { onClick: () => setError("") }, "×")),
    !game.hero
      ? h(Welcome, {
          hasSave: game.hasSave,
          busy,
          onStart: (name, heroClass) => run(() => api("/new", { method: "POST", body: JSON.stringify({ name, heroClass }) })),
          onLoad: () => run(() => api("/load", { method: "POST" }))
        })
      : h(Adventure, {
          game,
          busy,
          onExplore: () => run(() => api("/explore", { method: "POST" })),
          onAction: action => run(() => api("/action", { method: "POST", body: JSON.stringify({ action }) })),
          onSave: () => run(() => api("/save", { method: "POST" })),
          onReset: reset
        })
  );
}

createRoot(document.getElementById("root")).render(h(App));
