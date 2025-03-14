using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCore : MonoBehaviour
{
    // 角色控制
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    
    // 谜题
    private Dictionary<int, string> puzzles = new Dictionary<int, string>();

    // 小动物救援
    private List<GameObject> trappedAnimals = new List<GameObject>();
    
    // 敌人系统
    public GameObject enemyPrefab;
    private List<GameObject> enemies = new List<GameObject>();

    // UI 和成就系统
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject achievementsMenu;
    public Text achievementText;
    private List<string> achievements = new List<string>();
    
    // 音效
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioClip collectSound;
    public AudioClip puzzleSolveSound;
    public AudioClip rescueSound;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InitPuzzles();
        SpawnEnemies(3);
    }
    
    void Update()
    {
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        rb.velocity = movement * moveSpeed;
    }
    
    private void InitPuzzles()
    {
        puzzles[1] = "fire";
        puzzles[2] = "water";
        puzzles[3] = "wind";
    }
    
    public bool SolvePuzzle(int puzzleID, string answer)
    {
        if (puzzles.ContainsKey(puzzleID) && puzzles[puzzleID] == answer.ToLower())
        {
            sfxSource.PlayOneShot(puzzleSolveSound);
            return true;
        }
        return false;
    }
    
    public void RescueAnimal(GameObject animal)
    {
        if (trappedAnimals.Contains(animal))
        {
            trappedAnimals.Remove(animal);
            sfxSource.PlayOneShot(rescueSound);
            Debug.Log("Animal Rescued!");
        }
    }
    
    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0), Quaternion.identity);
            enemies.Add(enemy);
        }
    }
    
    public void CollectTreasure()
    {
        achievements.Add("Treasure Hunter");
        achievementText.text = "Achievement Unlocked: Treasure Hunter!";
        sfxSource.PlayOneShot(collectSound);
    }
    
    public void ShowMenu(GameObject menu)
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        achievementsMenu.SetActive(false);
        menu.SetActive(true);
    }
}