# ✦ Frieren: Beyond Journey's End — Reactive Combat System ✦

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-13.0-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![Architecture](https://img.shields.io/badge/Architecture-Event--Driven%20OOP-blue)](#architecture--design-patterns)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

An event-driven, turn-based tactical combat simulator written in **C# 13** and **.NET 10**, faithfully implementing the magic system, perception mechanics, and combat philosophy of *Frieren: Beyond Journey's End (葬送のフリーレン)*.

---

## 📖 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Combat & Lore Mechanics](#-combat--lore-mechanics)
  - [1. Mana Suppression & Demonic Deception](#1-mana-suppression--demonic-deception)
  - [2. Multi-Layered Damage Mitigation](#2-multi-layered-damage-mitigation)
  - [3. Refined Demon-Slaying Magic (Zoltraak)](#3-refined-demon-slaying-magic-zoltraak)
- [Playable Scenarios](#-playable-scenarios)
- [Architecture & Design Patterns](#-architecture--design-patterns)
- [Project Structure](#-project-structure)
- [Quick Start](#-quick-start)
- [Technical Documentation](#-technical-documentation)
- [Contributing & License](#-contributing--license)

---

## 🌟 Overview

In the world of *Frieren*, magic is an art form driven by visualization, mana control, and deception. Demons view mana as an absolute hierarchy—an unshakeable metric of strength and authority—making the concept of mana suppression unthinkable to them.

This project models these interactions in code:
- **Mages** can restrict their visible mana to a fraction of their true capacity.
- **Demons** judge opponent threat strictly based on perceived mana.
- Revealing true mana shatters demonic composure, leaving them stunned and vulnerable to devastating guard-break attacks.

---

## ⚡ Key Features

- **🎮 Playable Interactive CLI:** Rich turn-based combat with real-time ASCII status HUDs and dynamic health/mana progress bars.
- **🎬 Cinematic Automated Simulation:** Run headless or automated battle replays with `--demo`.
- **⚔️ Multi-Tier Damage Pipeline:** Factors demonic guard-break (+35%), hexagonal barriers (-75%), and physical defensive stances (-50%).
- **📡 Multicast Event Architecture:** Lifecycle events (`Damaged`, `Healed`, `Defeated`, `SpellCast`, `ManaSuppressionChanged`, `InnateMagicCast`) keep the domain model cleanly decoupled from the UI.
- **🧠 Rule-Based Demonic AI:** Enemy decision-making prioritizes signature spells, mana thresholds, desperation blocks, and atmospheric mana regeneration.
- **📦 Zero External Dependencies:** Compiles natively against the .NET 10 Base Class Library without external NuGet packages.

---

## 🪄 Combat & Lore Mechanics

### 1. Mana Suppression & Demonic Deception
Taught to Frieren by the great mage Flamme, mana concealment restricts apparent mana to **10% of maximum capacity**:

$$\text{PerceivedMana} = \max\left(1, \left\lfloor \frac{\text{MaxMana}}{10} \right\rfloor\right)$$

- **Demonic Underestimation:** If $\text{PerceivedMana} < \frac{\text{Demon.Mana}}{2}$, the demon mocks the mage and lowers their guard.
- **The Reveal (Guard Break):** When suppression is dropped, the sudden revelation of immense mana inflicts a **+35% vulnerability damage penalty** on the demon:

$$\text{Damage}_{\text{GuardBreak}} = \lfloor \text{Damage} \times 1.35 \rfloor$$

### 2. Multi-Layered Damage Mitigation
Incoming attacks pass through three defensive layers:

```
Incoming Attack Damage
         │
         ▼
[1] Demon Guard Break Check ─── (+35% if deceived & unsuppressed)
         │
         ▼
[2] Hexagonal Defensive Barrier  ─── (Absorbs 75% damage, then breaks)
         │
         ▼
[3] Physical Stance Block        ─── (Halves remaining residual damage)
         │
         ▼
Subtracted from HP (min 0)
```

### 3. Refined Demon-Slaying Magic (Zoltraak)
Originally invented by the demon **Qual** as destruction magic, Zoltraak was decoded and refined by humans into "ordinary offensive magic":
- Standard damage: **45 dmg**
- Against Demons: **67 dmg (+50% bonus)**

---

## ⚔️ Playable Scenarios

The main menu features three canonical lore encounters:

| Scenario | Player | Opponent | Lore Theme |
| :--- | :--- | :--- | :--- |
| **1. The Scales of Obedience** | **Frieren** (120 HP / 200 MP) | **Aura the Guillotine** (110 HP / 160 MP) | A millennium of mana suppression meets Aura's absolute scales. |
| **2. Emissary of Blood** | **Fern** (90 HP / 110 MP) | **Lugner** (95 HP / 100 MP) | Rapid, relentless Zoltraak casting vs malleable blood magic. |
| **3. Elder Sage of Corruption** | **Frieren** (120 HP / 200 MP) | **Qual** (140 HP / 180 MP) | The father of Zoltraak encounters humanity's refined modern magic. |

---

## 🏗️ Architecture & Design Patterns

The codebase is engineered around clean Object-Oriented principles:

```
                 ┌────────────────────────┐
                 │       Character        │
                 │ ────────────────────── │
                 │ • Name, Health, HP Bar │
                 │ • Block(), TakeDamage()│
                 │ • Damaged / Defeated   │
                 └───────────┬────────────┘
                             │
              ┌──────────────┴──────────────┐
              │                             │
              ▼                             ▼
   ┌────────────────────┐        ┌────────────────────┐
   │        Mage        │        │       Demon        │
   │ ────────────────── │        │ ────────────────── │
   │ • Mana & MaxMana   │        │ • Innate Magic     │
   │ • PerceivedMana    │        │ • Demonic Pride    │
   │ • CastZoltraak()   │        │ • EvaluateMana()   │
   │ • Barrier & Shield │        │ • TakeDamageFrom() │
   │ • Mana Suppression │        │ • Mana Dissolution │
   └────────────────────┘        └────────────────────┘
              ▲                             ▲
              │                             │
              └──────────────┬──────────────┘
                             │
                 ┌───────────────────────┐
                 │        Program        │
                 │ ───────────────────── │
                 │ • CLI Game Loop       │
                 │ • Demonic AI Engine   │
                 │ • Automated Demo      │
                 └───────────────────────┘
```

- **Inheritance & Polymorphism:** [`Character`](Character.cs) is specialized by [`Mage`](Mage.cs) and [`Demon`](Demon.cs) with overridden damage calculations and lifecycle hooks.
- **Encapsulation:** Invariants on health, mana, and non-empty character names protected with guard clauses.
- **Event-Driven Reactivity:** Actions dispatch multicast delegates (`Action<T...>`) allowing UI and logging systems to remain decoupled from combat rules.

---

## 📁 Project Structure

```text
Frieren-Game/
├── Character.cs                                 # Base combatant class (HP, damage mitigation, events)
├── Mage.cs                                      # Magic-user subclass (Mana, Zoltraak, barrier, suppression)
├── Demon.cs                                     # Antagonist subclass (Innate magic, perception, guard-break)
├── Program.cs                                   # Interactive menu, AI logic, and automated simulation
├── FrierenCombat.csproj                         # .NET 10 project definition
├── Frieren_Combat_Technical_Documentation.pdf   # 33-page comprehensive technical reference manual
└── .gitignore                                   # Standard .NET build output exclusions
```

---

## 🚀 Quick Start

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) (or later)

### Installation & Run

1. **Clone the repository:**
   ```bash
   git clone https://github.com/chethan779/Frieren-Game.git
   cd Frieren-Game
   ```

2. **Launch the interactive battle menu:**
   ```bash
   dotnet run
   ```

3. **Run the cinematic automated simulation:**
   ```bash
   dotnet run -- --demo
   ```

### In-Game Combat Controls

During interactive combat, choose from tactical actions each round:
```text
Choose your action:
 [1] ✨ Cast Zoltraak (Ordinary Offensive Magic) [25 MP, 45-67 dmg, +50% vs Demon]
 [2] 🛡️ Erect Hexagonal Defensive Barrier [15 MP, absorbs 75% next attack]
 [3] 🎭 Toggle Mana Suppression [Current: Suppressed / Full Aura]
 [4] 🪄 Cast Basic Magic [10 MP, 20 dmg]
 [5] 🦯 Staff Strike [0 MP, 10 dmg]
 [6] 🧘 Meditate [Restores 30 MP]
 [7] 🛡️ Defensive Block [Halves next turn damage]
```

---

## 📄 Technical Documentation

A complete, 33-page **Technical Reference Manual** is included directly in the repository:
- **File:** [`Frieren_Combat_Technical_Documentation.pdf`](Frieren_Combat_Technical_Documentation.pdf)
- **Covers:** Line-by-line method call maps, state machines, math formulas, class member specifications, and pedagogical extensions for instructors and students.

---

## 📜 License

This project is licensed under the **MIT License**. Free for educational, personal, and open-source use.

