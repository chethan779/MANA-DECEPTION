public class Demon : Character
{
    public int Mana { get; protected set; }
    public int MaxMana { get; protected set; }
    public string InnateMagicName { get; init; }
    public string Title { get; init; }
    public int PrideLevel { get; protected set; } // Pride in their vast mana reserves

    // Demonic event hooks
    public event Action<Demon, string>? InnateMagicCast;

    public Demon(string name, int maxHealth, int maxMana = 120, string innateMagicName = "Original Zoltraak", string title = "Demon")
        : base(name, maxHealth)
    {
        if (maxMana <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxMana), "Max mana must be greater than 0.");

        MaxMana = maxMana;
        Mana = maxMana;
        InnateMagicName = innateMagicName;
        Title = title;
        PrideLevel = 100;
    }

    /// <summary>
    /// In Frieren lore, demons view mana as their absolute hierarchy, identity, and status.
    /// They cannot conceive of suppressing mana because it defies demonic pride.
    /// If an opponent conceals mana, the demon miscalculates their threat level.
    /// </summary>
    public bool UnderestimatesOpponent(Character opponent)
    {
        if (opponent is Mage mage)
        {
            return mage.PerceivedMana < (Mana / 2);
        }
        return false;
    }

    /// <summary>
    /// Executes the demon's innate signature magic (e.g. Auserlese, Baelsewer, Original Zoltraak).
    /// </summary>
    public virtual bool CastInnateMagic(Character target)
    {
        ArgumentNullException.ThrowIfNull(target);

        const int manaCost = 30;
        const int damage = 38;

        if (!IsAlive)
        {
            Console.WriteLine($"{Name} cannot cast innate magic because they are defeated.");
            return false;
        }

        if (Mana < manaCost)
        {
            Console.WriteLine($"{Name} attempts to summon [{InnateMagicName}], but lacks sufficient mana! ({Mana}/{manaCost} required)");
            Attack(target, 12);
            return false;
        }

        Mana -= manaCost;
        Console.WriteLine($"👿 [{Title}] {Name} activates their terrifying innate magic: [{InnateMagicName}] against {target.Name}!");
        InnateMagicCast?.Invoke(this, InnateMagicName);

        if (target is Mage mage && UnderestimatesOpponent(mage))
        {
            Console.WriteLine($"   \"{target.Name}... your mana is insignificant. Bow before superior power!\" mocks {Name}.");
        }

        target.TakeDamage(damage);
        return true;
    }

    /// <summary>
    /// Demonic mana blast or ranged assault.
    /// </summary>
    public virtual void CastSpell(Character target)
    {
        ArgumentNullException.ThrowIfNull(target);

        if (!IsAlive)
        {
            Console.WriteLine($"{Name} cannot cast spells because they are defeated.");
            return;
        }

        if (Mana >= 15)
        {
            Mana -= 15;
            Console.WriteLine($"🔮 {Name} releases a wave of dark mana projectiles at {target.Name}!");
            target.TakeDamage(24);
        }
        else
        {
            Attack(target, 12);
        }
    }

    /// <summary>
    /// Deals damage while factoring in whether the demon underestimated a mage who revealed their true mana.
    /// </summary>
    public virtual int TakeDamageFrom(Character attacker, int amount)
    {
        int finalDamage = amount;
        if (attacker is Mage mage && UnderestimatesOpponent(mage) && !mage.IsSuppressingMana)
        {
            Console.WriteLine($"💥 {Name}'s composure shatters upon realizing {attacker.Name}'s disguised mana! Defensive gap exposed!");
            finalDamage = (int)(amount * 1.35);
        }

        return TakeDamage(finalDamage);
    }

    /// <summary>
    /// Passively draws mana from surrounding atmospheric flow.
    /// </summary>
    public virtual int RegenerateMana(int amount = 20)
    {
        if (!IsAlive) return 0;

        int previousMana = Mana;
        Mana = Math.Min(MaxMana, Mana + amount);
        int recovered = Mana - previousMana;
        Console.WriteLine($"🌀 {Name} gathers demonic mana from the surrounding aura (+{recovered} MP, now {Mana}/{MaxMana}).");
        return recovered;
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Console.WriteLine($"✨ {Name}'s demonic body disintegrates into glowing mana particles, scattering endlessly into the wind...");
    }

    public override string ToString() =>
        $"{Name} [{Title}] (HP: {Health}/{MaxHealth}, MP: {Mana}/{MaxMana}, Magic: {InnateMagicName}, Status: {(IsAlive ? "Alive" : "Defeated")})";
}

