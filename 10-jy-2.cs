public class MemoryFragmentSystem : MonoBehaviour {
    private Dictionary<string, bool> unlockedAbilities = new Dictionary<string, bool>(){
        {"TimeSlow", false}, 
        {"Precognition", false}
    };

    public void CollectFragment(MemoryType type) {
        switch(type) {
            case MemoryType.PASSIVE:
                PlayMemoryCutscene(GetNextPassiveMemory());
                break;
            case MemoryType.ACTIVE:
                UnlockNewAbility();
                break;
        }
    }

    private void UnlockNewAbility() {
        if(fragmentsCollected >= 3 && !unlockedAbilities["Precognition"]) {
            player.AddComponent<PrecognitionSkill>();
            unlockedAbilities["Precognition"] = true;
        }
    }
}

// 预知技能实现
public class PrecognitionSkill : MonoBehaviour {
    private float duration = 5f; // 预知5秒
    
    void Update() {
        if(Input.GetKeyDown(KeyCode.Q)) {
            StartCoroutine(ShowDangerPreview());
        }
    }

    IEnumerator ShowDangerPreview() {
        Time.timeScale = 0.2f; // 子弹时间
        GhostTrail.GenerateFuturePath(); // 显示未来轨迹
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1f;
    }
}