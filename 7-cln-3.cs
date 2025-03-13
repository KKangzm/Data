public class PuzzleDirector : MonoBehaviour {
    // 潮汐系统示例：水位随现实时间变化
    public Transform waterPlane;
    private float tideSpeed = 0.1f;
    private Vector3 initialPosition;

    // AI提示系统
    [SerializeField] private float hintDelay = 60f; 
    private Dictionary<string, float> puzzleStuckTime = new Dictionary<string, float>();

    void Start() {
        initialPosition = waterPlane.position;
        StartCoroutine(TideMovement());
    }

    IEnumerator TideMovement() {
        while(true) {
            // 正弦模拟潮汐
            float yOffset = Mathf.Sin(Time.time * tideSpeed) * 2f;
            waterPlane.position = initialPosition + new Vector3(0, yOffset, 0);
            
            // 触发水位相关机关
            if(yOffset > 1.5f) {
                EventManager.TriggerEvent("HighTide", null);
            }
            yield return null;
        }
    }

    // 当玩家卡关时触发提示
    public void RegisterPuzzleStuck(string puzzleID) {
        if(!puzzleStuckTime.ContainsKey(puzzleID)) {
            StartCoroutine(CheckStuckTime(puzzleID));
        }
    }

    IEnumerator CheckStuckTime(string puzzleID) {
        float timer = 0;
        while(timer < hintDelay) {
            timer += Time.deltaTime;
            yield return null;
        }
        
        // 触发提示：狐狸指引/高亮关键物体
        GameObject hintObject = PuzzleDatabase.GetHintObject(puzzleID);
        hintObject.GetComponent<Outline>().enabled = true;
        
        // 狐狸AI移动到目标位置
        TimeFoxAI.Instance.MoveToHintPosition(hintObject.transform.position);
    }
}