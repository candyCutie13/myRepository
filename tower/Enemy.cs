namespace tower;

public class Enemy : IEntity
{
    public string Name { get; set; }
    public int Damage { get; set; }
    public int Health { get; set; }
    
    public void TakeDamage(int damage)
    {
        throw new NotImplementedException();
    }

    public void Attack(IEntity target)
    {
        throw new NotImplementedException();
    }

    public void CriticalChance()
    {
        throw new NotImplementedException();
    }
}