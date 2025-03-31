namespace tower;

class Goblin : Entity
{
    public Goblin(int floor) : base(
        baseHealth: 50 + (floor - 1) * 7,
        baseDamage: 10 + (floor - 1) * 3,
        startFloor: floor
    ) {}
    
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if(!IsAlive) Console.WriteLine("The Goblin died!");
    }
    
    public override string ToString() => $"Goblin (\t HP: {BaseHealth} \t DMG: {BaseDamage})";
}