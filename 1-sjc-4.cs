// PuzzleDevice.cs
public class PuzzleDevice : MonoBehaviour
{
    [System.Serializable]
    public struct PuzzleSequence{
        public int[] correctOrder; // 正确操作序列
        public GameObject[] linkedObjects; // 关联的可交互物体
    }

    public PuzzleSequence virusSequence;
    private List<int> inputHistory = new List<int>();

    public void RegisterInput(int inputCode){
        inputHistory.Add(inputCode);
        CheckSolution();
    }

    void CheckSolution(){
        if(inputHistory.Count != virusSequence.correctOrder.Length) return;
        
        for(int i=0; i<virusSequence.correctOrder.Length; i++){
            if(inputHistory[i] != virusSequence.correctOrder[i]){
                ResetPuzzle();
                return;
            }
        }
        TriggerSuccess();
    }

    void TriggerSuccess(){
        foreach(GameObject obj in virusSequence.linkedObjects){
            obj.GetComponent<Animator>().SetTrigger("Activate");
        }
    }
}