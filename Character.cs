
class Character
{
    public string Name { get; private set; }
    public int Health { get; protected set; }
    public int MaxHealth { get; private set; }
    public bool IsBlocking { get; private set; }
    public bool IsAlive => Health > 0;

    // Combat lifecycle events
    public event Action<Character, int>? Damaged;
    public event Action<Character, int>? Healed;
    public event Action<Character>? Defeated;

    public Character(string name, int maxHealth)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Character name cannot be null or empty.", nameof(name));

        if (maxHealth <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxHealth), "Max health must be greater than 0.");

        Name = name.Trim();
        MaxHealth = maxHealth;
        Health = maxHealth;
        IsBlocking = false;
    }

    public virtual void Block()
    {
        if (!IsAlive)
        {
            Console.WriteLine($"{Name} cannot block because they are defeated.");
            return;
        }

        if (IsBlocking)
        {
            Console.WriteLine($"{Name} is already in a defensive stance.");
            return;
        }

        IsBlocking = true;
        Console.WriteLine($"{Name} prepares to block incoming attacks!");
    }

    public void ResetBlock()
    {
        IsBlocking = false;
    }

    public virtual int TakeDamage(int amount)
    {
        if (!IsAlive)
        {
            Console.WriteLine($"{Name} is already defeated.");
            return 0;
        }

        if (amount <= 0)
        {
            Console.WriteLine($"{Name} was targeted, but took no damage.");
            return 0;
        }

        int actualDamage = amount;

        if (IsBlocking)
        {
            actualDamage = amount / 2;
            IsBlocking = false;
            Console.WriteLine($"{Name} blocks half the damage!");
        }

        int previousHealth = Health;
        Health = Math.Max(0, Health - actualDamage);
        int damageDealt = previousHealth - Health;

        Console.WriteLine($"{Name} takes {damageDealt} damage. HP: {Health}/{MaxHealth}");

        OnDamaged(damageDealt);

        if (!IsAlive)
        {
            OnDeath();
        }

        return damageDealt;
    }

    // Retained for backward compatibility with existing code calling Damage(amount)
    public virtual void Damage(int amount) => TakeDamage(amount);

    public virtual int Heal(int amount)
    {
        if (!IsAlive)
        {
            Console.WriteLine($"{Name} is defeated and cannot be healed.");
            return 0;
        }

        if (amount <= 0)
            return 0;

        int previousHealth = Health;
        Health = Math.Min(MaxHealth, Health + amount);
        int healedAmount = Health - previousHealth;

        if (healedAmount > 0)
        {
            Console.WriteLine($"{Name} heals for {healedAmount} HP. HP: {Health}/{MaxHealth}");
            OnHealed(healedAmount);
        }
        else
        {
            Console.WriteLine($"{Name} is already at full health (HP: {Health}/{MaxHealth}).");
        }

        return healedAmount;
    }

    public virtual void Attack(Character target, int damage = 10)
    {
        ArgumentNullException.ThrowIfNull(target);

        if (!IsAlive)
        {
            Console.WriteLine($"{Name} cannot attack because they are defeated.");
            return;
        }

        if (target == this)
        {
            Console.WriteLine($"{Name} cannot attack themselves.");
            return;
        }

        Console.WriteLine($"{Name} attacks {target.Name}!");
        target.TakeDamage(damage);
    }

    protected virtual void OnDamaged(int damageTaken)
    {
        // Trigger external subscribers
        Damaged?.Invoke(this, damageTaken);

        // Warning trigger when HP falls into critical range (<= 25%)
        if (IsAlive && Health <= MaxHealth * 0.25)
        {
            Console.WriteLine($"⚠️ [Critical] {Name} is critically injured! (HP: {Health}/{MaxHealth})");
        }
    }

    protected virtual void OnHealed(int amountHealed)
    {
        // Trigger external subscribers
        Healed?.Invoke(this, amountHealed);

        // Notification if restored to full vitality
        if (Health >= MaxHealth)
        {
            Console.WriteLine($"{Name} has returned to maximum vitality!");
        }
    }

    protected virtual void OnDeath()
    {
        // Clear lingering combat states
        IsBlocking = false;

        Console.WriteLine($"{Name} has fallen in battle!");

        // Trigger external subscribers
        Defeated?.Invoke(this);
    }

    public override string ToString() => $"{Name} (HP: {Health}/{MaxHealth}, Status: {(IsAlive ? "Alive" : "Defeated")})";
}