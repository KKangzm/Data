// DynamicEnvironment.cs
public class DynamicEnvironment : MonoBehaviour
{
    [Header("冰川崩解")] 
    public GameObject[] iceSections;
    public float collapseInterval = 180f; // 每3分钟变化
    
    [Header("孢子风暴")] 
    public ParticleSystem sporeStorm;
    public float stormProbability = 0.3f;

    IEnumerator Start(){
        while(true){
            yield return new WaitForSeconds(collapseInterval);
            CollapseRandomIceSection();
            CheckStormActivation();
        }
    }

    void CollapseRandomIceSection(){
        int index = Random.Range(0, iceSections.Length);
        iceSections[index].GetComponent<IceSection>().TriggerCollapse();
    }

    void CheckStormActivation(){
        if(Random.value <= stormProbability){
            StartCoroutine(TriggerSporeStorm());
        }
    }

    IEnumerator TriggerSporeStorm(){
        sporeStorm.Play();
        yield return new WaitForSeconds(120f);
        sporeStorm.Stop();
    }
}