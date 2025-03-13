public abstract class PuzzleBase : MonoBehaviour {
    public string puzzleID;
    public bool isSolved;
    
    // 谜题通用接口
    public abstract void OnPlayerInteract(GameObject player);
    protected virtual void SolvePuzzle() {
        isSolved = true;
        EventSystem.TriggerEvent("PuzzleSolved", puzzleID); // 事件驱动
    }
}

// 具体谜题实现示例：齿轮谜题
public class GearPuzzle : PuzzleBase {
    public List<RotatingGear> requiredGears;
    
    public override void OnPlayerInteract(GameObject player) {
        if (CheckGearAlignment()) SolvePuzzle();
    }
    
    bool CheckGearAlignment() {
        return requiredGears.All(gear => Mathf.Abs(gear.currentRotation - gear.targetAngle) < 5f);
    }
}

// 声音触发谜题（迷雾森林岛）
public class SoundTriggerPuzzle : PuzzleBase {
    public AudioClip targetAnimalSound;
    private float[] _spectrumData = new float[1024];
    
    void Update() {
        AudioListener.GetSpectrumData(_spectrumData, 0, FFTWindow.BlackmanHarris);
        if (DetectTargetFrequency(targetAnimalSound.frequency)) SolvePuzzle();
    }
}