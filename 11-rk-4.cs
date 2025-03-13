using UnityEngine;

public class ElectronicDoor : MonoBehaviour
{
    public bool isLocked = true;
    public string requiredKeycard = "LabKey";

    public void TryUnlock(string playerItem)
    {
        if (playerItem == requiredKeycard)
        {
            isLocked = false;
            Debug.Log("Door unlocked!");
            OpenDoor();
        }
        else
        {
            Debug.Log("Access denied. You need a " + requiredKeycard);
        }
    }

    void OpenDoor()
    {
        // 触发开门动画
        Debug.Log("Door is opening...");
        gameObject.SetActive(false); // 临时模拟门打开
    }
}
