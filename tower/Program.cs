using System.Formats.Asn1;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
//Сделать фишку с броней и артефактами в конце этажей
//Отредачить и разнообразить вывод при появлении существ
//Сделать чтобы у героя было имя
//Сделать нормальный ввод переменных в методе Main
//Сделать рандом кол-во этажей в башне(от 1 до 9)
//Откорректировать двойной удар у гоблинов

namespace tower;

class Program
{
    static void Main(string[] args)
    {
        var tower = new Tower(7);
        var hero = new Hero(1500, 40, 1);
        tower.AddHero(hero);
        tower.GenerateRandomEnemies();
        
        Console.WriteLine("Hero begins his ascent");
        Console.WriteLine("\n Press ENTER to start ascending");
        Console.ReadLine();
        
        while (hero.IsAlive && hero.CurrentFloor <= tower.Floors)
        {
            
            tower.PrintFloorStatus(hero.CurrentFloor);
            
            tower.SimulateFloorBattle();

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
    }
}