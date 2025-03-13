public abstract class TimeSensitiveObject : MonoBehaviour {
    // 存储双时间线状态的数据结构
    [System.Serializable]
    public struct EraState {
        public GameObject[] activateObjects;
        public Collider[] interactiveColliders;
        public Material[] swapMaterials;
    }

    public EraState modernState;
    public EraState ancientState;

    protected virtual void OnEnable() {
        EventManager.StartListening("PostTimeShift", OnTimeShift);
    }

    protected virtual void OnDisable() {
        EventManager.StopListening("PostTimeShift", OnTimeShift);
    }

    // 时间切换时更新对象状态
    private void OnTimeShift(object era) {
        bool isModern = (bool)era;
        EraState targetState = isModern ? modernState : ancientState;

        foreach (GameObject obj in targetState.activateObjects) {
            obj.SetActive(!obj.activeSelf); // 反转激活状态
        }

        foreach (Collider col in targetState.interactiveColliders) {
            col.enabled = !col.enabled; // 反转碰撞体
        }

        // 材质切换（如残破→完整墙壁）
        if (TryGetComponent<Renderer>(out var renderer)) {
            renderer.materials = targetState.swapMaterials;
        }
    }
}