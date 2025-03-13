using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameCore : MonoBehaviour
{
    #region 角色控制系统
    [Header("角色控制")]
    public CharacterController controller;
    public float walkSpeed = 5f;
    public float climbSpeed = 3f;
    public float jumpForce = 4f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isClimbing;
    private bool isGrounded;
    private float groundDistance = 0.4f;

    void UpdateMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * (isClimbing ? climbSpeed : walkSpeed) * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    #endregion

    #region 交互系统
    [Header("交互设置")]
    public float interactRange = 3f;
    public LayerMask interactableMask;
    public Inventory inventory = new Inventory();

    void CheckInteraction()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, 
            out RaycastHit hit, interactRange, interactableMask))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact(this);
                }
            }
        }
    }

    public class Inventory
    {
        public List<Item> items = new List<Item>();

        public void AddItem(Item item)
        {
            items.Add(item);
            CheckAchievement("collector", items.Count);
        }
    }

    public interface IInteractable
    {
        void Interact(GameCore player);
    }
    #endregion

    #region 谜题系统
    [Header("谜题管理")]
    public List<Puzzle> activePuzzles = new List<Puzzle>();

    public abstract class Puzzle : MonoBehaviour
    {
        public bool isSolved;
        public abstract void CheckSolution();
    }

    public class PressurePlatePuzzle : Puzzle
    {
        public float requiredWeight;
        private float currentWeight;

        public override void CheckSolution()
        {
            isSolved = currentWeight >= requiredWeight;
        }

        void OnTriggerEnter(Collider other)
        {
            currentWeight += other.GetComponent<Rigidbody>().mass;
            CheckSolution();
        }
    }
    #endregion

    #region 天气系统
    public enum WeatherType { Sunny, Rainy, Foggy, Stormy }
    
    [Header("天气设置")]
    public WeatherType currentWeather;
    public float weatherChangeInterval = 120f;
    private float weatherTimer;

    void UpdateWeather()
    {
        weatherTimer += Time.deltaTime;
        if (weatherTimer >= weatherChangeInterval)
        {
            ChangeRandomWeather();
            weatherTimer = 0;
        }
    }

    void ChangeRandomWeather()
    {
        Array values = Enum.GetValues(typeof(WeatherType));
        currentWeather = (WeatherType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        ApplyWeatherEffects();
    }

    void ApplyWeatherEffects()
    {
        switch(currentWeather)
        {
            case WeatherType.Rainy:
                RenderSettings.fogDensity = 0.02f;
                break;
            case WeatherType.Foggy:
                RenderSettings.fogDensity = 0.05f;
                break;
            case WeatherType.Stormy:
                RenderSettings.fogDensity = 0.03f;
                break;
            default:
                RenderSettings.fogDensity = 0.01f;
                break;
        }
    }
    #endregion

    #region 存档系统
    [System.Serializable]
    class SaveData
    {
        public Vector3 playerPosition;
        public List<string> inventoryItems;
        public List<bool> puzzleStates;
        public List<string> unlockedAchievements;
    }

    public void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");

        SaveData data = new SaveData
        {
            playerPosition = transform.position,
            inventoryItems = inventory.items.ConvertAll(i => i.name),
            puzzleStates = activePuzzles.ConvertAll(p => p.isSolved),
            unlockedAchievements = AchievementManager.GetUnlockedAchievements()
        };

        formatter.Serialize(file, data);
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveData data = (SaveData)formatter.Deserialize(file);

            transform.position = data.playerPosition;
            // 恢复其他数据...
        }
    }
    #endregion

    #region 成就系统
    public static class AchievementManager
    {
        private static HashSet<string> unlocked = new HashSet<string>();

        public static void UnlockAchievement(string id)
        {
            if (!unlocked.Contains(id))
            {
                unlocked.Add(id);
                Debug.Log($"成就解锁: {id}");
                SaveSystem.SaveAchievements();
            }
        }

        public static void CheckAchievement(string condition, int value)
        {
            if (condition == "collector" && value >= 10)
            {
                UnlockAchievement("item_master");
            }
        }
    }
    #endregion

    void Update()
    {
        UpdateMovement();
        CheckInteraction();
        UpdateWeather();

        if (Input.GetKeyDown(KeyCode.F5)) SaveGame();
        if (Input.GetKeyDown(KeyCode.F9)) LoadGame();
    }

    void FixedUpdate()
    {
        foreach (var puzzle in activePuzzles)
        {
            puzzle.CheckSolution();
        }
    }
}

// 物品基类
public class Item : MonoBehaviour
{
    public string itemName;
    public void Pickup(GameCore player)
    {
        player.inventory.AddItem(this);
        gameObject.SetActive(false);
    }
}

// 示例交互物品
public class Chest : MonoBehaviour, GameCore.IInteractable
{
    public Item containedItem;

    public void Interact(GameCore player)
    {
        if (containedItem != null)
        {
            containedItem.Pickup(player);
            GameCore.AchievementManager.UnlockAchievement("first_chest");
        }
    }
}