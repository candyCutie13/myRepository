namespace tower;

class Program
{
    static void Main(string[] args)
    {
        var tower = new Tower(7);
        var hero = new Hero(1500, 40, 1);
        tower.AddHero(hero);
        tower.GenerateRandomEnemies();
        
        
    }
}