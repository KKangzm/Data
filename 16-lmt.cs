csharp
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacter : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded = true;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }

        // Animation states
        if (move > 0)
        {
            animator.SetBool("isRunning", true);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (move < 0)
        {
            animator.SetBool("isRunning", true);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

public class EnvironmentInteraction : MonoBehaviour
{
    public GameObject stone;
    public GameObject lever;
    public GameObject torch;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithEnvironment();
        }
    }

    void InteractWithEnvironment()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 2f);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject == stone)
            {
                stone.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 500f);
            }
            else if (hit.collider.gameObject == lever)
            {
                lever.GetComponent<Lever>().PullLever();
            }
            else if (hit.collider.gameObject == torch)
            {
                torch.GetComponent<Torch>().LightUp();
            }
        }
    }
}

public class PuzzleLogic : MonoBehaviour
{
    public List<GameObject> puzzles;
    public GameObject finalDoor;

    void Update()
    {
        bool allPuzzlesSolved = true;
        foreach (GameObject puzzle in puzzles)
        {
            if (!puzzle.GetComponent<Puzzle>().IsSolved())
            {
                allPuzzlesSolved = false;
                break;
            }
        }

        if (allPuzzlesSolved)
        {
            finalDoor.GetComponent<Door>().Open();
        }
    }
}

public class LevelManager : MonoBehaviour
{
    public List<GameObject> levels;
    private int currentLevelIndex = 0;

    public void LoadNextLevel()
    {
        if (currentLevelIndex < levels.Count - 1)
        {
            levels[currentLevelIndex].SetActive(false);
            currentLevelIndex++;
            levels[currentLevelIndex].SetActive(true);
        }
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevelIndex);
    }

    public void LoadProgress()
    {
        currentLevelIndex = PlayerPrefs.GetInt("CurrentLevel", 0);
        for (int i = 0; i <= currentLevelIndex; i++)
        {
            levels[i].SetActive(i == currentLevelIndex);
        }
    }
}

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 1f;
    public float speed = 3f;
    private bool isAttacking = false;

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && !isAttacking)
        {
            ChasePlayer();
        }
        else if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void AttackPlayer()
    {
        isAttacking = true;
        // Attack logic here
        Invoke("ResetAttack", 1f); // Reset after 1 second
    }

    void ResetAttack()
    {
        isAttacking = false;
    }
}

public class BossBattle : MonoBehaviour
{
    public GameObject boss;
    public int health = 100;
    public List<GameObject> attacks;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        boss.SetActive(false);
        // Win condition or next level
    }

    public void PerformAttack(int attackIndex)
    {
        attacks[attackIndex].GetComponent<Attack>().Activate();
    }
}

public class AudioController : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public List<AudioSource> soundEffects;

    void Start()
    {
        backgroundMusic.Play();
    }

    public void PlaySoundEffect(int index)
    {
        if (index >= 0 && index < soundEffects.Count)
        {
            soundEffects[index].Play();
        }
    }
}

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void ShowSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
}

public class SaveLoadSystem : MonoBehaviour
{
    public void SaveGame()
    {
        // Save game logic
    }

    public void LoadGame()
    {
        // Load game logic
    }
}
