namespace tower;

public class Murlock : IEntity
{
    public Murlock(string name, int damage, int health)
    {
        Name = name;
        Damage = damage;
        Health = health;
    }
    
    public string Name { get; set; }
    public int Damage { get; set; }
    public int Health { get; set; }
    
    public void TakeDamage(int damage)
    {
        if (Health <= 0)
        {
            return;
        }
        
        Health -= damage;
        
    }

    public void Attack(IEntity target)
    {
        throw new NotImplementedException();
    }
}