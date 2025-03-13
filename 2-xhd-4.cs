// 食物链能量系统
public class EcologySystem : MonoBehaviour {
    private Dictionary<SpeciesType, List<Creature>> speciesMap = new Dictionary<SpeciesType, List<Creature>>();
    
    void Update(){
        foreach(var predatorSpecies in speciesMap.Keys){
            foreach(var preySpecies in GetPreySpecies(predatorSpecies)){
                SimulatePredation(predatorSpecies, preySpecies);
            }
        }
    }

    void SimulatePredation(SpeciesType predator, SpeciesType prey){
        foreach(var predatorCreature in speciesMap[predator]){
            foreach(var preyCreature in speciesMap[prey]){
                if(Vector3.Distance(predatorCreature.transform.position, 
                                  preyCreature.transform.position) < 5f){
                    preyCreature.TakeDamage(10);
                    predatorCreature.Heal(5);
                }
            }
        }
    }
}

// 动态环境事件
public class EnvironmentalEventSystem : MonoBehaviour {
    [SerializeField] private AnimationCurve eventProbabilityCurve;
    
    void CheckForEvents(){
        float imbalance = EcologySystem.GetCurrentImbalance();
        float probability = eventProbabilityCurve.Evaluate(imbalance);
        
        if(Random.value < probability){
            TriggerRandomEvent();
        }
    }

    void TriggerRandomEvent(){
        EventType eventType = (EventType)Random.Range(0, Enum.GetValues(typeof(EventType)).Length);
        switch(eventType){
            case EventType.Earthquake:
                StartCoroutine(ExecuteEarthquake());
                break;
            // 其他事件处理...
        }
    }
}