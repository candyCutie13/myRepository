namespace tower;

public class Hero : Entity
{
    private const double CritChance = 0.25;
    private const double CritMultiplier = 1.75;
    public Hero(int baseHealth, int baseDamage, int startFloor) : base(startFloor, baseHealth, baseDamage){}

    public override void Attack(Entity target)
    {
        if (!IsAlive) return;

        var damage = BaseDamage;
        var isCrit = new Random().NextDouble() < CritChance;

        if (isCrit)
        {
            damage = (int)(damage * CritMultiplier);
            Console.WriteLine($"{GetType().Name} will deal a critical hit!");
        }
        Console.WriteLine($"{GetType().Name} attack {target.GetType().Name} and deal {damage} damage!");
        target.TakeDamage(damage);
    }
    public void LevelUp()
    {
        BaseHealth = (int)(BaseHealth * 1.3);
        BaseDamage = (int)(BaseDamage * 1.2);
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



