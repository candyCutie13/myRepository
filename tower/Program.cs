namespace tower;

class Program
{
    static void Main(string[] args)
    {
        var tower = new Tower(7);
        var hero = new Hero(1500, 40, 1);
        tower.GenerateRandomEnemies();

        var battleground = new Battleground(hero, tower);
        
        battleground.StartBattle();
    }
}