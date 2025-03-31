namespace tower;

class Dragon : Entity
{
    public const double DamageReduction = 0.4;

    public override void TakeDamage(int damage)
    {
        int reducedDamage = (int)(damage * (1 - DamageReduction));
        Console.WriteLine($"The dragon takes {DamageReduction * 100}% less damage due to its scales!({damage} -> {reducedDamage})");
        
        base.TakeDamage(reducedDamage);
        
        if(!IsAlive) Console.WriteLine("The Dragon died!");
    }
    public Dragon(int floor) : base(
        baseHealth: 325 + (floor - 1) * 30,
        baseDamage: 40 + (floor - 1) * 15,
        startFloor: floor
    ) {}
    
    public override string ToString() => $"Dragon (\t HP: {BaseHealth} \t DMG: {BaseDamage})";
}