using System;
using System.Collections.Generic;

public class Game
{
    private Player player;
    private List<Item> items;
    private List<NPC> npcs;
    private List<Puzzle> puzzles;
    private WeatherSystem weatherSystem;
    private SaveLoadSystem saveLoadSystem;
    private AchievementSystem achievementSystem;

    public Game()
    {
        player = new Player();
        items = new List<Item>();
        npcs = new List<NPC>();
        puzzles = new List<Puzzle>();
        weatherSystem = new WeatherSystem();
        saveLoadSystem = new SaveLoadSystem();
        achievementSystem = new AchievementSystem();
    }

    public void StartGame()
    {
        // Initialize game elements
        InitializeItems();
        InitializeNPCs();
        InitializePuzzles();

        // Main game loop
        while (true)
        {
            Console.WriteLine("1. Move");
            Console.WriteLine("2. Jump");
            Console.WriteLine("3. Climb");
            Console.WriteLine("4. Interact with item/NPC");
            Console.WriteLine("5. Solve puzzle");
            Console.WriteLine("6. Change weather");
            Console.WriteLine("7. Save game");
            Console.WriteLine("8. Load game");
            Console.WriteLine("9. Check achievements");
            Console.WriteLine("0. Exit");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    player.Move();
                    break;
                case 2:
                    player.Jump();
                    break;
                case 3:
                    player.Climb();
                    break;
                case 4:
                    InteractWithEnvironment();
                    break;
                case 5:
                    SolvePuzzle();
                    break;
                case 6:
                    weatherSystem.ChangeWeather();
                    break;
                case 7:
                    saveLoadSystem.SaveGame(player, items, npcs, puzzles, weatherSystem, achievementSystem);
                    break;
                case 8:
                    saveLoadSystem.LoadGame(out player, out items, out npcs, out puzzles, out weatherSystem, out achievementSystem);
                    break;
                case 9:
                    achievementSystem.CheckAchievements(player);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    private void InitializeItems()
    {
        items.Add(new Item { Name = "Key", Description = "A small rusty key." });
        items.Add(new Item { Name = "Map", Description = "A map of the area." });
    }

    private void InitializeNPCs()
    {
        npcs.Add(new NPC { Name = "Old Man", Dialogue = "Welcome traveler, what brings you here?" });
        npcs.Add(new NPC { Name = "Guard", Dialogue = "Halt! Who goes there?" });
    }

    private void InitializePuzzles()
    {
        puzzles.Add(new Puzzle { Type = "Physical", Description = "Move the crate to reach the high platform.", Solution = "Move crate" });
        puzzles.Add(new Puzzle { Type = "Logic", Description = "Arrange the numbers in ascending order.", Solution = "12345" });
    }

    private void InteractWithEnvironment()
    {
        Console.WriteLine("Choose an item or NPC to interact with:");
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Item: {items[i].Name}");
        }
        for (int i = 0; i < npcs.Count; i++)
        {
            Console.WriteLine($"{i + items.Count + 1}. NPC: {npcs[i].Name}");
        }

        int choice = int.Parse(Console.ReadLine()) - 1;
        if (choice < items.Count)
        {
            player.InteractWithItem(items[choice]);
        }
        else
        {
            player.InteractWithNPC(npcs[choice - items.Count]);
        }
    }

    private void SolvePuzzle()
    {
        Console.WriteLine("Choose a puzzle to solve:");
        for (int i = 0; i < puzzles.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Puzzle: {puzzles[i].Description}");
        }

        int choice = int.Parse(Console.ReadLine()) - 1;
        Console.WriteLine("Enter your solution:");
        string solution = Console.ReadLine();

        if (puzzles[choice].Solution == solution)
        {
            Console.WriteLine("Congratulations! You solved the puzzle.");
            achievementSystem.AwardAchievement("Puzzle Solver");
        }
        else
        {
            Console.WriteLine("Incorrect solution. Try again.");
        }
    }
}

public class Player
{
    public void Move()
    {
        Console.WriteLine("Player is moving.");
    }

    public void Jump()
    {
        Console.WriteLine("Player is jumping.");
    }

    public void Climb()
    {
        Console.WriteLine("Player is climbing.");
    }

    public void InteractWithItem(Item item)
    {
        Console.WriteLine($"Player interacts with item: {item.Name} - {item.Description}");
    }

    public void InteractWithNPC(NPC npc)
    {
        Console.WriteLine($"Player interacts with NPC: {npc.Name} - {npc.Dialogue}");
    }
}

public class Item
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class NPC
{
    public string Name { get; set; }
    public string Dialogue { get; set; }
}

public class Puzzle
{
    public string Type { get; set; }
    public string Description { get; set; }
    public string Solution { get; set; }
}

public class WeatherSystem
{
    private string currentWeather;

    public WeatherSystem()
    {
        currentWeather = "Sunny";
    }

    public void ChangeWeather()
    {
        Random random = new Random();
        string[] weathers = { "Sunny", "Rainy", "Foggy", "Snowy" };
        currentWeather = weathers[random.Next(weathers.Length)];
        Console.WriteLine($"Weather changed to {currentWeather}.");
    }
}

public class SaveLoadSystem
{
    public void SaveGame(Player player, List<Item> items, List<NPC> npcs, List<Puzzle> puzzles, WeatherSystem weatherSystem, AchievementSystem achievementSystem)
    {
        Console.WriteLine("Game saved.");
        // Implement saving logic here
    }

    public void LoadGame(out Player player, out List<Item> items, out List<NPC> npcs, out List<Puzzle> puzzles, out WeatherSystem weatherSystem, out AchievementSystem achievementSystem)
    {
        player = new Player();
        items = new List<Item>();
        npcs = new List<NPC>();
        puzzles = new List<Puzzle>();
        weatherSystem = new WeatherSystem();
        achievementSystem = new AchievementSystem();
        Console.WriteLine("Game loaded.");
        // Implement loading logic here
    }
}

public class AchievementSystem
{
    public void AwardAchievement(string achievement)
    {
        Console.WriteLine($"Achievement unlocked: {achievement}");
    }

    public void CheckAchievements(Player player)
    {
        Console.WriteLine("Checking achievements...");
        // Implement achievement checking logic here
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.StartGame();
    }
}
