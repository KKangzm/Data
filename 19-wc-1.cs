using System;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    // 角色控制
    public class PlayerController : MonoBehaviour
    {
        public float speed = 5f;
        public float jumpForce = 10f;
        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            float move = Input.GetAxis("Horizontal");
            transform.position += new Vector3(move * speed * Time.deltaTime, 0, 0);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
    }

    // 解密系统
    public class PuzzleSystem
    {
        public bool ValidatePuzzle(string puzzleType, string solution)
        {
            Debug.Log($"Validating {puzzleType} with solution: {solution}");
            return true;
        }
    }

    // 资源管理系统
    public class ResourceManager
    {
        private int energyCores = 0;

        public void CollectEnergyCore()
        {
            energyCores++;
            Debug.Log($"Energy Cores Collected: {energyCores}");
        }

        public bool UseEnergyCore()
        {
            if (energyCores > 0)
            {
                energyCores--;
                Debug.Log("Energy Core Used");
                return true;
            }
            return false;
        }
    }

    // 环境互动
    public class EnvironmentInteraction
    {
        public void MoveRock()
        {
            Debug.Log("Rock moved!");
        }

        public void ActivateMechanism()
        {
            Debug.Log("Mechanism activated!");
        }
    }

    // 战斗系统
    public class CombatSystem
    {
        public void Attack()
        {
            Debug.Log("Player attacked!");
        }

        public void Defend()
        {
            Debug.Log("Player defended!");
        }
    }

    // 时间限制
    public class TimerSystem
    {
        public float timeLeft = 60f;

        public void UpdateTimer()
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                Debug.Log($"Time Left: {timeLeft}s");
            }
            else
            {
                Debug.Log("Time's up!");
            }
        }
    }

    // UI界面
    public class UIManager
    {
        public void DisplayStatus(string message)
        {
            Debug.Log($"UI: {message}");
        }
    }

    // 存档与读档
    public class SaveSystem
    {
        public void SaveGame()
        {
            Debug.Log("Game saved!");
        }

        public void LoadGame()
        {
            Debug.Log("Game loaded!");
        }
    }
}