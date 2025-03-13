public class SoundPuzzleController : MonoBehaviour {
    public AudioClip[] frequencyClips; // 不同频率音波
    private int currentSequenceStep = 0;
    private int[] targetSequence = {2,4,1,3}; // 正确音阶顺序

    // 玩家触发石柱时
    public void OnPillarActivated(int pillarID) {
        if(pillarID == targetSequence[currentSequenceStep]) {
            currentSequenceStep++;
            PlayFrequencyEffect(pillarID);
            if(currentSequenceStep >= targetSequence.Length) {
                SolvePuzzle();
            }
        } else {
            ResetPuzzle();
        }
    }

    private void PlayFrequencyEffect(int id) {
        // 生成可视化音波波纹
        WaveRenderer.DrawWaveform(frequencyClips[id].frequency); 
        // 改变潮汐池物理状态
        TidalPool.ChangeViscosity(id * 0.2f); 
    }
}