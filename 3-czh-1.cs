public class DynamicEnvironmentSystem : MonoBehaviour {
    // 天气状态机
    public enum WeatherState { Sunny, Rainy, Storm, Foggy }
    private WeatherState _currentWeather;
    
    // 天气对场景的影响（可扩展）
    void UpdateWeatherEffects() {
        switch (_currentWeather) {
            case WeatherState.Rainy:
                EnableConductiveSurfaces(true); // 雨水导电逻辑
                AdjustWaterLevel(0.2f);         // 水位上涨
                break;
            case WeatherState.Sunny:
                TriggerMirrorReflections();     // 镜面反射激活
                break;
            // 其他天气状态...
        }
    }

    // 动态物体重力控制（如漂浮岛屿）
    void ApplyDynamicGravity(Rigidbody obj, bool isReverse) {
        obj.useGravity = !isReverse;
        obj.AddForce(isReverse ? Physics.gravity * -2 : Vector3.zero);
    }
}