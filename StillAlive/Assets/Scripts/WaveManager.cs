using System.Collections;
using UnityEngine;
using TMPro;

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

    private int currentWaveIndex = 0;
    private int zombiesAlive = 0;
    private bool isIntermission = false;

    private IntermissionManager intermissionManager;

    void Start()
    {
        intermissionManager = FindObjectOfType<IntermissionManager>();
        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (zombiesAlive == 0 && !isIntermission)
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
                Debug.Log("Finally, YOU survive!");
            }
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
        Instantiate(zombiePrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
    }

    public void ZombieDied()
    {
        zombiesAlive--;
    }
}