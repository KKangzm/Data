// 资源衰减系统
public class ResourceDepletion : MonoBehaviour {
    [SerializeField] private float depletionInterval = 300f; // 5分钟现实时间
    
    void Start(){
        StartCoroutine(DepletionCycle());
    }

    IEnumerator DepletionCycle(){
        while(true){
            yield return new WaitForSeconds(depletionInterval);
            foreach(ResourceNode node in FindObjectsOfType<ResourceNode>()){
                node.Deplete(0.2f);
            }
        }
    }
}

// 动态建筑系统
public class DynamicStructure : MonoBehaviour {
    [SerializeField] private float maxDurability = 100f;
    private float currentDurability;
    
    void Update(){
        currentDurability -= Time.deltaTime * CalculateDecayRate();
        if(currentDurability <= 0){
            TriggerCollapse();
        }
    }

    float CalculateDecayRate(){
        return 1 + (WeatherSystem.Instance.StormIntensity * 0.5f);
    }
}