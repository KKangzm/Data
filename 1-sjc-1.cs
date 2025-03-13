// SurvivalManager.cs
public class SurvivalManager : MonoBehaviour
{
    [Header("生存指标")] 
    public float bodyTemperature = 100f; // 体温百分比
    public float infectionLevel = 0f;   // 感染程度 
    public float sanity = 100f;         // 精神值

    [Header("衰减速率")] 
    public float tempDecayRate = 0.5f; 
    public float infectionGrowRate = 0.2f;
    
    void Update() {
        // 基础衰减计算
        bodyTemperature -= tempDecayRate * Time.deltaTime;
        infectionLevel += infectionGrowRate * Time.deltaTime;
        
        // 状态检测
        if(bodyTemperature <= 30f) TriggerHypothermia();
        if(infectionLevel >= 100f) TriggerMutation();
        if(sanity <= 0) TriggerMadness();
    }

    // 状态触发方法
    private void TriggerHypothermia(){
        Debug.Log("进入失温状态！移动速度降低50%");
        // 调用角色移动组件减速
    }
}