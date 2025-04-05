namespace tower;

public class Tower
{
    public List<Entity>[] EntitiesOnFloor { get; set; }
    private readonly Random _random = new();
    private const int MaxFloors = 9;

    public int Floors { get; set; }

    public Tower(int floors)
    {
        if (floors <= 0 || floors > MaxFloors) throw new ArgumentException($"Invalid number of floors: {floors}");
        Floors = floors;

        EntitiesOnFloor = new List<Entity>[Floors + 1];

        for (var i = 0; i <= Floors; i++) EntitiesOnFloor[i] = [];
    }

    private int GetRandomCount(params (int count, double chance)[] options)
    {
        var total = options.Sum(o => o.chance);
        var roll = new Random().NextDouble() * total;
        double accumulating = 0;

        foreach (var (count, chance) in options)
        {
            accumulating += chance;
            if (roll < accumulating)
                return count;
        }

        return options.Last().count;
    }

    public void AddEntity(Entity entity, int floor)
    {
        if (floor < 1 || floor > Floors)
            throw new ArgumentException("Invalid floor");
        if (entity == null)
            throw new ArgumentException(nameof(entity));
        if (entity.BaseHealth < 1 || entity.BaseDamage < 1)
            throw new ArgumentException("Entity stats can not be less than 1");

        entity.CurrentFloor = floor;
        EntitiesOnFloor[floor].Add(entity);
    }

    public void PrintFloorStatus(int floor)
    {
        Console.WriteLine($"\t Floor ({floor})\t");
        foreach (var entity in EntitiesOnFloor[floor])
        {
            var status = entity.BaseHealth <= 0 ? "Dead" : $"{entity.BaseHealth} HP";
            Console.WriteLine($"{entity.GetType().Name}: {status}, {entity.BaseDamage} DMG, {entity.BaseArmor}% Armor");
        }
    }

    public void GenerateRandomEnemies()
    {
        for (var floor = 1; floor <= Floors; floor++)
        {
            if (floor <= 2)
            {
                var goblinCount = GetRandomCount((2, 0.6), (3, 0.3), (4, 0.1));
                for (var i = 0; i < goblinCount; i++) AddEntity(new Goblin(floor), floor);
                Console.WriteLine($"A group of {goblinCount} goblins appeared on floor {floor}");
                continue;
            }

            if (floor < Floors)
            {
                var enemies = new List<Entity>();

                Entity mainEnemy = _random.NextDouble() < 0.8 ? new Ogre(floor) : new Goblin(floor);

                enemies.Add(mainEnemy);

                var extraEnemies = GetRandomCount(
                    (1, 0.5),
                    (2, 0.3),
                    (3, 0.2)
                );

                for (var i = 0; i < extraEnemies; i++)
                    enemies.Add(_random.NextDouble() < 0.7 ? new Goblin(floor) : new Ogre(floor));

                foreach (var enemy in enemies) AddEntity(enemy, floor);

                var ogres = enemies.Count(e => e is Ogre);
                var goblins = enemies.Count(e => e is Goblin);

                var message = $"On the floor {floor} appears a group of enemies:";
                if (ogres > 0) message += $"{ogres} ogre,";
                if (goblins > 0) message += $"{goblins} goblin";

                Console.WriteLine(message.TrimEnd(','));
            }

            if (floor != Floors || Floors < 5) continue;

            AddEntity(new Dragon(floor), floor);
            Console.WriteLine($"Dragon is waiting for you on the last floor!");
        }
    }
}