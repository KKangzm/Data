using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // 初始化游戏环境
        GameEnvironment game = new GameEnvironment();
        
        // 运行游戏主循环
        game.StartGame();
    }
}

class GameEnvironment
{
    private ResourceManagement resourceManager;
    private WeatherSystem weatherSystem;
    private PuzzleSystem puzzleSystem;
    private NPCSystem npcSystem;
    private EndingLogic endingLogic;
    private AudioSystem audioSystem;

    public GameEnvironment()
    {
        resourceManager = new ResourceManagement();
        weatherSystem = new WeatherSystem();
        puzzleSystem = new PuzzleSystem();
        npcSystem = new NPCSystem();
        endingLogic = new EndingLogic();
        audioSystem = new AudioSystem();
    }

    public void StartGame()
    {
        // 模拟一天的游戏时间
        for (int i = 0; i < 24; i++)
        {
            Console.WriteLine($"当前时间：{i}:00");

            // 更新天气
            weatherSystem.UpdateWeather();

            // 收集资源
            resourceManager.CollectResources();

            // 与NPC互动
            npcSystem.InteractWithNPC();

            // 解谜
            puzzleSystem.SolvePuzzle();

            // 更新音效
            audioSystem.PlayBackgroundMusic();

            // 检查是否达到某个结局条件
            if (endingLogic.CheckEndingCondition())
            {
                break;
            }

            // 模拟每小时的等待
            System.Threading.Thread.Sleep(1000);
        }

        // 游戏结束，显示结局
        endingLogic.ShowEnding();
    }
}

class ResourceManagement
{
    private Dictionary<string, int> resources;

    public ResourceManagement()
    {
        resources = new Dictionary<string, int>();
    }

    public void CollectResources()
    {
        // 示例：收集木材
        if (!resources.ContainsKey("Wood"))
        {
            resources["Wood"] = 0;
        }
        resources["Wood"] += 5;
        Console.WriteLine("收集了5个单位的木材");
    }

    public void UseResource(string resourceName, int amount)
    {
        if (resources.ContainsKey(resourceName) && resources[resourceName] >= amount)
        {
            resources[resourceName] -= amount;
            Console.WriteLine($"使用了{amount}个单位的{resourceName}");
        }
        else
        {
            Console.WriteLine($"没有足够的{resourceName}");
        }
    }
}

class WeatherSystem
{
    private string currentWeather;

    public WeatherSystem()
    {
        currentWeather = "Sunny";
    }

    public void UpdateWeather()
    {
        // 简单的天气变化逻辑
        switch (currentWeather)
        {
            case "Sunny":
                currentWeather = "Cloudy";
                break;
            case "Cloudy":
                currentWeather = "Rainy";
                break;
            case "Rainy":
                currentWeather = "Sunny";
                break;
        }
        Console.WriteLine($"当前天气：{currentWeather}");
    }
}

class PuzzleSystem
{
    private List<string> puzzles;

    public PuzzleSystem()
    {
        puzzles = new List<string> { "Logic Puzzle", "Physics Puzzle" };
    }

    public void SolvePuzzle()
    {
        if (puzzles.Count > 0)
        {
            string puzzle = puzzles[0];
            puzzles.RemoveAt(0);
            Console.WriteLine($"解决了{puzzle}");
        }
        else
        {
            Console.WriteLine("没有更多的谜题可解");
        }
    }
}

class NPCSystem
{
    public void InteractWithNPC()
    {
        Console.WriteLine("与NPC进行了对话");
    }
}

class EndingLogic
{
    private bool isEndingTriggered;

    public EndingLogic()
    {
        isEndingTriggered = false;
    }

    public bool CheckEndingCondition()
    {
        // 示例：如果解决了所有谜题，则触发结局
        // 实际游戏中应有更复杂的逻辑
        isEndingTriggered = true;
        return isEndingTriggered;
    }

    public void ShowEnding()
    {
        if (isEndingTriggered)
        {
            Console.WriteLine("恭喜你，达成了游戏的结局！");
        }
        else
        {
            Console.WriteLine("游戏还在继续...");
        }
    }
}

class AudioSystem
{
    public void PlayBackgroundMusic()
    {
        Console.WriteLine("播放背景音乐");
    }
}
