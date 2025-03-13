using UnityEngine;

public class GameEnding : MonoBehaviour
{
    public enum EndingType { Escape, DestroyLab, CureMutation };
    
    public void TriggerEnding(EndingType ending)
    {
        switch (ending)
        {
            case EndingType.Escape:
                Debug.Log("You managed to escape the island... but at what cost?");
                break;

            case EndingType.DestroyLab:
                Debug.Log("You destroyed the facility, burying its secrets forever.");
                break;

            case EndingType.CureMutation:
                Debug.Log("You found a cure... but will the world believe you?");
                break;
        }

        EndGame();
    }

    void EndGame()
    {
        Debug.Log("Game Over. Returning to main menu.");
        // 加载主菜单或结局动画
    }
}
