using UnityEngine;

public class PlayerSurvival : MonoBehaviour
{
    public float health = 100f;
    public float hunger = 100f;
    public float stamina = 100f;
    
    private float hungerRate = 0.5f; // 每秒减少饥饿值
    private float staminaRegenRate = 5f; // 体力恢复速度
    
    void Update()
    {
        HandleHunger();
        RegenerateStamina();
    }

    void HandleHunger()
    {
        hunger -= hungerRate * Time.deltaTime;
        if (hunger <= 0)
        {
            hunger = 0;
            TakeDamage(5f * Time.deltaTime); // 饥饿造成伤害
        }
    }

    void RegenerateStamina()
    {
        if (stamina < 100f)
            stamina += staminaRegenRate * Time.deltaTime;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void EatFood(float foodValue)
    {
        hunger += foodValue;
        if (hunger > 100f) hunger = 100f;
    }

    void Die()
    {
        Debug.Log("Player has died!");
        // 触发游戏结束事件
    }
}
