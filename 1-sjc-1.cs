// 核心游戏系统
public class SurvivalSystem
{
    // 资源管理系统
    public class ResourceManager
    {
        private Dictionary<string, int> resources = new Dictionary<string, int>();
        
        public void AddResource(string type, int amount) => 
            resources[type] = resources.ContainsKey(type) ? resources[type] + amount : amount;
        
        public bool ConsumeResource(string type, int amount)
        {
            if (resources.ContainsKey(type) && resources[type] >= amount)
            {
                resources[type] -= amount;
                return true;
            }
            return false;
        }
    }

    // 角色系统
    public class Survivor
    {
        public string Name { get; }
        public Dictionary<SkillType, int> Skills { get; } = new Dictionary<SkillType, int>();
        public bool IsInfected { get; set; }

        public Survivor(string name, Dictionary<SkillType, int> skills)
        {
            Name = name;
            Skills = skills;
        }

        public bool PerformTask(TaskType taskType)
        {
            // 根据任务类型计算成功率
            return new Random().Next(0, 100) < GetSkillValue(taskType) * 10;
        }

        private int GetSkillValue(TaskType taskType) => 
            taskType switch {
                TaskType.Medical => Skills[SkillType.Medical],
                TaskType.Engineering => Skills[SkillType.Engineering],
                _ => Skills[SkillType.Survival]
            };
    }

    // 探索系统
    public class ExplorationSystem
    {
        private List<Location> discoveredLocations = new List<Location>();
        
        public void ExploreLocation(Location location)
        {
            if (!discoveredLocations.Contains(location))
            {
                discoveredLocations.Add(location);
                OnNewLocationDiscovered?.Invoke(location);
            }
        }

        public event Action<Location> OnNewLocationDiscovered;
    }

    // 谜题系统
    public class PuzzleSystem
    {
        public bool SolvePuzzle(Puzzle puzzle, string playerInput)
        {
            bool solved = puzzle.VerifySolution(playerInput);
            if (solved) puzzle.OnSolved?.Invoke();
            return solved;
        }
    }

    // 动态事件系统
    public class EventSystem
    {
        private List<GameEvent> possibleEvents = new List<GameEvent>();
        
        public void TriggerRandomEvent()
        {
            if (possibleEvents.Count == 0) return;
            int index = new Random().Next(0, possibleEvents.Count);
            possibleEvents[index].TriggerEvent();
        }
    }
}

// 游戏核心数据模型
public enum SkillType { Medical, Engineering, Combat, Survival }
public enum TaskType { Healing, Building, Fighting, Exploring }

public class Location
{
    public string Name { get; }
    public List<Item> AvailableItems { get; }
    public Puzzle AssociatedPuzzle { get; }
    public bool IsLabEntrance { get; }
}

public abstract class Puzzle
{
    public string Description { get; }
    public abstract bool VerifySolution(string input);
    public Action OnSolved;
}

public class LabPuzzle : Puzzle
{
    public override bool VerifySolution(string input)
    {
        // 实验室密码谜题示例
        return input == "VIRUS_ORIGIN_2050";
    }
}

// 游戏管理器
public class GameManager : MonoBehaviour 
{
    private ResourceManager resourceManager = new ResourceManager();
    private List<Survivor> survivors = new List<Survivor>();
    private ExplorationSystem exploration = new ExplorationSystem();
    
    private void Start()
    {
        InitializeGame();
        StartCoroutine(DayNightCycle());
    }

    private void InitializeGame()
    {
        // 创建初始幸存者
        survivors.Add(new Survivor("Doctor", new Dictionary<SkillType, int> {
            [SkillType.Medical] = 8,
            [SkillType.Survival] = 5
        }));
    }

    // 时间管理系统
    private IEnumerator DayNightCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(120); // 每2分钟为1游戏小时
            UpdateTime();
        }
    }

    private void UpdateTime()
    {
        // 实现时间推进逻辑
        // 触发夜间事件、资源消耗等
    }
}

// 动态事件示例
public class InfectionEvent : GameEvent
{
    public override void TriggerEvent()
    {
        // 随机感染幸存者逻辑
    }
}