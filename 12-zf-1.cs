using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandGameManager : MonoBehaviour
{
    public static IslandGameManager Instance;
    public List<Location> locations;
    public Text gameMessage;
    
    private HashSet<string> discoveredLocations = new HashSet<string>();
    private HashSet<string> rescuedAnimals = new HashSet<string>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ExploreLocation(string locationId)
    {
        if (!discoveredLocations.Contains(locationId))
        {
            discoveredLocations.Add(locationId);
            gameMessage.text = $"探索了 {locationId}!";
        }
        else
        {
            gameMessage.text = $"你已经探索过 {locationId}!";
        }
    }

    public void SolvePuzzle(string locationId)
    {
        if (discoveredLocations.Contains(locationId))
        {
            gameMessage.text = $"成功解开 {locationId} 的谜题!";
        }
        else
        {
            gameMessage.text = $"你需要先探索 {locationId}!";
        }
    }
    
    public void RescueAnimal(string animalId)
    {
        if (!rescuedAnimals.Contains(animalId))
        {
            rescuedAnimals.Add(animalId);
            gameMessage.text = $"你救援了一只 {animalId}!";
        }
        else
        {
            gameMessage.text = $"{animalId} 已经被救援!";
        }
    }
}

[System.Serializable]
public class Location
{
    public string locationId;
    public string displayName;
    public bool hasPuzzle;
    public bool hasAnimal;
}