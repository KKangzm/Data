// 时间状态管理器
public class TimeStateManager : MonoBehaviour {
    private Stack<TimeSnapshot> timeStack = new Stack<TimeSnapshot>();
    
    // 创建时间快照
    public void CaptureSnapshot() {
        timeStack.Push(new TimeSnapshot(
            currentEnvironmentState,
            player.transform.position,
            activePuzzleStatus
        ));
    }

    // 执行时间回退
    public void RevertTime() {
        if(timeStack.Count > 0) {
            TimeSnapshot snapshot = timeStack.Pop();
            ApplyEnvironmentState(snapshot.environmentState);
            player.ResetPosition(snapshot.playerPosition);
            UpdatePuzzleStatus(snapshot.puzzleStatus);
        }
    }
}

// 环境状态切换器
public class EnvironmentSwitcher : MonoBehaviour {
    public GameObject pastVersion;  // 古代神庙
    public GameObject futureVersion; // 现代实验室
    
    public void SwitchVersion(bool isPast) {
        pastVersion.SetActive(isPast);
        futureVersion.SetActive(!isPast);
        // 触发材质替换（手绘→赛博风格）
        GetComponent<MaterialSwapper>().SwapMaterials(isPast); 
    }
}