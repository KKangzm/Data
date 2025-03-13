using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    private List<string> completedQuests = new List<string>();

    void Awake()
    {
        Instance = this;
    }

    public void CompleteQuest(string questName)
    {
        if (!completedQuests.Contains(questName))
        {
            completedQuests.Add(questName);
            Debug.Log("Quest Completed: " + questName);
        }
    }

    public bool IsQuestCompleted(string questName)
    {
        return completedQuests.Contains(questName);
    }
}
