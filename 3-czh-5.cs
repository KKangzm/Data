public class EndingManager : MonoBehaviour {
    private int _moralChoicePoints;
    private float _puzzleSpeedBonus;
    
    // 根据玩家行为计算结局
    public void CalculateEnding() {
        float score = _moralChoicePoints * 0.6f + _puzzleSpeedBonus * 0.4f;
        
        if (score > 80) TriggerEnding(EndingType.Liberation);
        else if (score > 40) TriggerEnding(EndingType.Neutral);
        else TriggerEnding(EndingType.Domination);
    }
    
    // 道德选择事件监听
    void OnNPCEvent(NPCInteractionType type) {
        if (type == NPCInteractionType.Save) _moralChoicePoints += 20;
        else if (type == NPCInteractionType.Abandon) _moralChoicePoints -= 15;
    }
}