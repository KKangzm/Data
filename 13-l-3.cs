using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;
    public int energy = 50;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"Player takes {amount} damage. Remaining HP: {health}");
        if (health <= 0)
        {
            Debug.Log("Player has died.");
            // 触发游戏失败逻辑
        }
    }

    public void RestoreEnergy(int amount)
    {
        energy += amount;
        Debug.Log($"Energy restored by {amount}. Current Energy: {energy}");
    }
}
