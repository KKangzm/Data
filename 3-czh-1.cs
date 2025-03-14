// 角色控制系统
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 4f;
    public float gravity = -9.81f;
    public float interactionDistance = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleInteraction();
    }

    private void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                interactable?.Interact();
            }
        }
    }
}

// 谜题系统基类
public abstract class PuzzleBase : MonoBehaviour, IInteractable
{
    public UnityEvent OnPuzzleSolved;
    public bool IsSolved { get; protected set; }

    public abstract void Interact();
    protected abstract bool CheckSolution();
}

// 密码锁谜题
public class CombinationLockPuzzle : PuzzleBase
{
    public string correctCombination = "314";
    private string currentInput = "";

    public override void Interact()
    {
        if (IsSolved) return;
        
        UIManager.Instance.ShowNumberPad((input) => 
        {
            currentInput = input;
            if (CheckSolution())
            {
                IsSolved = true;
                OnPuzzleSolved.Invoke();
            }
        });
    }

    protected override bool CheckSolution()
    {
        return currentInput == correctCombination;
    }
}

// 资源管理系统
public class ResourceManager : Singleton<ResourceManager>
{
    public enum ResourceType { Food, Water, Metal, Medicine }

    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    public event Action<ResourceType, int> OnResourceChanged;

    public void AddResource(ResourceType type, int amount)
    {
        resources[type] = GetResourceCount(type) + amount;
        OnResourceChanged?.Invoke(type, resources[type]);
    }

    public bool ConsumeResource(ResourceType type, int amount)
    {
        if (GetResourceCount(type) >= amount)
        {
            resources[type] -= amount;
            OnResourceChanged?.Invoke(type, resources[type]);
            return true;
        }
        return false;
    }

    public int GetResourceCount(ResourceType type)
    {
        return resources.ContainsKey(type) ? resources[type] : 0;
    }
}

// 存档系统
public class SaveSystem : MonoBehaviour
{
    private const string SAVE_KEY = "GameSave";
    
    [System.Serializable]
    private class SaveData
    {
        public Vector3 playerPosition;
        public Quaternion playerRotation;
        public Dictionary<ResourceType, int> resources;
        public List<string> solvedPuzzles;
    }

    public void SaveGame()
    {
        var saveData = new SaveData
        {
            playerPosition = PlayerController.Instance.transform.position,
            playerRotation = PlayerController.Instance.transform.rotation,
            resources = ResourceManager.Instance.GetAllResources(),
            solvedPuzzles = PuzzleManager.Instance.GetSolvedPuzzleIDs()
        };

        string jsonData = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SAVE_KEY, jsonData);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

            // 恢复游戏状态
            PlayerController.Instance.transform.SetPositionAndRotation(
                saveData.playerPosition, saveData.playerRotation);
            ResourceManager.Instance.LoadResources(saveData.resources);
            PuzzleManager.Instance.LoadSolvedPuzzles(saveData.solvedPuzzles);
        }
    }
}

// 辅助类和接口
public interface IInteractable
{
    void Interact();
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}