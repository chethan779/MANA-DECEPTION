using System;
using System.Text;

namespace ManaDeception;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        // If run with arguments or in non-interactive / redirected mode, execute the automated demo
        if (args.Length > 0 && (args[0] == "--demo" || args[0] == "--test" || args[0] == "-d") || Console.IsInputRedirected)
        {
            RunAutomatedDemonstration();
            return;
        }

        RunMainMenu();
    }

    private static void RunMainMenu()
    {
        while (true)
        {
            Console.Clear();
            PrintBanner();
            Console.WriteLine(" Select a combat scenario:");
            Console.WriteLine(" [1] ⚔️  Interactive Duel: Frieren vs Aura the Guillotine");
            Console.WriteLine(" [2] 🪄  Interactive Duel: Fern vs Lugner");
            Console.WriteLine(" [3] ⚡  Interactive Boss Duel: Frieren vs Qual (Elder Sage of Corruption)");
            Console.WriteLine(" [4] 🎬  Cinematic Automated Battle Simulation");
            Console.WriteLine(" [5] 🚪  Exit");
            Console.WriteLine();
            Console.Write("Enter your choice (1-5): ");

            string? choice = Console.ReadLine()?.Trim();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    RunInteractiveBattle(
                        player: new Mage("Frieren", 120, 200),
                        boss: new Demon("Aura the Guillotine", 110, 160, "Auserlese (Scales of Obedience)", "Great Demon of the Seven Sages of Destruction"),
                        startSuppressed: true,
                        loreIntro: "Aura measures mana on her Scales of Obedience. Unbeknownst to her, Frieren has spent over a millennium mastering mana suppression."
                    );
                    break;
                case "2":
                    RunInteractiveBattle(
                        player: new Mage("Fern", 90, 110),
                        boss: new Demon("Lugner", 95, 100, "Baelsewer (Blood Magic)", "Demon Emissary"),
                        startSuppressed: false,
                        loreIntro: "Lugner relies on malleable blood magic. Fern counters with rapid, relentless casting of Zoltraak."
                    );
                    break;
                case "3":
                    RunInteractiveBattle(
                        player: new Mage("Frieren", 120, 200),
                        boss: new Demon("Qual", 140, 180, "Original Zoltraak (Elder Destruction Magic)", "Elder Sage of Corruption"),
                        startSuppressed: false,
                        loreIntro: "Qual invented Zoltraak 80 years ago. Today, humanity has refined it into ordinary offensive and defensive magic."
                    );
                    break;
                case "4":
                    RunAutomatedDemonstration();
                    break;
                case "5":
                case "q":
                case "exit":
                    Console.WriteLine("Farewell, traveler of the northern lands.");
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to return to menu...");
                    Console.ReadKey(true);
                    break;
            }
        }
    }

    private static void RunInteractiveBattle(Mage player, Demon boss, bool startSuppressed, string loreIntro)
    {
        Console.Clear();
        PrintBanner();
        Console.WriteLine($"📜 Encounter: {player.Name} vs {boss.Name} ({boss.Title})");
        Console.WriteLine($"   {loreIntro}");
        Console.WriteLine();

        if (startSuppressed)
        {
            player.SetManaSuppression(true);
        }

        // Attach event listeners for battle narrations
        player.Defeated += chara => Console.WriteLine($"💀 [Combat Event] {chara.Name} has succumbed to fatal injuries!");
        boss.Defeated += chara => Console.WriteLine($"🎉 [Combat Event] {chara.Name} has been purged from the mortal realm!");

        int turn = 1;

        while (player.IsAlive && boss.IsAlive)
        {
            Console.WriteLine(new string('─', 65));
            Console.WriteLine($"▶ TURN {turn}");
            Console.WriteLine(new string('─', 65));

            DisplayCombatantHUD(player);
            DisplayCombatantHUD(boss, player);
            Console.WriteLine();

            // Player Turn
            bool actionExecuted = false;
            while (!actionExecuted)
            {
                Console.WriteLine("Choose your action:");
                Console.WriteLine(" [1] ✨ Cast Zoltraak (Ordinary Offensive Magic) [25 MP, 45-67 dmg, +50% vs Demon]");
                Console.WriteLine(" [2] 🛡️  Erect Hexagonal Defensive Barrier [15 MP, absorbs 75% next attack]");
                Console.WriteLine($" [3] 🎭 Toggle Mana Suppression [Current: {(player.IsSuppressingMana ? "Suppressed (Deceiving Demon)" : "Full Aura Unleashed")}]");
                Console.WriteLine(" [4] 🪄  Cast Basic Magic [10 MP, 20 dmg]");
                Console.WriteLine(" [5] 🦯  Staff Strike [0 MP, 10 dmg]");
                Console.WriteLine(" [6] 🧘  Meditate [Restores 30 MP]");
                Console.WriteLine(" [7] 🛡️  Defensive Block [Halves next turn damage]");
                Console.Write("Action > ");

                string? input = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        actionExecuted = player.CastZoltraak(boss);
                        break;
                    case "2":
                        actionExecuted = player.DeployDefensiveBarrier();
                        break;
                    case "3":
                        player.ToggleManaSuppression();
                        actionExecuted = true;
                        break;
                    case "4":
                        actionExecuted = player.CastBasicSpell(boss);
                        break;
                    case "5":
                        player.Attack(boss, 10);
                        actionExecuted = true;
                        break;
                    case "6":
                        player.Meditate(30);
                        actionExecuted = true;
                        break;
                    case "7":
                        player.Block();
                        actionExecuted = true;
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please choose an action from 1 to 7.");
                        break;
                }
            }

            if (!boss.IsAlive)
                break;

            Console.WriteLine();
            Console.WriteLine($"── {boss.Name}'s Turn ──");

            // Demon AI logic
            ExecuteDemonTurn(boss, player);

            // Natural end-of-turn recovery
            boss.RegenerateMana(10);

            turn++;
            Console.WriteLine();
            Console.WriteLine("Press any key to continue to the next round...");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        Console.WriteLine(new string('═', 65));
        if (player.IsAlive)
        {
            Console.WriteLine($"✨ VICTORY: {player.Name} has triumphed over {boss.Name}!");
        }
        else
        {
            Console.WriteLine($"💀 DEFEAT: {boss.Name} overwhelmed {player.Name}.");
        }
        Console.WriteLine(new string('═', 65));

        Console.WriteLine("\nPress any key to return to the main menu...");
        Console.ReadKey(true);
    }

    private static void ExecuteDemonTurn(Demon demon, Mage opponent)
    {
        bool underestimating = demon.UnderestimatesOpponent(opponent);

        // Demon priority decision tree
        if (demon.Mana >= 30)
        {
            demon.CastInnateMagic(opponent);
        }
        else if (demon.Mana >= 15)
        {
            demon.CastSpell(opponent);
        }
        else if (demon.Health <= demon.MaxHealth * 0.3)
        {
            Console.WriteLine($"🛡️ {demon.Name} falls back into a defensive stance!");
            demon.Block();
        }
        else
        {
            demon.Attack(opponent, 14);
        }
    }

    private static void RunAutomatedDemonstration()
    {
        Console.WriteLine();
        PrintBanner();
        Console.WriteLine("📜 Running Automated Combat Simulation:");
        Console.WriteLine("   Frieren the Slayer vs Aura the Guillotine");
        Console.WriteLine("   (Demonstrating Mana Suppression deception, Zoltraak demon bonus, and Defensive Barrier)");
        Console.WriteLine(new string('═', 65));

        var frieren = new Mage("Frieren", 120, 200);
        var aura = new Demon("Aura the Guillotine", 110, 160, "Auserlese (Scales of Obedience)", "Seven Sages of Destruction");

        // Frieren conceals her mana
        frieren.SetManaSuppression(true);

        Console.WriteLine($"\n[Setup] Frieren's true mana: {frieren.Mana}/{frieren.MaxMana} MP. Apparent mana: {frieren.PerceivedMana} MP.");
        Console.WriteLine($"[Setup] Aura perceives Frieren's mana as trivial. Underestimating: {aura.UnderestimatesOpponent(frieren)}.");

        int step = 1;

        // Round 1: Aura casts innate magic, Frieren uses defensive barrier
        Console.WriteLine($"\n── Round {step++}: Aura strikes, Frieren prepares defense ──");
        frieren.DeployDefensiveBarrier();
        aura.CastInnateMagic(frieren);

        // Round 2: Frieren reveals true mana, turning the battle!
        Console.WriteLine($"\n── Round {step++}: The Reveal ──");
        Console.WriteLine("Frieren: \"Aura... the mana you are looking at is restricted.\"");
        frieren.SetManaSuppression(false);
        Console.WriteLine("Aura: \"Impossible... that is a deception spanning over a thousand years...!\"");

        // Round 3: Frieren casts demon-piercing Zoltraak
        Console.WriteLine($"\n── Round {step++}: Zoltraak Demonic Execution ──");
        frieren.CastZoltraak(aura);

        // Round 4: Finishing blow
        if (aura.IsAlive)
        {
            Console.WriteLine($"\n── Round {step++}: The Final Verdict ──");
            Console.WriteLine("Frieren: \"Aura... k**l yourself.\"");
            frieren.CastZoltraak(aura);
        }

        Console.WriteLine(new string('═', 65));
        Console.WriteLine($"Simulation concluded. Frieren status: {(frieren.IsAlive ? "Alive" : "Defeated")}, Aura status: {(aura.IsAlive ? "Alive" : "Defeated")}");
        Console.WriteLine(new string('═', 65));
    }

    private static void DisplayCombatantHUD(Character character, Mage? playerContext = null)
    {
        string hpBar = GenerateBar(character.Health, character.MaxHealth, 16);

        if (character is Mage mage)
        {
            string mpBar = GenerateBar(mage.Mana, mage.MaxMana, 12);
            string suppressionTag = mage.IsSuppressingMana ? " [🎭 Concealing Mana]" : " [🌊 Full Mana Aura]";
            string barrierTag = mage.HasDefensiveBarrier ? " [🛡️ Barrier Active]" : "";
            string blockTag = mage.IsBlocking ? " [🛡️ Blocking]" : "";

            Console.WriteLine($"🧙 {mage.Name,-12} HP: {hpBar} {mage.Health,3}/{mage.MaxHealth} | MP: {mpBar} {mage.Mana,3}/{mage.MaxMana}{suppressionTag}{barrierTag}{blockTag}");
        }
        else if (character is Demon demon)
        {
            string mpBar = GenerateBar(demon.Mana, demon.MaxMana, 12);
            string underestimationTag = (playerContext != null && demon.UnderestimatesOpponent(playerContext)) ? " [⚠️ Underestimating Opponent]" : "";
            string blockTag = demon.IsBlocking ? " [🛡️ Blocking]" : "";

            Console.WriteLine($"👿 {demon.Name,-12} HP: {hpBar} {demon.Health,3}/{demon.MaxHealth} | MP: {mpBar} {demon.Mana,3}/{demon.MaxMana}{underestimationTag}{blockTag}");
        }
        else
        {
            Console.WriteLine($"👤 {character.Name,-12} HP: {hpBar} {character.Health,3}/{character.MaxHealth}");
        }
    }

    private static string GenerateBar(int current, int max, int totalBlocks)
    {
        if (max <= 0) return new string('░', totalBlocks);
        int filled = Math.Clamp((int)Math.Round((double)current / max * totalBlocks), 0, totalBlocks);
        return $"[{new string('█', filled)}{new string('░', totalBlocks - filled)}]";
    }

    private static void PrintBanner()
    {
        Console.WriteLine("╔═════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║               🪄   M A N A  :  D E C E P T I O N   🪄             ║");
        Console.WriteLine("║             Tactical Combat System — Built with .NET 10         ║");
        Console.WriteLine("╚═════════════════════════════════════════════════════════════════╝");
        Console.WriteLine();
    }
}
