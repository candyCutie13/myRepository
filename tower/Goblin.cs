namespace tower;

class Goblin : Entity
{
    private const double DoubleAttackChance = 0.4;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        
        if(!IsAlive) Console.WriteLine($"The {GetType().Name} died!");
    }
    
    public Goblin(int floor) : base(
        baseHealth: 50 + (floor - 1) * 7,
        baseDamage: 10 + (floor - 1) * 3,
        startFloor: floor,
        baseArmor: 2
    ) {}
    
    public override string ToString() => $"Goblin (\t HP: {BaseHealth} \t DMG: {BaseDamage})";
}