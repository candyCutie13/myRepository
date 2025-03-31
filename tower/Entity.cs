using System.Formats.Asn1;
using System.Reflection.Metadata;

// Не используешь - удаляй.
// YANGNI - You are not gonna need it. 

namespace tower;

public class Entity
{
    // все, что не private - С БОЛЬШОЙ БУКВЫ
    private int _baseHealth;
    private int _baseDamage;
    private int _currentFloor;
    public bool IsAlive => BaseHealth > 0;
    public int CurrentFloor
    {
        get => _currentFloor;
        set => _currentFloor = value >= 1 ? value : throw new ArgumentException("Floor can not be less than 1");
    }

    public int BaseHealth
    {
        get => _baseHealth;
        set => _baseHealth = value >= 1 ? value : throw new ArgumentException("Health can not be less than 1");
    }

    public int BaseDamage
    {
        get => _baseDamage;
        set => _baseDamage = value >= 1 ? value : throw new ArgumentException("Damage can not be less than 1");
    }
    
    public Entity(int startFloor, int baseHealth, int baseDamage)
    {
        ValidateFields(startFloor, baseHealth, baseDamage);
        
        BaseHealth = baseHealth;
        BaseDamage = baseDamage;
        CurrentFloor = startFloor;
    }

    private void ValidateFields(int startFloor, int baseHealth, int baseDamage)
    {
        if (startFloor < 0)
        {
            throw new ArgumentException("Start floor can not be less than 0");
        }

        if (baseHealth < 0)
        {
            throw new ArgumentException("Base health can not be less than 0");
        }

        if (baseDamage < 0)
        {
            throw new ArgumentException("Base damage can not be less than 0");
        }
    }
    
    public virtual void Attack(Entity target)
    {
        if (!IsAlive)
        {
            Console.WriteLine($"{GetType().Name} can not attack, he is dead!");
            return;
        }

        if (!target.IsAlive)
        {
            Console.WriteLine($"{GetType().Name} is already dead!");
            return;
        }

        Console.WriteLine($"{GetType().Name} attack {target.GetType().Name} and deal {BaseDamage} damage!");
        target.TakeDamage(BaseDamage);
    }
    public virtual void TakeDamage(int damage)
    {
        if (!IsAlive) return;

        _baseHealth -= damage;

        if (_baseHealth <= 0)
        {
            _baseHealth = 0;
        }
        else
        {
            Console.WriteLine($"{GetType().Name} takes {damage} damage. {_baseHealth} health is left");
        }
    }

    
}