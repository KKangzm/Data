using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 在此处处理物品拾取逻辑，例如增加玩家的物品栏
            Debug.Log("Item collected!");
            Destroy(gameObject);
        }
    }
}
