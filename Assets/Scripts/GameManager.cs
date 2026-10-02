using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private static bool skipStartMenu = false;
    public GameObject winPanel;

    [Header("Player")]
    public GameObject player;
    public int maxLives = 3;
    public int currentLives { get; private set; }

    [Header("Respawn")]
    public Transform[] respawnPoints;
    public Transform startPoint;

    [Header("UI")]
    public Text livesText;
    public GameObject startPanel;
    public GameObject gameOverPanel;

    private bool isGameOver = false;
    private bool waterCooldown = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentLives = maxLives;   // always 3 (set maxLives = 3 in the Inspector too)

        if (player == null) player = GameObject.FindGameObjectWithTag("Player");

        if (startPoint == null)
        {
            GameObject sp = new GameObject("StartPoint");
            sp.transform.position = player.transform.position;
            startPoint = sp.transform;
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        UpdateUI();

        if (startPanel != null && !skipStartMenu)
        {
            startPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            if (startPanel != null) startPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        skipStartMenu = false;

        if (winPanel != null) winPanel.SetActive(false);
    }

    void UpdateUI()
    {
        if (livesText != null) livesText.text = "x" + currentLives;
    }

    // ---------- LIFE LOSS (single place everything goes through) ----------
    void LoseLife()
    {
        currentLives--;
        if (currentLives < 0) currentLives = 0;
        UpdateUI();

        if (currentLives <= 0) GameOver();
    }

    // Call from enemies, spikes, etc. Life is lost, no respawn.
    public void DamagePlayer()
    {
        if (isGameOver) return;
        LoseLife();
    }

    // Call from water. Life is lost, then respawn near that water.
    public void PlayerEnteredWater(Transform waterTransform)
    {
        if (isGameOver || waterCooldown) return;   // ignore duplicate trigger calls

        LoseLife();
        if (!isGameOver)
        {
            RespawnPlayer(waterTransform);
            StartCoroutine(WaterCooldown());
        }
    }

    IEnumerator WaterCooldown()
    {
        waterCooldown = true;
        yield return new WaitForSeconds(0.5f);
        waterCooldown = false;
    }

    void RespawnPlayer(Transform waterTransform)
    {
        Transform bestPoint = startPoint;
        float minDistance = Mathf.Infinity;

        if (respawnPoints != null && waterTransform != null)
        {
            foreach (Transform point in respawnPoints)
            {
                float dist = Vector2.Distance(waterTransform.position, point.position);
                if (dist < minDistance) { minDistance = dist; bestPoint = point; }
            }
        }

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) { rb.linearVelocity = Vector2.zero; rb.position = bestPoint.position; }
        player.transform.position = bestPoint.position;
    }

  
    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void StartGame()
    {
        if (startPanel != null) startPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        skipStartMenu = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;          // stops further damage/water life loss
        Time.timeScale = 0f;
        if (winPanel != null) winPanel.SetActive(true);
    }
}