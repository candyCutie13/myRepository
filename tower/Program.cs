namespace tower;

//Сделать фишку с артефактами в конце этажей
//Переделать параметр брони таким образом, чтобы он показывал броню сущ-ва в единицах, дмг резал в процентах, и не так криво это выводил
//Отредачить и разнообразить вывод при появлении существ
//Сделать чтобы у героя было имя
//Сделать нормальный ввод переменных в методе Main
//Сделать рандом кол-во этажей в башне(от 1 до 9)
//Добавить пиксель арт

class Program
{
    static void Main(string[] args)
    {
        var tower = new Tower(5);
        var hero = new Hero(1500, 40, 1);
        tower.GenerateRandomEnemies();

        var battleground = new Battleground(hero, tower);
        
        battleground.StartBattle();
    }
}