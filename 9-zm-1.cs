// 环境参数监听器
public class EnvironmentalTrigger : MonoBehaviour {
    [System.Serializable]
    public class PuzzleCondition {
        public TimeSpan requiredTime;
        public WeatherType requiredWeather;
        public float physicsValueThreshold;
    }

    public PuzzleCondition currentCondition;
    private bool isSolved = false;

    void Update() {
        if (!isSolved && CheckConditions()) {
            SolvePuzzle();
        }
    }

    bool CheckConditions() {
        return WorldTime.Instance.CurrentTime >= currentCondition.requiredTime &&
               WeatherSystem.Instance.CurrentWeather == currentCondition.requiredWeather &&
               PhysicsSensor.LastRecordedValue >= currentCondition.physicsValueThreshold;
    }

    void SolvePuzzle() {
        GetComponent<IPuzzle>().OnSolved();
        isSolved = true;
    }
}

// 谜题逻辑编辑器基础接口
public interface IPuzzleEditor {
    void ConfigureCondition(PuzzleCondition condition);
    void LinkToEnvironmentSensor(int sensorID);
    void SetFeedbackParticles(ParticleSystem particles);
}