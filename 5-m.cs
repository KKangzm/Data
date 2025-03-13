using System;
using System.Collections.Generic;

namespace MysteriousIslandLostExploration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("欢迎来到《神秘之岛：失落的探险》");

            // 初始化游戏环境
            Game game = new Game();
            game.Start();

            // 游戏主循环
            while (game.IsRunning)
            {
                game.Update();
            }

            Console.WriteLine("感谢您的游玩！");
        }
    }

    public class Game
    {
        public bool IsRunning { get; private set; } = true;
        private Player player;
        private List<NPC> npcs;
        private List<Item> items;
        private List<Puzzle> puzzles;
        private List<Location> locations;

        public void Start()
        {
            InitializeGame();
            DisplayIntroduction();
        }

        private void InitializeGame()
        {
            player = new Player("探险者", "你是一名勇敢的探险者，来到神秘之岛寻找失踪的探险队。");
            npcs = new List<NPC>
            {
                new NPC("日记本", "一本破旧的日记本，记录了探险队的日常活动。"),
                new NPC("地图", "一张古老的地图，标记着通往遗迹的路径。")
            };
            items = new List<Item>
            {
                new Item("钥匙", "一把古老的铜钥匙，看起来可以开启某些锁。"),
                new Item("火把", "一支可以照明的火把，用于黑暗的地方。")
            };
            puzzles = new List<Puzzle>
            {
                new Puzzle("入口谜题", "你需要找到正确的符号来打开遗迹的大门。"),
                new Puzzle("机关陷阱", "小心地避开陷阱，使用物品解除机关。")
            };
            locations = new List<Location>
            {
                new Location("海滩", "你降落在一个荒凉的海滩上，四周是茂密的丛林。"),
                new Location("遗迹入口", "这是一个古老的石门，上面刻有复杂的图案。")
            };
        }

        private void DisplayIntroduction()
        {
            Console.WriteLine("你是一名勇敢的探险者，来到了传说中的神秘之岛。");
            Console.WriteLine("这里曾经是一个先进文明的栖息地，但因一场灾难而消失。");
            Console.WriteLine("你的任务是找到失踪的探险队，揭开岛屿的秘密，并安全返回。");
            Console.WriteLine("按任意键开始游戏...");
            Console.ReadKey();
        }

        public void Update()
        {
            Console.Clear();
            Console.WriteLine($"当前位置：{player.CurrentLocation.Name}");
            Console.WriteLine(player.CurrentLocation.Description);
            Console.WriteLine("\n可选操作：");
            Console.WriteLine("1. 探索周围环境");
            Console.WriteLine("2. 查看背包");
            Console.WriteLine("3. 与NPC对话");
            Console.WriteLine("4. 解决谜题");
            Console.WriteLine("5. 查看地图");
            Console.WriteLine("6. 退出游戏");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ExploreEnvironment();
                    break;
                case "2":
                    ViewInventory();
                    break;
                case "3":
                    TalkToNPC();
                    break;
                case "4":
                    SolvePuzzle();
                    break;
                case "5":
                    ViewMap();
                    break;
                case "6":
                    IsRunning = false;
                    break;
                default:
                    Console.WriteLine("无效输入，请重新选择。");
                    break;
            }
        }

        private void ExploreEnvironment()
        {
            Console.WriteLine("你在周围环境中发现了以下物品：");
            foreach (var item in items)
            {
                Console.WriteLine($"- {item.Name}: {item.Description}");
            }
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
        }

        private void ViewInventory()
        {
            Console.WriteLine("你的背包里有以下物品：");
            foreach (var item in player.Inventory)
            {
                Console.WriteLine($"- {item.Name}: {item.Description}");
            }
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
        }

        private void TalkToNPC()
        {
            Console.WriteLine("你可以与以下NPC对话：");
            foreach (var npc in npcs)
            {
                Console.WriteLine($"- {npc.Name}: {npc.Description}");
            }
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
        }

        private void SolvePuzzle()
        {
            Console.WriteLine("当前需要解决的谜题：");
            foreach (var puzzle in puzzles)
            {
                Console.WriteLine($"- {puzzle.Name}: {puzzle.Description}");
            }
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
        }

        private void ViewMap()
        {
            Console.WriteLine("你的地图显示了以下地点：");
            foreach (var location in locations)
            {
                Console.WriteLine($"- {location.Name}: {location.Description}");
            }
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
        }
    }

    public class Player
    {
        public string Name { get; }
        public string Description { get; }
        public Location CurrentLocation { get; set; }
        public List<Item> Inventory { get; private set; }

        public Player(string name, string description)
        {
            Name = name;
            Description = description;
            CurrentLocation = new Location("海滩", "你降落在一个荒凉的海滩上，四周是茂密的丛林。");
            Inventory = new List<Item>();
        }
    }

    public class NPC
    {
        public string Name { get; }
        public string Description { get; }

        public NPC(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    public class Item
    {
        public string Name { get; }
        public string Description { get; }

        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    public class Puzzle
    {
        public string Name { get; }
        public string Description { get; }

        public Puzzle(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    public class Location
    {
        public string Name { get; }
        public string Description { get; }

        public Location(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
