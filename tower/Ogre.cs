namespace tower;

public class Ogre : Entity
{
    // Ссылочные и значимые типы. bool - значимые тип.
    public Ogre(int floor) : base(
        baseHealth: 150 + (floor - 1) * 15,
        baseDamage: 15 + (floor - 1) * 5,
        startFloor: floor
    ) {}
    
    public const double StunChance = 0.2;
    public bool _didStunThisTurn;
    
    private readonly Random _random = new();
    
    public override void Attack(Entity target)
    {
        if (!IsAlive) return;
        base.Attack(target);

        if (!_didStunThisTurn && IsAlive && target.IsAlive && _random.NextDouble() < StunChance)
        {
            Console.WriteLine($"Ogre's powerful strike stuns {target.GetType().Name}!");
            _didStunThisTurn = true;
        }
        else
        {
            _didStunThisTurn = false;
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if(!IsAlive) Console.WriteLine("The Ogre died!");
    }
    
    public override string ToString() => $"Ogre (\t HP: {BaseHealth} \t DMG: {BaseDamage})";
}