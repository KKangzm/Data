using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGameCore : MonoBehaviour
{
    // 资源管理
    private Dictionary<string, int> resources = new Dictionary<string, int>();
    
    // 角色属性
    public int experience = 0;
    public int strength = 10;
    public int agility = 10;
    public int intelligence = 10;
    
    // 设施管理
    private List<GameObject> buildings = new List<GameObject>();
    
    // 探索系统
    private HashSet<Vector2> exploredAreas = new HashSet<Vector2>();
    
    // 战斗系统
    public int playerHealth = 100;
    public int enemyHealth = 100;
    public bool isPlayerTurn = true;
    
    // 多结局
    private string gameEnding = "";
    
    void Start()
    {
        InitResources();
        StartCoroutine(DisasterCycle());
    }
    
    // 资源管理
    private void InitResources()
    {
        resources["Wood"] = 100;
        resources["Stone"] = 50;
        resources["Food"] = 200;
    }
    
    public void CollectResource(string type, int amount)
    {
        if (resources.ContainsKey(type))
        {
            resources[type] += amount;
        }
    }
    
    public bool UseResource(string type, int amount)
    {
        if (resources.ContainsKey(type) && resources[type] >= amount)
        {
            resources[type] -= amount;
            return true;
        }
        return false;
    }
    
    // 探索与解谜
    public void ExploreArea(Vector2 position)
    {
        exploredAreas.Add(position);
        Debug.Log("Explored: " + position);
    }
    
    public bool SolvePuzzle(string puzzleType, string solution)
    {
        return puzzleType == solution;
    }
    
    // 战斗系统
    public void StartBattle()
    {
        playerHealth = 100;
        enemyHealth = 100;
        isPlayerTurn = true;
    }
    
    public void PlayerAttack()
    {
        if (isPlayerTurn)
        {
            enemyHealth -= strength;
            Debug.Log("Player attacked! Enemy health: " + enemyHealth);
            isPlayerTurn = false;
        }
    }
    
    public void EnemyAttack()
    {
        if (!isPlayerTurn)
        {
            playerHealth -= 10;
            Debug.Log("Enemy attacked! Player health: " + playerHealth);
            isPlayerTurn = true;
        }
    }
    
    // 角色成长
    public void GainExperience(int amount)
    {
        experience += amount;
        if (experience >= 100)
        {
            LevelUp();
            experience = 0;
        }
    }
    
    private void LevelUp()
    {
        strength += 2;
        agility += 2;
        intelligence += 2;
        Debug.Log("Level Up! Strength: " + strength + " Agility: " + agility + " Intelligence: " + intelligence);
    }
    
    // 多结局
    public void DetermineEnding()
    {
        if (playerHealth <= 0)
        {
            gameEnding = "Defeat";
        }
        else if (exploredAreas.Count > 10)
        {
            gameEnding = "Explorer's Victory";
        }
        Debug.Log("Game Ending: " + gameEnding);
    }
    
    // 自然灾害
    private IEnumerator DisasterCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(30, 90));
            TriggerDisaster();
        }
    }
    
    private void TriggerDisaster()
    {
        string[] disasters = { "Storm", "Earthquake", "Flood" };
        string disaster = disasters[Random.Range(0, disasters.Length)];
        Debug.Log("Disaster Occurred: " + disaster);
    }
}