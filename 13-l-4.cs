using UnityEngine;

public class Exploration : MonoBehaviour
{
    public string areaName;
    public bool isDiscovered = false;
    
    public void DiscoverArea()
    {
        if (!isDiscovered)
        {
            isDiscovered = true;
            Debug.Log($"Discovered a new area: {areaName}");
        }
        else
        {
            Debug.Log($"You have already explored {areaName}.");
        }
    }
}
