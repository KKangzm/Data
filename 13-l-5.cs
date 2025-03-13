using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int health = 50;
    public int attackDamage = 10;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"{enemyName} takes {damage} damage. HP left: {health}");

        if (health <= 0)
        {
            Debug.Log($"{enemyName} is defeated!");
            Destroy(gameObject);
        }
    }

    public void AttackPlayer(Player player)
    {
        player.TakeDamage(attackDamage);
        Debug.Log($"{enemyName} attacks the player for {attackDamage} damage!");
    }
}
