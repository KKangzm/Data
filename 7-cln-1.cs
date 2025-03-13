using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameCore : MonoBehaviour
{
    #region 角色控制系统
    [Header("角色控制")]
    public float moveSpeed = 5f;
    public CharacterController controller;
    private Vector3 playerVelocity;

    void UpdateMovement()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * moveSpeed);
        
        if (move != Vector3.zero)
            transform.forward = move;
    }
    #endregion

    #region 物品交互系统
    [Header("物品管理")]
    public List<Item> inventory = new List<Item>();
    public LayerMask interactableLayer;
    
    class Item {
        public string itemID;
        public Sprite icon;
    }

    void CheckInteractions()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, 
            out RaycastHit hit, 2f, interactableLayer))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Item newItem = hit.collider.GetComponent<Interactable>().Interact();
                if(newItem != null) inventory.Add(newItem);
                UpdateInventoryUI();
            }
        }
    }

    void CombineItems(Item itemA, Item itemB)
    {
        // 物品组合逻辑
        if (itemA.itemID == "Key1" && itemB.itemID == "Key2")
        {
            inventory.Add(new Item(){ itemID = "MasterKey" });
        }
    }
    #endregion

    #region 对话系统
    [System.Serializable]
    public class DialogueNode {
        public string text;
        public List<DialogueOption> options;
    }

    [System.Serializable]
    public class DialogueOption {
        public string text;
        public DialogueNode nextNode;
        public UnityEvent onSelect;
    }

    [Header("对话系统")]
    public DialogueNode currentDialogue;
    public bool inDialogue;

    void StartDialogue(DialogueNode startNode)
    {
        currentDialogue = startNode;
        inDialogue = true;
        ShowDialogueUI();
    }

    void SelectOption(int index)
    {
        if (index < currentDialogue.options.Count)
        {
            currentDialogue.options[index].onSelect.Invoke();
            currentDialogue = currentDialogue.options[index].nextNode;
            UpdateDialogueUI();
        }
    }
    #endregion

    #region 谜题系统
    public interface IPuzzle {
        void InitializePuzzle();
        bool CheckSolution();
        void ProvideHint();
    }

    public class LogicPuzzle : IPuzzle {
        private string correctSequence = "3124";
        private string playerInput = "";

        public bool CheckSolution() => playerInput == correctSequence;

        public void InitializePuzzle() {
            playerInput = "";
        }

        public void ReceiveInput(int number) {
            playerInput += number.ToString();
        }
    }

    public class PhysicsPuzzle : IPuzzle {
        private List<GameObject> activatedSwitches = new List<GameObject>();

        public bool CheckSolution() => activatedSwitches.Count == 3;

        public void InitializePuzzle() {
            activatedSwitches.Clear();
        }

        public void ActivateSwitch(GameObject switchObj) {
            if(!activatedSwitches.Contains(switchObj))
                activatedSwitches.Add(switchObj);
        }
    }
    #endregion

    #region 存档系统
    [System.Serializable]
    class SaveData {
        public Vector3 playerPosition;
        public List<string> inventoryIDs;
        public DialogueNode currentDialogueState;
        public Dictionary<string, bool> puzzleStates;
    }

    void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        
        SaveData data = new SaveData();
        data.playerPosition = transform.position;
        data.inventoryIDs = inventory.ConvertAll(item => item.itemID);
        
        formatter.Serialize(file, data);
        file.Close();
    }

    void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveData data = (SaveData)formatter.Deserialize(file);
            
            transform.position = data.playerPosition;
            inventory = data.inventoryIDs.ConvertAll(id => new Item(){ itemID = id });
        }
    }
    #endregion

    #region UI系统
    [Header("UI管理")]
    public GameObject mainMenu;
    public GameObject dialoguePanel;
    public GameObject inventoryPanel;

    void TogglePauseMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        Time.timeScale = mainMenu.activeSelf ? 0 : 1;
    }

    void UpdateInventoryUI()
    {
        // 更新库存UI显示
    }

    void ShowDialogueUI()
    {
        dialoguePanel.SetActive(true);
    }
    #endregion

    void Update()
    {
        if (!inDialogue)
        {
            UpdateMovement();
            CheckInteractions();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) 
            TogglePauseMenu();
    }
}

// 交互物品基类
public abstract class Interactable : MonoBehaviour
{
    public abstract GameCore.Item Interact();
}

// 示例物品实现
public class KeyItem : Interactable {
    public override GameCore.Item Interact()
    {
        Destroy(gameObject);
        return new GameCore.Item(){ itemID = "Key" };
    }
}