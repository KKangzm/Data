public class TimeShiftController : MonoBehaviour {
    public static TimeShiftController Instance; // 单例模式
    
    [Header("Config")]
    public int maxTimeFragments = 5;  // 最大时间碎片容量
    public float timeShiftCooldown = 3f; // 切换冷却
    
    [Header("States")]
    [SerializeField] private int currentFragments;
    [SerializeField] private bool isModernEra = true;
    private bool canShift = true;

    void Awake() {
        Instance = this;
        currentFragments = maxTimeFragments; // 初始满碎片
    }

    // 玩家触发时间切换
    public void AttemptTimeShift() {
        if(canShift && currentFragments > 0) {
            StartCoroutine(ShiftTimeCoroutine());
        }
    }

    IEnumerator ShiftTimeCoroutine() {
        canShift = false;
        currentFragments--;
        
        // 触发全局事件
        EventManager.TriggerEvent("PreTimeShift", isModernEra);
        
        // 环境切换特效（2秒）
        CameraFX.PlayTimeRippleEffect();
        yield return new WaitForSeconds(2f);
        
        isModernEra = !isModernEra;
        EventManager.TriggerEvent("PostTimeShift", isModernEra);
        
        yield return new WaitForSeconds(timeShiftCooldown);
        canShift = true;
    }

    // 从谜题/场景中获取碎片
    public void AddTimeFragment(int amount) {
        currentFragments = Mathf.Clamp(currentFragments + amount, 0, maxTimeFragments);
        UIManager.UpdateFragmentUI(currentFragments);
    }
}