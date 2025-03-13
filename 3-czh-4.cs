public class IslandLoader : MonoBehaviour {
    private List<AsyncOperation> _loadingOperations = new();
    
    public void LoadIsland(string islandName) {
        StartCoroutine(LoadIslandAsync(islandName));
    }
    
    IEnumerator LoadIslandAsync(string islandName) {
        // 分块加载优化
        _loadingOperations.Add(SceneManager.LoadSceneAsync(islandName, LoadSceneMode.Additive));
        
        foreach (var operation in _loadingOperations) {
            while (!operation.isDone) {
                UpdateLoadingUI(operation.progress);
                yield return null;
            }
        }
        
        InitializeDynamicPhysics(islandName); // 初始化岛屿物理规则
    }
    
    void InitializeDynamicPhysics(string island) {
        if (island.Contains("Mechanical")) {
            Physics.gravity *= 0.7f; // 机械岛低重力环境
        }
    }
}