using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGameCore : MonoBehaviour
{
    // 资源管理
    private Dictionary<string, int> resources = new Dictionary<string, int>();
    
    // 设施管理
    private List<GameObject> buildings = new List<GameObject>();
    
    // 探索系统
    private HashSet<Vector2> exploredAreas = new HashSet<Vector2>();
    
    // 军队与战斗
    private List<GameObject> trainedUnits = new List<GameObject>();
    
    // 灾害系统
    private string[] disasters = { "Storm", "Earthquake", "Flood" };
    
    // 居民管理
    private int population = 10;
    private int happiness = 50;
    
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
    
    // 建造与设施
    public void ConstructBuilding(GameObject buildingPrefab, Vector3 position)
    {
        GameObject newBuilding = Instantiate(buildingPrefab, position, Quaternion.identity);
        buildings.Add(newBuilding);
    }
    
    public void UpgradeBuilding(GameObject building)
    {
        // 设施升级逻辑
        Debug.Log("Building upgraded: " + building.name);
    }
    
    public void MaintainBuilding(GameObject building)
    {
        Debug.Log("Building maintained: " + building.name);
    }
    
    // 探索与解谜
    public void ExploreArea(Vector2 position)
    {
        exploredAreas.Add(position);
    }
    
    public bool SolvePuzzle(string puzzleType, string solution)
    {
        return puzzleType == solution;
    }
    
    // 战斗与策略
    public void TrainUnit(GameObject unitPrefab)
    {
        GameObject unit = Instantiate(unitPrefab);
        trainedUnits.Add(unit);
    }
    
    public void StartBattle()
    {
        Debug.Log("Battle Started!");
    }
    
    public void HandleDiplomacy(string action)
    {
        Debug.Log("Diplomatic Action: " + action);
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
        string disaster = disasters[Random.Range(0, disasters.Length)];
        Debug.Log("Disaster Occurred: " + disaster);
    }
    
    // 居民管理
    public void AdjustPopulation(int change)
    {
        population += change;
    }
    
    public void AdjustHappiness(int change)
    {
        happiness += change;
    }
}
