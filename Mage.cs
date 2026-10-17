class Mage : Character
{
    public int Mana { get; private set; }
    public int MaxMana { get; private set; }

    public Mage(string name, int maxHealth) : base(name, maxHealth)
    {
        MaxMana = 100;
        Mana = MaxMana;
    }

    public void CastSpell(Character target)
    {
        if (Mana >= 10)
        {
            Mana -= 10;
            target.Damage(20);
            Console.WriteLine($"{Name} casts a spell on {target.Name}!");
        }
    }   
}