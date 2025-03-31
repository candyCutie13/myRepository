namespace tower;

public interface IEntity
{
    string Name { get; set; }
    
    int Damage { get; set; }
    
    int Health { get; set; }
    
    void TakeDamage(int damage);

    void Attack(IEntity target);
}