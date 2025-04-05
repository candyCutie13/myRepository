namespace tower;

public class Hero : Entity
{
    private const double CritChance = 0.25;
    private const double CritMultiplier = 1.75;
    public Hero(int baseHealth, int baseDamage, int startFloor) : base(startFloor, baseHealth, baseDamage, baseArmor: 10){}

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        
        if(!IsAlive) Console.WriteLine($"The {GetType().Name} died!");
    }
    public override void Attack(Entity target)
    {
        if (!IsAlive) return;

        int damage = BaseDamage;
        bool IsCrit = new Random().NextDouble() < CritChance;

        if (IsCrit)
        {
            damage = (int)(damage * CritMultiplier);
            Console.WriteLine($"{GetType().Name} will deal a critical hit!");
        }
        Console.WriteLine($"{GetType().Name} attack {target.GetType().Name} and deal {damage} damage!");
        target.TakeDamage(damage);
    }
    public int BaseHealth
    {
        get => base.BaseHealth;
        set => base.BaseHealth = value >= 0 ? value : throw new ArgumentException("Health can not be less than 1");
    }
    public int BaseDamage
    {
        get => base._baseDamage;
        set => base.BaseDamage = value >= 0 ? value : throw new ArgumentException("Damage can not be less than 1");
    } 
    public void LevelUp()
    {
        BaseHealth = (int)(_baseHealth * 1.3);
        BaseDamage = (int)(_baseDamage * 1.2);
        CurrentFloor++;
        Console.WriteLine($"Hero went up to the floor {CurrentFloor}! \n New stats: {BaseHealth} HP | {BaseDamage} DMG");
    }

    public void AttackRandomEnemy(List<Entity> enemies)
    {
        if (!IsAlive || enemies.Count == 0) return;
        var target = enemies[new Random().Next(enemies.Count)];
        Attack(target);
    }

    public override string ToString() => $"Hero (Floor: {CurrentFloor} \t HP: {BaseHealth} \t DMG: {BaseDamage})";
}



