using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool isLocked = true;
    public string requiredKey = "GoldenKey";

    public void Interact()
    {
        if (isLocked)
        {
            if (Inventory.Instance.HasItem(requiredKey))
            {
                isLocked = false;
                Debug.Log("Chest opened! You found a treasure!");
            }
            else
            {
                Debug.Log("The chest is locked. You need a key.");
            }
        }
        else
        {
            Debug.Log("The chest is already open.");
        }
    }
}
