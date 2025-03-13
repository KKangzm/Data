public class MemoryFragment : MonoBehaviour {
    public string memoryID;
    public GameObject hologramPrefab; // 关联的记忆回放预制体
    
    void OnCollect() {
        InventorySystem.AddItem("MemoryFragment", 1);
        PlayMemorySequence();
        SaveSystem.SetFragmentCollected(memoryID);
    }

    void PlayMemorySequence() {
        Instantiate(hologramPrefab).GetComponent<MemoryHologram>()
            .Play(memoryID); // 播放3D全息记忆片段
    }
}

// 记忆回溯管理器
public class MemoryReplaySystem : MonoBehaviour {
    private Dictionary<string, Texture2D[]> _memoryTextures = new();
    
    public void LoadMemory(string id) {
        StartCoroutine(AssembleMemoryFragments(
            _memoryTextures[id], 
            PlayerCamera.overlayMaterial // 在屏幕空间拼合
        ));
    }
}