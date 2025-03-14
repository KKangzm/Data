using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCore : MonoBehaviour
{
    // 角色控制
    public CharacterController controller;
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    private Vector3 velocity;
    private bool isGrounded;
    
    // 环境互动
    public void InteractWithObject(GameObject obj)
    {
        Debug.Log("Interacting with " + obj.name);
    }
    
    // 解谜逻辑
    public bool SolvePuzzle(string puzzleType, string solution)
    {
        Debug.Log("Solving " + puzzleType + " with solution: " + solution);
        return true;
    }
    
    // 关卡管理
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // 敌人AI
    public class EnemyAI
    {
        public string state = "Patrolling";
        public void Attack()
        {
            Debug.Log("Enemy is attacking!");
        }
    }
    
    // Boss战
    public class Boss
    {
        public int health = 500;
        public void PerformAttack()
        {
            Debug.Log("Boss performs a powerful attack!");
        }
    }
    
    // 音效与音乐
    public AudioSource audioSource;
    public AudioClip backgroundMusic;
    public void PlayMusic()
    {
        audioSource.clip = backgroundMusic;
        audioSource.Play();
    }
    
    // 用户界面
    public void ShowMainMenu()
    {
        Debug.Log("Showing Main Menu");
    }
    
    public void ShowPauseMenu()
    {
        Debug.Log("Game Paused");
    }
    
    // 存档与读档
    public void SaveGame()
    {
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Game Saved");
    }
    
    public void LoadGame()
    {
        int savedLevel = PlayerPrefs.GetInt("Level", 0);
        SceneManager.LoadScene(savedLevel);
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        float move = Input.GetAxis("Horizontal");
        controller.Move(new Vector3(move, 0, 0) * moveSpeed * Time.deltaTime);
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpForce;
        }
        
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}