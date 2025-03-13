public class EndingManager : MonoBehaviour {
    private int moralityValue; // -10到+10区间
    
    public void CheckEndingConditions() {
        if(reactorDestroyed) {
            if(moralityValue > 5) TriggerEnding("时之救赎");
            else TriggerEnding("永恒囚徒");
        } else if(escapedAlone) {
            TriggerEnding("沉默真相");
        }
    }

    private void TriggerEnding(string endingID) {
        // 加载对应结局CG与字幕
        SceneLoader.LoadEndingScene(endingID);
        // 存档结局成就
        SteamAchievement.Unlock(endingID); 
    }
}