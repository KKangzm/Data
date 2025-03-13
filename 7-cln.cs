using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class Game
    {
        private Player player;
        private List<Scene> scenes;
        private Scene currentScene;
        private List<Item> inventory;
        private List<NPC> npcs;
        private List<Puzzle> puzzles;
        private SaveLoadManager saveLoadManager;
        private UIManager uiManager;

        public Game()
        {
            player = new Player();
            scenes = new List<Scene>();
            inventory = new List<Item>();
            npcs = new List<NPC>();
            puzzles = new List<Puzzle>();
            saveLoadManager = new SaveLoadManager();
            uiManager = new UIManager();

            // Initialize scenes, NPCs, and puzzles
            InitializeGameWorld();
        }

        private void InitializeGameWorld()
        {
            // Example of adding scenes
            var startScene = new Scene("Start", "The starting point of the game.");
            scenes.Add(startScene);
            currentScene = startScene;

            // Example of adding items to the scene
            var key = new Item("Key", "A small brass key.");
            startScene.AddItem(key);

            // Example of adding NPCs
            var npc = new NPC("Guard", "A vigilant guard.");
            npcs.Add(npc);
            startScene.AddNPC(npc);

            // Example of adding puzzles
            var puzzle = new Puzzle("Unlock Door", "Use the key to unlock the door.", key);
            puzzles.Add(puzzle);
            startScene.AddPuzzle(puzzle);
        }

        public void Start()
        {
            uiManager.ShowMainMenu();
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "start")
                {
                    uiManager.ShowInGameUI();
                    break;
                }
                else if (input == "exit")
                {
                    Environment.Exit(0);
                }
            }

            MainGameLoop();
        }

        private void MainGameLoop()
        {
            while (true)
            {
                Console.WriteLine($"You are in {currentScene.Name}: {currentScene.Description}");
                Console.WriteLine("What do you want to do? (move, interact, talk, solve, save, load, exit)");
                string input = Console.ReadLine().ToLower();

                switch (input)
                {
                    case "move":
                        MovePlayer();
                        break;
                    case "interact":
                        InteractWithItems();
                        break;
                    case "talk":
                        TalkToNPCs();
                        break;
                    case "solve":
                        SolvePuzzles();
                        break;
                    case "save":
                        saveLoadManager.SaveGame(this);
                        break;
                    case "load":
                        Game loadedGame = saveLoadManager.LoadGame();
                        if (loadedGame != null)
                        {
                            this.player = loadedGame.player;
                            this.currentScene = loadedGame.currentScene;
                            this.inventory = loadedGame.inventory;
                            this.npcs = loadedGame.npcs;
                            this.puzzles = loadedGame.puzzles;
                        }
                        break;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }
        }

        private void MovePlayer()
        {
            Console.WriteLine("Where do you want to move? (north, south, east, west)");
            string direction = Console.ReadLine().ToLower();
            Scene nextScene = currentScene.GetConnectedScene(direction);

            if (nextScene != null)
            {
                currentScene = nextScene;
                Console.WriteLine($"Moved to {nextScene.Name}.");
            }
            else
            {
                Console.WriteLine("You can't go that way.");
            }
        }

        private void InteractWithItems()
        {
            Console.WriteLine("Which item do you want to interact with?");
            foreach (var item in currentScene.Items)
            {
                Console.WriteLine(item.Name);
            }

            string itemName = Console.ReadLine();
            Item item = currentScene.GetItemByName(itemName);

            if (item != null)
            {
                Console.WriteLine($"Interacting with {item.Name}: {item.Description}");
                if (item.CanBePickedUp)
                {
                    inventory.Add(item);
                    currentScene.RemoveItem(item);
                    Console.WriteLine($"{item.Name} picked up.");
                }
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        private void TalkToNPCs()
        {
            Console.WriteLine("Which NPC do you want to talk to?");
            foreach (var npc in currentScene.NPCs)
            {
                Console.WriteLine(npc.Name);
            }

            string npcName = Console.ReadLine();
            NPC npc = currentScene.GetNPCByName(npcName);

            if (npc != null)
            {
                Console.WriteLine(npc.Talk());
            }
            else
            {
                Console.WriteLine("NPC not found.");
            }
        }

        private void SolvePuzzles()
        {
            Console.WriteLine("Which puzzle do you want to solve?");
            foreach (var puzzle in currentScene.Puzzles)
            {
                Console.WriteLine(puzzle.Name);
            }

            string puzzleName = Console.ReadLine();
            Puzzle puzzle = currentScene.GetPuzzleByName(puzzleName);

            if (puzzle != null)
            {
                Console.WriteLine(puzzle.Solve(inventory));
                if (puzzle.IsSolved)
                {
                    currentScene.RemovePuzzle(puzzle);
                }
            }
            else
            {
                Console.WriteLine("Puzzle not found.");
            }
        }
    }

    public class Player
    {
        // Player properties and methods
    }

    public class Scene
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Scene> ConnectedScenes { get; set; }
        public List<Item> Items { get; set; }
        public List<NPC> NPCs { get; set; }
        public List<Puzzle> Puzzles { get; set; }

        public Scene(string name, string description)
        {
            Name = name;
            Description = description;
            ConnectedScenes = new List<Scene>();
            Items = new List<Item>();
            NPCs = new List<NPC>();
            Puzzles = new List<Puzzle>();
        }

        public void AddConnectedScene(Scene scene, string direction)
        {
            ConnectedScenes.Add(scene);
        }

        public Scene GetConnectedScene(string direction)
        {
            return ConnectedScenes.Find(s => s.Name == direction);
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public Item GetItemByName(string name)
        {
            return Items.Find(i => i.Name == name);
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);
        }

        public void AddNPC(NPC npc)
        {
            NPCs.Add(npc);
        }

        public NPC GetNPCByName(string name)
        {
            return NPCs.Find(n => n.Name == name);
        }

        public void RemoveNPC(NPC npc)
        {
            NPCs.Remove(npc);
        }

        public void AddPuzzle(Puzzle puzzle)
        {
            Puzzles.Add(puzzle);
        }

        public Puzzle GetPuzzleByName(string name)
        {
            return Puzzles.Find(p => p.Name == name);
        }

        public void RemovePuzzle(Puzzle puzzle)
        {
            Puzzles.Remove(puzzle);
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool CanBePickedUp { get; set; }

        public Item(string name, string description, bool canBePickedUp = true)
        {
            Name = name;
            Description = description;
            CanBePickedUp = canBePickedUp;
        }
    }

    public class NPC
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Dialogue { get; set; }

        public NPC(string name, string description, string dialogue = "Hello, traveler.")
        {
            Name = name;
            Description = description;
            Dialogue = dialogue;
        }

        public string Talk()
        {
            return Dialogue;
        }
    }

    public class Puzzle
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Item RequiredItem { get; set; }
        public bool IsSolved { get; set; }

        public Puzzle(string name, string description, Item requiredItem)
        {
            Name = name;
            Description = description;
            RequiredItem = requiredItem;
            IsSolved = false;
        }

        public string Solve(List<Item> inventory)
        {
            if (inventory.Contains(RequiredItem))
            {
                IsSolved = true;
                return $"You solved the puzzle using the {RequiredItem.Name}.";
            }
            else
            {
                return "You don't have the required item to solve this puzzle.";
            }
        }
    }

    public class SaveLoadManager
    {
        public void SaveGame(Game game)
        {
            // Save game state to file
            Console.WriteLine("Game saved.");
        }

        public Game LoadGame()
        {
            // Load game state from file
            Console.WriteLine("Game loaded.");
            return new Game(); // Placeholder for actual loaded game
        }
    }

    public class UIManager
    {
        public void ShowMainMenu()
        {
            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. Start Game");
            Console.WriteLine("2. Exit");
            Console.WriteLine("Enter 'start' to begin or 'exit' to quit.");
        }

        public void ShowInGameUI()
        {
            Console.WriteLine("In-Game Menu:");
            Console.WriteLine("Move, Interact, Talk, Solve, Save, Load, Exit");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
}
