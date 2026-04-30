using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public int zombieCount;
        public float spawnRate;
    }

    [Header("Wave Setting")]
    public Wave[] waves;
    public Transform[] spawnPoints;
    public GameObject zombiePrefab;

    [Header("UI")]
    public TextMeshProUGUI waveTextUI;

    [Header("Timer")]
    public TextMeshProUGUI timerTextUI;
    public GameObject endGamePanel;
    public TextMeshProUGUI finalTimeText;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverTimeText;

    private int currentWaveIndex = 0;
    private int zombiesAlive = 0;
    private bool isIntermission = false;

    private float gameTimer = 0f;
    private bool isGameActive = true;

    private IntermissionManager intermissionManager;

    void Start()
    {
        intermissionManager = FindFirstObjectByType<IntermissionManager>();
        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (isGameActive)
        {
            gameTimer += Time.deltaTime;
            UpdateTimerUI();
        }

        if (zombiesAlive <= 0 && !isIntermission)
        {
            isIntermission = true;
            currentWaveIndex++;

            if (currentWaveIndex < waves.Length)
            {
                if(intermissionManager != null)
                {
                    intermissionManager.ShowIntermission();
                }
            }
            else
            {
                EndGame();
            }
        }
    }

    void UpdateTimerUI()
    {
        if (timerTextUI != null)
        {
            int minutes = Mathf.FloorToInt(gameTimer / 60F);
            int seconds = Mathf.FloorToInt(gameTimer - minutes * 60);
            timerTextUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StartNextWave()
    {
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        isIntermission = true;
        Wave currentWave = waves[currentWaveIndex];
        
        if (waveTextUI != null)
        {
            waveTextUI.text = currentWave.waveName;
        }

        yield return new WaitForSeconds(3f);
        
        isIntermission = false;
        zombiesAlive = currentWave.zombieCount;

        for (int i = 0; i < currentWave.zombieCount; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(currentWave.spawnRate);
        }
    }

    void SpawnZombie()
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject spawnedZombie = Instantiate(zombiePrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);

        spawnedZombie.GetComponent<ZombieController>().ApplyWaveBuff(currentWaveIndex);
    }

    public void ZombieDied()
    {
        zombiesAlive--;
    }

    public void EndGame()
    {
        isGameActive = false;
        Time.timeScale = 0f;
        endGamePanel.SetActive(true);

        int minutes = Mathf.FloorToInt(gameTimer / 60F);
        int seconds = Mathf.FloorToInt(gameTimer - minutes * 60);
        finalTimeText.text = "Survival Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        int minutes = Mathf.FloorToInt(gameTimer / 60F);
        int seconds = Mathf.FloorToInt(gameTimer - minutes * 60);
        gameOverTimeText.text = "Survival Time : " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void GoToCreditsScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Credits");
    }
}