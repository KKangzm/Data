using UnityEngine;

public class PuzzleDoor : MonoBehaviour, IInteractable
{
    public bool isSolved = false;

    public void Interact()
    {
        if (isSolved)
        {
            Debug.Log("The door opens...");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("The puzzle is not solved yet.");
        }
    }

    public void SolvePuzzle()
    {
        isSolved = true;
    }
}
