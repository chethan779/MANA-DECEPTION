public class Mage : Character
{
    public int Mana { get; protected set; }
    public int MaxMana { get; protected set; }
    public bool IsSuppressingMana { get; private set; }
    public bool HasDefensiveBarrier { get; private set; }

    // Mage events
    public event Action<Mage, string>? SpellCast;
    public event Action<Mage, bool>? ManaSuppressionChanged;

    public Mage(string name, int maxHealth) : this(name, maxHealth, 100)
    {
    }

    public Mage(string name, int maxHealth, int maxMana) : base(name, maxHealth)
    {
        if (maxMana <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxMana), "Max mana must be greater than 0.");

        MaxMana = maxMana;
        Mana = maxMana;
        IsSuppressingMana = false;
        HasDefensiveBarrier = false;
    }

    /// <summary>
    /// Perceived mana seen by opponents.
    /// Flamme's teaching: by concealing mana, the opponent fails to estimate the mage's true strength.
    /// </summary>
    public int PerceivedMana => IsSuppressingMana ? Math.Max(1, MaxMana / 10) : Mana;

    /// <summary>
    /// Toggles the suppression of mana aura.
    /// </summary>
    public void ToggleManaSuppression()
    {
        IsSuppressingMana = !IsSuppressingMana;
        if (IsSuppressingMana)
        {
            Console.WriteLine($"🎭 {Name} flawlessly restricts their mana flow. To outside eyes, their mana appears tiny ({PerceivedMana} MP)!");
        }
        else
        {
            Console.WriteLine($"🌊 {Name} releases their mana concealment! A colossal, breathtaking torrent of mana erupts ({Mana}/{MaxMana} MP)!");
        }
        ManaSuppressionChanged?.Invoke(this, IsSuppressingMana);
    }

    public void SetManaSuppression(bool suppress)
    {
        if (IsSuppressingMana != suppress)
            ToggleManaSuppression();
    }

    /// <summary>
    /// Basic spell casting method (retained for backward compatibility).
    /// </summary>
    public virtual void CastSpell(Character target)
    {
        CastBasicSpell(target);
    }

    /// <summary>
    /// Standard magical projectile attack.
    /// </summary>
    public virtual bool CastBasicSpell(Character target)
    {
        ArgumentNullException.ThrowIfNull(target);

        const int manaCost = 10;
        const int damage = 20;

        if (!IsAlive)
        {
            Console.WriteLine($"{Name} cannot cast spells because they are defeated.");
            return false;
        }

        if (Mana < manaCost)
        {
            Console.WriteLine($"{Name} does not have enough mana to cast basic magic! ({Mana}/{manaCost} required)");
            return false;
        }

        Mana -= manaCost;
        Console.WriteLine($"🪄 {Name} casts a spell on {target.Name}!");
        SpellCast?.Invoke(this, "Basic Magic");
        target.TakeDamage(damage);
        return true;
    }

    /// <summary>
    /// Zoltraak (Ordinary Offensive Magic) - Humanity's refined demon-piercing offensive spell.
    /// Deals devastating damage, especially against Demons!
    /// </summary>
    public virtual bool CastZoltraak(Character target)
    {
        ArgumentNullException.ThrowIfNull(target);

        const int manaCost = 25;
        int baseDamage = 45;

        if (!IsAlive)
        {
            Console.WriteLine($"{Name} cannot cast Zoltraak because they are defeated.");
            return false;
        }

        if (Mana < manaCost)
        {
            Console.WriteLine($"{Name} lacks the mana for Zoltraak! ({Mana}/{manaCost} required)");
            return false;
        }

        Mana -= manaCost;

        bool isDemon = target is Demon;
        int actualDamage = isDemon ? (int)(baseDamage * 1.5) : baseDamage;

        Console.WriteLine($"✨ {Name} casts [Zoltraak - Ordinary Offensive Magic] targeting {target.Name}!");
        if (isDemon)
        {
            Console.WriteLine($"   ⚡ The piercing white beam tears through demonic defenses (+50% demon-slaying bonus)!");
        }

        SpellCast?.Invoke(this, "Zoltraak");

        if (target is Demon demon)
        {
            demon.TakeDamageFrom(this, actualDamage);
        }
        else
        {
            target.TakeDamage(actualDamage);
        }

        return true;
    }

    /// <summary>
    /// Deploys a hexagonal defensive barrier that heavily absorbs the next incoming strike.
    /// </summary>
    public virtual bool DeployDefensiveBarrier()
    {
        const int manaCost = 15;

        if (!IsAlive)
        {
            Console.WriteLine($"{Name} cannot deploy a barrier while defeated.");
            return false;
        }

        if (Mana < manaCost)
        {
            Console.WriteLine($"{Name} does not have enough mana for a Defensive Barrier! ({Mana}/{manaCost} required)");
            return false;
        }

        Mana -= manaCost;
        HasDefensiveBarrier = true;
        Console.WriteLine($"🛡️ {Name} deploys a hexagonal [Defensive Barrier]! Incoming attack damage will be heavily absorbed.");
        SpellCast?.Invoke(this, "Defensive Barrier");
        return true;
    }

    /// <summary>
    /// Meditates to gather natural mana.
    /// </summary>
    public virtual int Meditate(int amount = 30)
    {
        if (!IsAlive)
        {
            Console.WriteLine($"{Name} cannot meditate while defeated.");
            return 0;
        }

        int previousMana = Mana;
        Mana = Math.Min(MaxMana, Mana + amount);
        int recovered = Mana - previousMana;
        Console.WriteLine($"🧘 {Name} calms their mind and focuses mana flow, recovering {recovered} MP ({Mana}/{MaxMana}).");
        return recovered;
    }

    public override int TakeDamage(int amount)
    {
        if (HasDefensiveBarrier && IsAlive && amount > 0)
        {
            int absorbed = (int)(amount * 0.75);
            int residual = amount - absorbed;
            Console.WriteLine($"🛡️ {Name}'s hexagonal barrier absorbs {absorbed} damage, fracturing from the impact!");
            HasDefensiveBarrier = false;
            return base.TakeDamage(residual);
        }

        return base.TakeDamage(amount);
    }

    public override string ToString() =>
        $"{Name} [Mage] (HP: {Health}/{MaxHealth}, MP: {Mana}/{MaxMana}, Perceived MP: {PerceivedMana}, Status: {(IsAlive ? "Alive" : "Defeated")})";
}
