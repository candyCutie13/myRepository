using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace tower;

class Tower
{
    public int _floors;
    public List<Entity>[] EntitiesOnFloor;
    public Hero? Hero { get; private set; }
    private Random random = new Random();
    private const int MaxFloors = 9;

    public int Floors
    {
        get => _floors;
        set => _floors = value >= 1 && value <= MaxFloors
            ? value
            : throw new ArgumentException($"A Tower can have from 1 to {MaxFloors} floors");
    }

    public Tower(int _floors)
    {
        Floors = _floors;
        EntitiesOnFloor = new List<Entity>[_floors + 1];
        for (int i = 0; i <= _floors; i++)
        {
            EntitiesOnFloor[i] = new List<Entity>();
        }
    }

    public int GetRandomCount(params (int count, double chance)[] options)
    {
        double total = options.Sum(o => o.chance);
        double roll = new Random().NextDouble() * total;
        double accumulating = 0;

        foreach (var (count, chance) in options)
        {
            accumulating += chance;
            if (roll < accumulating)
                return count;
        }

        return options.Last().count;
    }
    public void AddHero(Hero hero, int startFloor = 1)
    {
        Hero = hero;
        AddEntity(hero, startFloor);
    }

    public bool TryClimbFloor()
    {
        if (Hero == null || Hero.CurrentFloor >= Floors)
        {
            Console.WriteLine("The hero can't get any higher");
            return false;
        }
        
        Hero.LevelUp();
        AddEntity(Hero, Hero.CurrentFloor);
        return true;
    }
    public void AddEntity(Entity entity, int floor)
    {
        if (floor < 1 || floor > Floors)
            throw new ArgumentException("Invalid floor");
        if (entity == null)
            throw new ArgumentException(nameof(entity));
        if(entity.BaseHealth < 1 || entity.BaseDamage < 1)
            throw new ArgumentException("Entity stats can not be less than 1");

        entity.CurrentFloor = floor;
        EntitiesOnFloor[floor].Add(entity);
    }

    public void PrintFloorStatus(int floor)
    {
        Console.WriteLine($"\t Floor ({floor})\t");
        foreach (var entity in EntitiesOnFloor[floor])
        {
            string status = entity.BaseHealth <= 0 ? "Dead" : $"{entity.BaseHealth} HP";
            Console.WriteLine($"{entity.GetType().Name}: {status}, {entity.BaseDamage} DMG");
        }
    }

    public void GenerateRandomEnemies()
    {
        for (int floor = 1; floor <= Floors; floor++)
        {
                if (Floors <= 2)
                {
                    if (floor == 1)
                    {
                        AddEntity(new Goblin(floor), floor);
                        Console.WriteLine($"Goblin settled on the {floor} floor!");
                    }
                    else if (floor == 2)
                    {
                        AddEntity(new Ogre(floor), floor);
                        Console.WriteLine($"An Ogre settled on the {floor} floor!");
                    }
                    continue;
                }
                if (Floors <= 4)
            {
                if (floor <= 2)
                {
                    AddEntity(new Goblin(floor), floor);
                    Console.WriteLine($"A Goblin has appeared on floor {floor}!");
                }
                else if (floor <= 4)
                {
                    AddEntity(new Ogre(floor), floor);
                    Console.WriteLine($"An Ogre has appeared on floor {floor}!");
                }       
                continue;
            }

            if (Floors > 4)
            {
                if (floor <= 2)
                {
                    int goblinCount = GetRandomCount((2, 0.6), (3, 0.3), (4, 0.1));
                    for (int i = 0; i < goblinCount; i++)
                    {
                        AddEntity(new Goblin(floor), floor);
                    }
                    Console.WriteLine($"A group of {goblinCount} goblins appeared on floor {floor}");
                    continue;
                }
                if (floor >= 3 && floor != Floors)
                {
                    var enemies = new List<Entity>();
                    Entity mainEnemy = random.NextDouble() < 0.8 ? new Ogre(floor) : new Goblin(floor);
                    enemies.Add(mainEnemy);
                    int extraEnemies = GetRandomCount(
                        (1, 0.5),
                        (2, 0.3),
                        (3, 0.2)
                    );
                    for (int i = 0; i < extraEnemies; i++)
                    {
                        enemies.Add(random.NextDouble() < 0.7 ? new Goblin(floor) : new Ogre(floor));
                    }

                    foreach (var enemy in enemies)
                    {
                        AddEntity(enemy, floor);
                    }

                    int ogres = enemies.Count(e => e is Ogre);
                    int goblins = enemies.Count(e => e is Goblin);

                    string message = $"On the floor {floor} appears a group of enemies:";
                    if (ogres > 0) message += $"{ogres} ogre,";
                    if (goblins > 0) message += $"{goblins} goblin";
                    
                    Console.WriteLine(message.TrimEnd(','));
                }
                if (floor == Floors && Floors >= 5)
                {
                    AddEntity(new Dragon(floor), floor);
                    Console.WriteLine($"Dragon is waiting for you on the last floor!");
                }
            }
        }
    } 
    public void SimulateFloorBattle()
    {
        if (Hero == null || !Hero.IsAlive)
        {
            Console.WriteLine($"Hero can not fight!");
            return;
        }
        
        int floor = Hero.CurrentFloor;
        var aliveEnemies = EntitiesOnFloor[floor].Where(c => c != Hero && c.IsAlive).ToList();
        var aliveGoblins = aliveEnemies.OfType<Goblin>().ToList();
        var aliveOtherEnemies = aliveEnemies.Where(e => !(e is Goblin)).ToList();
        bool isHeroStunned = false;

        if (aliveEnemies.Count == 0)
        {
            Console.WriteLine("There is no enemies on the floor!");
            return;
        }
        
        Console.WriteLine($"\t Battle on floor {floor} \t");
        Console.WriteLine($"Hero ({Hero.BaseHealth} HP) vs {aliveEnemies.Count} enemies");
        while (aliveEnemies.Any(e => e.IsAlive) && Hero.IsAlive)
        {
            foreach (var goblin in aliveGoblins.Where(g => g.IsAlive).ToList())
            {
                goblin.Attack(Hero);
                if (!Hero.IsAlive) break;
            }
            if (!Hero.IsAlive) break;
            if (!isHeroStunned)
            {
                var enemies = aliveEnemies.Where(e => e.IsAlive).ToList();
                if (enemies.Any())
                {
                    var target = enemies[new Random().Next(enemies.Count)];
                    Hero.Attack(target);
                }
                else
                {
                    Console.WriteLine("Hero is stunned and misses a turn!");
                    isHeroStunned = false;
                }
            }

            foreach (var enemy in aliveOtherEnemies.Where(e => e.IsAlive).ToList())
            {
                enemy.Attack(Hero);
                if (enemy is Ogre ogre && ogre.IsAlive)
                {
                    isHeroStunned = ogre._didStunThisTurn;
                }
                if (!Hero.IsAlive) break;
            }
        }

        if (!Hero.IsAlive) Console.WriteLine("Hero is dead...");
        else
        {
            Console.WriteLine($"Hero defeated all the enemies on the floor {floor}!");
            Console.WriteLine($"{Hero.BaseHealth} health left");
        }
        EntitiesOnFloor[floor].RemoveAll(c => c.BaseHealth <= 0);
    }
}