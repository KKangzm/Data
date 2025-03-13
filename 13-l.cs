using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private Dictionary<string, int> resources = new Dictionary<string, int>();

    void Start()
    {
        // 初始化资源
        AddResource("Wood", 0);
        AddResource("Stone", 0);
        AddResource("Food", 0);
    }

    public void AddResource(string resourceName, int amount)
    {
        if (resources.ContainsKey(resourceName))
        {
            resources[resourceName] += amount;
        }
        else
        {
            resources.Add(resourceName, amount);
        }
    }

    public bool ConsumeResource(string resourceName, int amount)
    {
        if (resources.ContainsKey(resourceName) && resources[resourceName] >= amount)
        {
            resources[resourceName] -= amount;
            return true;
        }
        return false;
    }

    public int GetResourceAmount(string resourceName)
    {
        return resources.ContainsKey(resourceName) ? resources[resourceName] : 0;
    }

    public void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 250, 100), "Resource Manager");
        foreach (var resource in resources)
        {
            GUILayout.Label($"{resource.Key}: {resource.Value}");
        }

        if (GUILayout.Button("Collect Wood"))
        {
            CollectResource("Wood", 5);
        }

        if (GUILayout.Button("Collect Stone"))
        {
            CollectResource("Stone", 3);
        }

        if (GUILayout.Button("Build House (10 Wood, 5 Stone)"))
        {
            if (ConsumeResource("Wood", 10) && ConsumeResource("Stone", 5))
            {
                Debug.Log("House built!");
            }
            else
            {
                Debug.Log("Not enough resources to build house.");
            }
        }
    }

    public void CollectResource(string resourceName, int amount)
    {
        AddResource(resourceName, amount);
        Debug.Log($"Collected {amount} {resourceName}. Total: {GetResourceAmount(resourceName)}");
    }
}
