using System;
using System.Collections.Generic;

public class Game
{
    private Player player;
    private Map gameMap;
    private List<Item> inventory = new List<Item>();
    private Dictionary<string, int> resources = new Dictionary<string, int>();

    public Game()
    {
        player = new Player("Hero", 100, 10, 5, 3);
        gameMap = new Map();
        InitializeResources();
    }

    private void InitializeResources()
    {
        resources.Add("Food", 0);
        resources.Add("Water", 0);
        resources.Add("Wood", 0);
    }

    public void StartGame()
    {
        Console.WriteLine("欢迎来到冒险世界！");
        while (player.Health > 0)
        {
            DisplayStatus();
            string input = Console.ReadLine().ToLower();
            HandleInput(input);
        }
        Console.WriteLine("游戏结束。");
    }

    private void DisplayStatus()
    {
        Console.WriteLine($"健康: {player.Health}, 力量: {player.Strength}, 敏捷: {player.Agility}, 智力: {player.Intelligence}");
        Console.WriteLine("资源:");
        foreach (var resource in resources)
        {
            Console.WriteLine($"{resource.Key}: {resource.Value}");
        }
        Console.WriteLine("输入命令：");
    }

    private void HandleInput(string input)
    {
        switch (input)
        {
            case "move":
                MovePlayer();
                break;
            case "collect":
                CollectResources();
                break;
            case "fight":
                Fight();
                break;
            case "use":
                UseResource();
                break;
            case "quit":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("未知命令，请重新输入。");
                break;
        }
    }

    private void MovePlayer()
    {
        Console.WriteLine("选择方向 (north, south, east, west):");
        string direction = Console.ReadLine().ToLower();
        player.Move(direction);
        TriggerEvent();
    }

    private void TriggerEvent()
    {
        // 简单的事件触发逻辑
        Random random = new Random();
        int eventNumber = random.Next(1, 4);
        switch (eventNumber)
        {
            case 1:
                Console.WriteLine("你发现了一堆木材。");
                resources["Wood"] += 5;
                break;
            case 2:
                Console.WriteLine("你遇到了敌人！");
                Fight();
                break;
            case 3:
                Console.WriteLine("你找到了水源。");
                resources["Water"] += 10;
                break;
        }
    }

    private void CollectResources()
    {
        Console.WriteLine("选择要收集的资源 (Food, Water, Wood):");
        string resource = Console.ReadLine();
        if (resources.ContainsKey(resource))
        {
            resources[resource] += 5;
            Console.WriteLine($"收集了5个{resource}。");
        }
        else
        {
            Console.WriteLine("无效的资源。");
        }
    }

    private void Fight()
    {
        Enemy enemy = new Enemy("Goblin", 50, 5, 3);
        while (enemy.Health > 0 && player.Health > 0)
        {
            Console.WriteLine("选择行动 (attack, defend, skill):");
            string action = Console.ReadLine().ToLower();
            switch (action)
            {
                case "attack":
                    player.Attack(enemy);
                    break;
                case "defend":
                    player.Defend();
                    break;
                case "skill":
                    player.UseSkill(enemy);
                    break;
                default:
                    Console.WriteLine("未知行动。");
                    continue;
            }
            if (enemy.Health > 0)
            {
                enemy.Attack(player);
            }
        }
        if (player.Health > 0)
        {
            Console.WriteLine("你战胜了敌人！");
            player.GainExperience(10);
        }
        else
        {
            Console.WriteLine("你被击败了。");
        }
    }

    private void UseResource()
    {
        Console.WriteLine("选择要使用的资源 (Food, Water, Wood):");
        string resource = Console.ReadLine();
        if (resources.ContainsKey(resource) && resources[resource] > 0)
        {
            resources[resource]--;
            player.UseResource(resource);
        }
        else
        {
            Console.WriteLine("没有足够的资源。");
        }
    }
}

public class Player
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Strength { get; set; }
    public int Agility { get; set; }
    public int Intelligence { get; set; }
    public int Experience { get; set; }

    public Player(string name, int health, int strength, int agility, int intelligence)
    {
        Name = name;
        Health = health;
        Strength = strength;
        Agility = agility;
        Intelligence = intelligence;
        Experience = 0;
    }

    public void Move(string direction)
    {
        Console.WriteLine($"向{direction}移动。");
    }

    public void Attack(Enemy enemy)
    {
        Console.WriteLine($"你对{enemy.Name}造成了{Strength}点伤害。");
        enemy.Health -= Strength;
    }

    public void Defend()
    {
        Console.WriteLine("你进入了防御姿态。");
    }

    public void UseSkill(Enemy enemy)
    {
        Console.WriteLine($"你对{enemy.Name}使用了特殊技能。");
        enemy.Health -= (Strength + Intelligence);
    }

    public void GainExperience(int amount)
    {
        Experience += amount;
        LevelUp();
    }

    private void LevelUp()
    {
        if (Experience >= 100)
        {
            Console.WriteLine("等级提升！");
            Experience -= 100;
            Strength++;
            Agility++;
            Intelligence++;
        }
    }

    public void UseResource(string resource)
    {
        switch (resource)
        {
            case "Food":
                Health += 10;
                Console.WriteLine("恢复了10点健康。");
                break;
            case "Water":
                Health += 5;
                Console.WriteLine("恢复了5点健康。");
                break;
            case "Wood":
                Console.WriteLine("木材不能直接使用。");
                break;
        }
    }
}

public class Enemy
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int AttackPower { get; set; }
    public int Defense { get; set; }

    public Enemy(string name, int health, int attackPower, int defense)
    {
        Name = name;
        Health = health;
        AttackPower = attackPower;
        Defense = defense;
    }

    public void Attack(Player player)
    {
        Console.WriteLine($"{Name}对你造成了{AttackPower}点伤害。");
        player.Health -= AttackPower;
    }
}

public class Map
{
    // 地图相关的逻辑
}

public class Item
{
    // 物品相关的逻辑
}

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.StartGame();
    }
}
