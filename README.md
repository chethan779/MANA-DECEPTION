<div align="center">

# 🪄 Mana : Deception 🪄
### *A Reactive Tactical Combat Simulator Built on .NET 10 & C# 13*

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-13.0-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![Architecture](https://img.shields.io/badge/Architecture-Event--Driven%20OOP-0284c7?style=for-the-badge)](#-architecture--design-patterns)
[![Documentation](https://img.shields.io/badge/Docs-33--Page%20PDF-9333ea?style=for-the-badge&logo=adobeacrobatreader&logoColor=white)](Mana_Deception_Technical_Documentation.pdf)
[![License](https://img.shields.io/badge/License-MIT-f59e0b?style=for-the-badge)](LICENSE)

<br/>

> *"In the era of myths, mages spent centuries training their visualization and mana control.  
> But demons are creatures of mana—they view mana as absolute authority and hierarchy.  
> To deceive a demon, one must conceal their true strength until the decisive moment."*

<br/>

```text
╔═════════════════════════════════════════════════════════════════╗
║               🪄   M A N A  :  D E C E P T I O N   🪄             ║
║             Tactical Combat System — Built with .NET 10         ║
╚═════════════════════════════════════════════════════════════════╝
```

</div>

---

## 📖 Table of Contents

- [Overview](#-overview)
- [The Core Philosophy of Deception](#-the-core-philosophy-of-deception)
- [Key Features](#-key-features)
- [Combat & Mathematical Mechanics](#-combat--mathematical-mechanics)
  - [1. Mana Suppression & Demonic Perception](#1-mana-suppression--demonic-perception)
  - [2. Guard-Break Vulnerability Multiplier](#2-guard-break-vulnerability-multiplier)
  - [3. Multi-Tier Damage Mitigation Pipeline](#3-multi-tier-damage-mitigation-pipeline)
  - [4. Demon-Slaying Spellcraft (Zoltraak)](#4-demon-slaying-spellcraft-zoltraak)
- [Playable Scenarios](#-playable-scenarios)
- [Architecture & Design Patterns](#-architecture--design-patterns)
- [Project Structure](#-project-structure)
- [Quick Start](#-quick-start)
- [Technical Documentation (PDF)](#-technical-documentation-pdf)
- [License](#-license)

---

## 🌟 Overview

**🪄 Mana : Deception 🪄** is a turn-based tactical combat simulator written in modern **C# 13** on **.NET 10**. Inspired by the high-stakes magical warfare of *Frieren: Beyond Journey's End*, the game translates deep mana concealment techniques, psychological warfare, and reactive defense into an elegant, decoupled object-oriented system.

Rather than relying purely on raw stats, victory hinges on **information asymmetry**:
- Can you lure a proud demon into underestimating your capabilities?
- Can you withstand their overwhelming innate magic with a hexagonal barrier?
- Will your guard-breaking reveal shatter their defenses for a lethal *Zoltraak* strike?

---

## 🎭 The Core Philosophy of Deception

Demons are monsters composed entirely of mana. To them, visible mana is an immutable indicator of societal standing and destructive potential. They cannot comprehend suppressing mana because doing so defies their fundamental pride.

In **Mana : Deception**:
1. **Concealment:** Mages restrict their visible mana to a mere fraction of their actual pool.
2. **Underestimation:** Demons calculate threat strictly based on perceived mana. If they judge an opponent weak, they lower their defensive vigilance and cast recklessly.
3. **The Reveal:** Releasing mana suppression shatters the demon's composure, inflicting an irreversible **Guard-Break** that exposes them to catastrophic damage.

---

## ⚡ Key Features

- **🎮 Rich Turn-Based CLI:** Features dynamic Unicode ASCII HUDs showing real-time health and mana gauges with status indicator badges.
- **🎬 Cinematic Simulation Engine:** Includes a standalone automated mode (`--demo`) demonstrating the full combat arc from initial deception to demonic execution.
- **🛡️ 3-Tier Defensive Mitigation:** Accurately stacks demon vulnerability bonuses, hexagonal barrier absorption (75%), and physical bracing (50%).
- **📡 Event-Driven Reactive Architecture:** Utilizes multicast delegates (`Action<T...>`) for combat lifecycle hooks (`Damaged`, `Healed`, `Defeated`, `SpellCast`, `ManaSuppressionChanged`, `InnateMagicCast`).
- **🧠 Priority-Tree Demonic AI:** Enemies intelligently switch between signature innate magic, dark mana bursts, desperation guards, and atmospheric mana regeneration.
- **📦 Clean & Zero-Dependency:** Written with 100% pure Base Class Library (BCL) code—no external NuGet packages required.

---

## 🪄 Combat & Mathematical Mechanics

### 1. Mana Suppression & Demonic Perception

When mana suppression is active (`IsSuppressingMana = true`), apparent mana is restricted to **10% of maximum capacity**:

$$\text{PerceivedMana} = \max\left(1, \left\lfloor \frac{\text{MaxMana}}{10} \right\rfloor\right)$$

*Example: A master mage with $200$ MaxMana appears to demonic senses as having a trivial $20$ MP.*

### 2. Guard-Break Vulnerability Multiplier

Demons evaluate opponents strictly by comparing perceived mana against their own reserves:

$$\text{UnderestimatesOpponent} = \text{PerceivedMana} < \left\lfloor \frac{\text{Demon.Mana}}{2} \right\rfloor$$

When the mage unveils their true aura (`IsSuppressingMana = false`), the demon's miscalculation results in a **+35% vulnerability damage penalty**:

$$\text{Damage}_{\text{Final}} = \lfloor \text{Damage}_{\text{Base}} \times 1.35 \rfloor$$

### 3. Multi-Tier Damage Mitigation Pipeline

Every attack resolves through three deterministic layers:

```text
Incoming Attack Damage
         │
         ▼
[Layer 1] Demon Guard Break ─── (+35% if demon underestimated & mage unsuppressed)
         │
         ▼
[Layer 2] Hexagonal Barrier ─── (Absorbs 75% damage, fractures after impact)
         │
         ▼
[Layer 3] Defensive Stance  ─── (Halves remaining residual damage)
         │
         ▼
Subtracted from Target Health (bounded at 0)
```

### 4. Demon-Slaying Spellcraft (Zoltraak)

Originally developed by the demon Qual as piercing destruction magic, humanity decoded and refined Zoltraak into "ordinary offensive magic", specifically lethal against demons:

| Spell / Action | Mana Cost | Base Damage | Lore & Combat Effect |
| :--- | :---: | :---: | :--- |
| **Staff Strike** | `0 MP` | 10 | Basic physical staff strike; preserves mana. |
| **Basic Magic** | `10 MP` | 20 | Standard magical projectile attack. |
| **Zoltraak** | `25 MP` | 45 | **67 dmg (+50% bonus)** against Demons. Triggers guard-break vulnerability. |
| **Defensive Barrier** | `15 MP` | 0 | Manifests a hexagonal shield absorbing 75% of the next attack. |
| **Meditate** | `0 MP` | 0 | Calms the mind and channels natural mana (+30 MP). |
| **Innate Demonic Magic** | `30 MP` | 38 | Devastating unique demonic art (*Auserlese*, *Baelsewer*, *Original Zoltraak*). |

---

## ⚔️ Playable Scenarios

Choose from three canonical encounters in the main menu:

| Scenario | Player Combatant | Demonic Opponent | Conflict Dynamic |
| :--- | :--- | :--- | :--- |
| **1. The Scales of Obedience** | **Frieren**<br>*(120 HP / 200 MP)* | **Aura the Guillotine**<br>*(110 HP / 160 MP)* | A thousand years of continuous mana suppression collides with Aura's absolute scales of soul weight. |
| **2. Emissary of Blood** | **Fern**<br>*(90 HP / 110 MP)* | **Lugner**<br>*(95 HP / 100 MP)* | Relentless high-speed Zoltraak barrages against malleable demonic blood manipulation. |
| **3. Elder Sage of Corruption** | **Frieren**<br>*(120 HP / 200 MP)* | **Qual**<br>*(140 HP / 180 MP)* | The legendary inventor of Zoltraak faces humanity's refined modern offensive and defensive magic. |

---

## 🏗️ Architecture & Design Patterns

The engine is built around decoupled, extensible Object-Oriented principles:

```text
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
                     │ • Interactive Loop    │
                     │ • Demonic AI Engine   │
                     │ • Automated Demo      │
                     └───────────────────────┘
```

- **Inheritance & Polymorphism:** [`Character`](Character.cs) is extended by [`Mage`](Mage.cs) and [`Demon`](Demon.cs) with custom damage resolution and death lifecycle sequences.
- **Encapsulation:** Invariants strictly protect valid health and mana states while disallowing self-targeting or actions while defeated.
- **Multicast Event Handling:** Lifecycle notifications decouple game rules from console visualization.

---

## 📁 Project Structure

```text
Frieren-Game/
├── Character.cs                               # Base combatant domain model (HP, mitigation, events)
├── Mage.cs                                    # Magic-user subclass (Mana, Zoltraak, barrier, suppression)
├── Demon.cs                                   # Demon subclass (Innate magic, perception, guard-break)
├── Program.cs                                 # Main menu, HUD renderer, demonic AI, and battle loops
├── FrierenCombat.csproj                       # .NET 10 project definition (Assembly: ManaDeception)
├── Mana_Deception_Technical_Documentation.pdf # 33-page comprehensive technical reference manual
└── .gitignore                                 # Build artifact and cache exclusions
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

3. **Run the cinematic automated demonstration:**
   ```bash
   dotnet run -- --demo
   ```

### Interactive HUD & Controls Preview

```text
─────────────────────────────────────────────────────────────────
▶ TURN 2
─────────────────────────────────────────────────────────────────
🧙 Frieren      HP: [██████████████░░] 110/120 | MP: [███████████░] 185/200 [🎭 Concealing Mana]
👿 Aura         HP: [████████████████] 110/110 | MP: [██████████░░] 130/160 [⚠️ Underestimating Opponent]

Choose your action:
 [1] ✨ Cast Zoltraak (Ordinary Offensive Magic) [25 MP, 45-67 dmg, +50% vs Demon]
 [2] 🛡️ Erect Hexagonal Defensive Barrier [15 MP, absorbs 75% next attack]
 [3] 🎭 Toggle Mana Suppression [Current: Suppressed (Deceiving Demon)]
 [4] 🪄 Cast Basic Magic [10 MP, 20 dmg]
 [5] 🦯 Staff Strike [0 MP, 10 dmg]
 [6] 🧘 Meditate [Restores 30 MP]
 [7] 🛡️ Defensive Block [Halves next turn damage]
Action > 
```

---

## 📄 Technical Documentation (PDF)

A comprehensive, 33-page **Technical Reference Manual** is included directly in the root of the repository:
- **File:** [`Mana_Deception_Technical_Documentation.pdf`](Mana_Deception_Technical_Documentation.pdf)
- **Contents:** Complete line-by-line method call maps, execution flowcharts, exact formulas, class member specifications, and suggested curriculum extension assignments.

---

## 📜 License

This project is licensed under the **MIT License**. Free for educational, personal, and open-source use.
