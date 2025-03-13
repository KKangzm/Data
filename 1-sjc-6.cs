// InteractableItem.cs
public class InteractableItem : MonoBehaviour
{
    public ItemType itemType; // 枚举类型：钥匙/文件/药品等
    public string promptText = "按E拾取";
    
    private bool isInRange;

    void Update(){
        if(isInRange && Input.GetKeyDown(KeyCode.E)){
            HandleInteraction();
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            UIManager.Instance.ShowPrompt(promptText);
            isInRange = true;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            UIManager.Instance.HidePrompt();
            isInRange = false;
        }
    }

    void HandleInteraction(){
        InventoryManager.Instance.AddItem(itemType);
        Destroy(gameObject);
    }
}