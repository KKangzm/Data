// 角色选择与能力系统
public class CharacterSystem : MonoBehaviour
{
    [System.Serializable]
    public class CharacterProfile
    {
        public string name;
        public Sprite portrait;
        public float medicalSkill;
        public float engineeringSkill;
        public float combatSkill;
        public float explorationSpeed;
    }

    public CharacterProfile[] availableCharacters;
    private CharacterProfile selectedCharacter;

    // 角色选择UI事件
    public void OnCharacterSelected(int index)
    {
        selectedCharacter = availableCharacters[index];
        ApplyCharacterBonuses();
    }

    private void ApplyCharacterBonuses()
    {
        PlayerStats.MedicalBonus = selectedCharacter.medicalSkill;
        PlayerStats.EngineeringBonus = selectedCharacter.engineeringSkill;
        // 其他属性应用...
    }
}

// 程序化地图生成系统
public class MapGenerator : MonoBehaviour
{
    public int mapSize = 100;
    public GameObject[] terrainPrefabs;
    public GameObject labPrefab;

    public void GenerateNewMap(int seed)
    {
        System.Random rand = new System.Random(seed);
        
        // 使用柏林噪声生成地形
        float[,] noiseMap = GeneratePerlinNoise(mapSize, seed);
        
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                SpawnTerrain(noiseMap[x, y], x, y);
            }
        }

        PlaceSpecialLocations(rand);
    }

    private void SpawnTerrain(float noiseValue, int x, int y)
    {
        int terrainIndex = Mathf.FloorToInt(noiseValue * terrainPrefabs.Length);
        Instantiate(terrainPrefabs[terrainIndex], new Vector3(x, 0, y), Quaternion.identity);
    }

    private void PlaceSpecialLocations(System.Random rand)
    {
        // 放置实验室和其他关键地点
        Vector3 labPosition = new Vector3(rand.Next(mapSize/4, mapSize*3/4), 0, rand.Next(mapSize/4, mapSize*3/4));
        Instantiate(labPrefab, labPosition, Quaternion.identity);
    }
}

// 资源收集与建设系统
public class ConstructionSystem : MonoBehaviour
{
    public enum BuildingType { Shelter, Workshop, MedicalTent }
    
    public Dictionary<BuildingType, int> builtStructures = new Dictionary<BuildingType, int>();
    
    public void BuildStructure(BuildingType type)
    {
        if (ResourceManager.Instance.ConsumeResources(GetCost(type)))
        {
            builtStructures[type] = builtStructures.ContainsKey(type) ? builtStructures[type] + 1 : 1;
            ApplyStructureEffect(type);
        }
    }

    private Dictionary<ResourceType, int> GetCost(BuildingType type)
    {
        // 返回不同建筑类型的资源需求
    }

    private void ApplyStructureEffect(BuildingType type)
    {
        switch(type)
        {
            case BuildingType.Shelter:
                PlayerStats.RestEfficiency += 0.2f;
                break;
            // 其他建筑效果...
        }
    }
}

// 游牧玩法系统
public class NomadicSystem : MonoBehaviour
{
    public float movementThreshold = 100f;
    private float totalDistanceMoved;
    
    void Update()
    {
        float currentMovement = PlayerMovement.Instance.GetMovementDelta();
        totalDistanceMoved += currentMovement;
        
        if(totalDistanceMoved >= movementThreshold)
        {
            TriggerTravelBonus();
            totalDistanceMoved = 0;
        }
    }

    private void TriggerTravelBonus()
    {
        // 生成随机探索奖励
        int rewardType = Random.Range(0, 3);
        switch(rewardType)
        {
            case 0:
                ResourceManager.Instance.AddResource(ResourceType.Food, 5);
                break;
            case 1:
                TriggerRandomEvent();
                break;
            // 其他奖励类型...
        }
    }
}

// 轻量化战斗系统
public class CombatSystem : MonoBehaviour
{
    public enum CombatAction { Attack, Defend, UseItem }
    
    public void ResolveCombat(PlayerCombatant player, Enemy enemy)
    {
        StartCoroutine(CombatSequence(player, enemy));
    }

    private IEnumerator CombatSequence(PlayerCombatant player, Enemy enemy)
    {
        while(player.IsAlive && enemy.IsAlive)
        {
            // 玩家行动选择
            CombatAction action = UIManager.Instance.GetPlayerAction();
            
            switch(action)
            {
                case CombatAction.Attack:
                    enemy.TakeDamage(player.CalculateDamage());
                    break;
                case CombatAction.Defend:
                    player.EnableDefense();
                    break;
                // 其他行动处理...
            }
            
            // 敌人行动
            yield return new WaitForSeconds(1f);
            player.TakeDamage(enemy.GetNextAttack());
            
            yield return new WaitForSeconds(1f);
        }
    }
}

// UI系统
public class UIManager : MonoBehaviour
{
    public Text resourceText;
    public Text healthText;
    public GameObject constructionPanel;
    
    void Update()
    {
        UpdateResourceDisplay();
        UpdateHealthDisplay();
    }

    private void UpdateResourceDisplay()
    {
        string displayText = $"Food: {ResourceManager.Food}\nWater: {ResourceManager.Water}";
        resourceText.text = displayText;
    }

    public void ToggleConstructionMenu()
    {
        constructionPanel.SetActive(!constructionPanel.activeSelf);
    }
}

// 音效系统
public class AudioController : MonoBehaviour
{
    public AudioClip[] ambientSounds;
    public AudioClip combatMusic;
    
    private AudioSource ambientSource;
    private AudioSource musicSource;

    void Start()
    {
        PlayAmbientSound();
        PlayBackgroundMusic();
    }

    private void PlayAmbientSound()
    {
        ambientSource.clip = ambientSounds[Random.Range(0, ambientSounds.Length)];
        ambientSource.Play();
    }

    public void EnterCombat()
    {
        musicSource.clip = combatMusic;
        musicSource.Play();
    }
}