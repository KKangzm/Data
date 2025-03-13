using UnityEngine;

public class DoorMechanism : MonoBehaviour
{
    public GameObject switch1;
    public GameObject switch2;
    public GameObject door;

    void Update()
    {
        if (switch1.GetComponent<Switch>().isActivated && switch2.GetComponent<Switch>().isActivated)
        {
            // 打开门的逻辑
            door.SetActive(false);
        }
    }
}
