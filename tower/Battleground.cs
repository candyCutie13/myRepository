namespace tower;

public class Battleground
{
    public List<Entity> Entities { get; }
    public int FloorNumber { get; }
    public List<Entity>[] EntitiesOnFloor;
    public Hero? Hero { get; private set; }
    
    public void StartBattle()
    {
        Console.WriteLine("Hero begins his ascent");
        Console.WriteLine("\n Press ENTER to start ascending");
        Console.ReadLine();
        while (hero.IsAlive && hero.CurrentFloor <= tower.Floors)
        {
            
            tower.PrintFloorStatus(hero.CurrentFloor);
            if (!hero.IsAlive) break;

            if (hero.CurrentFloor < tower.Floors)
            {
                Console.WriteLine("\n Press ENTER to go up");
                Console.ReadLine();
                
                tower.TryClimbFloor();
            }
            else
            {
                Console.WriteLine("\n The peak has been reached!");
                break;
            }
        }
        Console.WriteLine(hero.IsAlive
            ? $"Hero wins! Final stats:{hero.BaseHealth} HP, {hero.BaseDamage} DMG" 
            : "Hero is dead..."
        );
        void SimulateFloorBattle()
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
}