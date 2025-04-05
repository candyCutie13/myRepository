namespace tower;

public class Ogre : Entity
{
    public const double StunChance = 0.2;
    public bool _didStunThisTurn = false;

    public override void Attack(Entity target)
    {
        if (!IsAlive) return;
        base.Attack(target);

        if (!_didStunThisTurn && IsAlive && target.IsAlive && new Random().NextDouble() < StunChance)
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
        
        if(!IsAlive) Console.WriteLine($"The {GetType().Name} died!");
    }

    public Ogre(int floor) : base(
        baseHealth: 150 + (floor - 1) * 15,
        baseDamage: 15 + (floor - 1) * 5,
        startFloor: floor,
        baseArmor: 5
        ) {}
    
    public override string ToString() => $"Ogre (\t HP: {BaseHealth} \t DMG: {BaseDamage})";
}