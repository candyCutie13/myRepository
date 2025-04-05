namespace tower;

// Запушу линию метро и сломаю им бараки (обамы)
public class Battleground
{
    private readonly Tower _tower;
    private readonly Hero _hero;

    public Battleground(Hero hero, Tower tower)
    {
        _hero = hero;
        _tower = tower;
    }


    // ctrl + r + r = поменять везде
    public void StartBattle()
    {
        Console.Write("Hero begins his ascent\nPress ENTER to start ascending");
        
        Console.ReadLine();

        while (_hero.IsAlive && _hero.CurrentFloor <= _tower.Floors)
        {
            _tower.PrintFloorStatus(_hero.CurrentFloor);
            SimulateFloorBattle();
            if (!_hero.IsAlive) break;

            if (_hero.CurrentFloor < _tower.Floors)
            {
                Console.WriteLine("\n Press ENTER to go up");
                Console.ReadLine();

                TryClimbFloor();
            }
            else
            {
                Console.WriteLine("\n The peak has been reached!");
                break;
            }
        }

        Console.WriteLine(_hero.IsAlive
            ? $"Hero wins! Final stats:{_hero.BaseHealth} HP, {_hero.BaseDamage} DMG"
            : "Hero is dead..."
        );
    }

    private void TryClimbFloor()
    {
        if (_hero.CurrentFloor >= _tower.Floors)
        {
            Console.WriteLine("The hero can't get any higher");
            return;
        }

        _hero.LevelUp();
        _tower.AddEntity(_hero, _hero.CurrentFloor);
    }

    public void SimulateFloorBattle()
    {
        if (!_hero.IsAlive)
        {
            Console.WriteLine($"Hero can not fight!");
            return;
        }

        var floor = _hero.CurrentFloor;
        var aliveEnemies = _tower.EntitiesOnFloor[floor].Where(c => c != _hero && c.IsAlive).ToList();
        var aliveGoblins = aliveEnemies.OfType<Goblin>().ToList();
        var aliveOtherEnemies = aliveEnemies.Where(e => !(e is Goblin)).ToList();
        var isHeroStunned = false;

        if (aliveEnemies.Count == 0)
        {
            Console.WriteLine("There is no enemies on the floor!");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"\t Battle on floor {floor} \t");
        Console.WriteLine($"Hero vs {aliveEnemies.Count} enemies");
        Console.WriteLine();
        while (aliveEnemies.Any(e => e.IsAlive) && _hero.IsAlive)
        {
            //Thread.Sleep(1500);
            foreach (var goblin in aliveGoblins.Where(g => g.IsAlive).ToList())
            {
                goblin.Attack(_hero);
                if (!_hero.IsAlive) break;
            }

            if (!_hero.IsAlive) break;
            if (!isHeroStunned)
            {
                var enemies = aliveEnemies.Where(e => e.IsAlive).ToList();
                if (enemies.Any())
                {
                    var target = enemies[new Random().Next(enemies.Count)];
                    _hero.Attack(target);
                }
                else
                {
                    Console.WriteLine("Hero is stunned and misses a turn!");
                    isHeroStunned = false;
                }
            }

            foreach (var enemy in aliveOtherEnemies.Where(e => e.IsAlive).ToList())
            {
                enemy.Attack(_hero);
                if (enemy is Ogre ogre && ogre.IsAlive) isHeroStunned = ogre._didStunThisTurn;

                if (!_hero.IsAlive) break;
            }
        }

        if (!_hero.IsAlive)
        {
            Console.WriteLine("Hero is dead...");
        }
        else
        {
            Console.WriteLine($"Hero defeated all the enemies on the floor {floor}!");
            Console.WriteLine($"{_hero.BaseHealth} health left");
        }

        _tower.EntitiesOnFloor[floor].RemoveAll(c => c.BaseHealth <= 0);
    }
}