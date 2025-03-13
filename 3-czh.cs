using System;
using System.Collections.Generic;

public class GameCharacter
{
    public float X { get; private set; } = 0f;
    public float Y { get; private set; } = 0f;
    public bool IsJumping { get; private set; } = false;

    public void Move(float deltaX)
    {
        X += deltaX;
    }

    public void Jump()
    {
        if (!IsJumping)
        {
            IsJumping = true;
            // 假设一个简单的跳跃逻辑
            Y += 10f;
        }
    }

    public void Land()
    {
        if (IsJumping)
        {
            IsJumping = false;
            Y = 0f;
        }
    }

    public void InteractWith(GameItem item)
    {
        item.Interact(this);
    }
}

public abstract class GameItem
{
    public virtual void Interact(GameCharacter character)
    {
        Console.WriteLine("Interacting with an item.");
    }
}

public class Key : GameItem
{
    public override void Interact(GameCharacter character)
    {
        Console.WriteLine("Picked up a key.");
    }
}

public class Puzzle
{
    private bool _isSolved = false;

    public void Solve()
    {
        if (!_isSolved)
        {
            _isSolved = true;
            Console.WriteLine("Puzzle solved!");
        }
    }

    public bool IsSolved => _isSolved;
}

public class ResourceManager
{
    private Dictionary<string, int> _resources = new Dictionary<string, int>();

    public void CollectResource(string resource, int amount)
    {
        if (_resources.ContainsKey(resource))
        {
            _resources[resource] += amount;
        }
        else
        {
            _resources.Add(resource, amount);
        }
        Console.WriteLine($"Collected {amount} of {resource}. Total: {_resources[resource]}");
    }

    public void UseResource(string resource, int amount)
    {
        if (_resources.TryGetValue(resource, out int currentAmount) && currentAmount >= amount)
        {
            _resources[resource] -= amount;
            Console.WriteLine($"Used {amount} of {resource}. Remaining: {_resources[resource]}");
        }
        else
        {
            Console.WriteLine($"Not enough {resource} to use.");
        }
    }
}

public class SaveLoadSystem
{
    public void SaveGame(GameCharacter character, ResourceManager resources)
    {
        // 这里只是一个示例，实际中应该将数据序列化并保存到文件或数据库
        Console.WriteLine("Game saved.");
    }

    public void LoadGame(GameCharacter character, ResourceManager resources)
    {
        // 这里只是一个示例，实际中应该从文件或数据库反序列化数据
        Console.WriteLine("Game loaded.");
    }
}

public class Program
{
    public static void Main()
    {
        GameCharacter player = new GameCharacter();
        ResourceManager resources = new ResourceManager();
        SaveLoadSystem saveLoadSystem = new SaveLoadSystem();

        player.Move(5f);
        player.Jump();
        player.Land();
        player.InteractWith(new Key());

        Puzzle puzzle = new Puzzle();
        puzzle.Solve();

        resources.CollectResource("Wood", 10);
        resources.UseResource("Wood", 5);

        saveLoadSystem.SaveGame(player, resources);
        saveLoadSystem.LoadGame(player, resources);
    }
}
