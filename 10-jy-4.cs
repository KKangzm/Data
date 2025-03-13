public class TemporalObject : MonoBehaviour {
    public GameObject pastState;  // 直立树木
    public GameObject futureState; // 倒塌桥梁
    
    void Update() {
        // 根据时间线切换碰撞体状态
        bool isPast = TimeStateManager.Instance.IsPastDimension;
        pastState.GetComponent<Collider>().enabled = isPast;
        futureState.GetComponent<Collider>().enabled = !isPast;
        
        // 叠加显示效果
        if(TimeStateManager.Instance.IsTimeBlending) {
            SetTransparency(pastState, 0.5f);
            SetTransparency(futureState, 0.5f);
        }
    }
}