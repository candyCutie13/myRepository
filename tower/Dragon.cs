namespace tower;

class Dragon : Entity
{
    private const double FireBreatheChance = 0.3;
    private const int BurnDamagePerTurn = 5;
    private const int BurnDuration = 2;
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        
        if(!IsAlive) Console.WriteLine($"The {GetType().Name} died!");
    }
    public Dragon(int floor) : base(
        baseHealth: 325 + (floor - 1) * 30,
        baseDamage: 40 + (floor - 1) * 15,
        startFloor: floor,
        baseArmor: 50
    ) {}

    public override void Attack(Entity target)
    {
        if (!IsAlive) return;
        
        base.Attack(target);
        
    }

    

    public override string ToString() => $"Dragon (\t HP: {BaseHealth} \t DMG: {BaseDamage})";
}