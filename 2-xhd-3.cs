// 动态谜题状态机
public class PuzzleController : MonoBehaviour {
    private enum PuzzleState { Idle, Activated, Solved }
    private PuzzleState currentState;
    
    private List<IPuzzleElement> elements = new List<IPuzzleElement>();
    
    void Start(){
        GeneratePuzzle();
    }

    void GeneratePuzzle(){
        int complexity = Mathf.Clamp(GameManager.Instance.PlayerSkillLevel, 1, 5);
        var puzzleTemplate = PuzzleDatabase.GetRandomTemplate(complexity);
        
        foreach(var elementData in puzzleTemplate.elements){
            IPuzzleElement element = InstantiateElement(elementData);
            element.OnInteract += HandleElementInteraction;
            elements.Add(element);
        }
    }

    void HandleElementInteraction(PuzzleElementType type){
        // 实现状态转移逻辑
        if(CheckSolutionCondition()){
            currentState = PuzzleState.Solved;
            RewardSystem.GrantReward();
        }
    }
}

// 谜题元素接口
public interface IPuzzleElement {
    event Action<PuzzleElementType> OnInteract;
    void Activate();
    void Reset();
}