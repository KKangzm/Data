using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string objectName;
    public bool isLocked = false;
    public string requiredItem = "";

    public void Interact()
    {
        if (isLocked)
        {
            Debug.Log(objectName + " is locked. Find " + requiredItem + " to open it.");
        }
        else
        {
            Debug.Log("You interacted with " + objectName);
            PerformAction();
        }
    }

    void PerformAction()
    {
        // 这里可以扩展解谜逻辑，例如打开门、恢复供电、读取日志等
        Debug.Log(objectName + " action triggered!");
    }
}
