using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUpDrop
{
    public GameObject powerUpPrefab;
    [Range(0f, 1f)] public float dropChance;
}

public class GameController : MonoBehaviour
{
    [Header("Spawn Enemy")]
    [SerializeField] List<GameObject> enemyPrefabs;

    [SerializeField] float spawnTime = 2f;
    [SerializeField] float minSpawnTime = 0.5f; // tốc độ spawn nhanh nhất
    [SerializeField] float spawnAccelerationRate = 0.05f; // giảm spawnTime mỗi lần
    [SerializeField] float spawnAccelerationInterval = 5f; // mỗi 5s sẽ giảm tốc độ spawn

    [SerializeField] int enemiesPerSpawn = 1;
    [SerializeField] int maxEnemiesPerSpawn = 5;
    [SerializeField] float difficultyIncreaseInterval = 10f;

    float m_spawnTime;
    bool m_isGameOver;

    float timeSinceStart;
    float difficultyTimer;
    float spawnAcceleratorTimer;

    [Header("Power Up")]
    [SerializeField] private List<PowerUpDrop> powerUpDrops;


    [Header("UI")]
    int m_score;
    UIManager uim;
    AudioSource aus;
    [SerializeField] AudioClip gameOverSound;

    void Start()
    {
        m_spawnTime = 0;
        uim = FindObjectOfType<UIManager>();
        aus = FindObjectOfType<AudioSource>();
        uim.SetScoreText("Score: " + m_score);
    }

    void Update()
    {
        if (m_isGameOver)
        {
            m_spawnTime = 0;
            aus.PlayOneShot(gameOverSound);
            uim.ShowGameOverPanel(true);
            return;
        }

        timeSinceStart += Time.deltaTime;
        difficultyTimer += Time.deltaTime;
        spawnAcceleratorTimer += Time.deltaTime;

        // tăng số lượng enemy spawn
        if (difficultyTimer >= difficultyIncreaseInterval && enemiesPerSpawn < maxEnemiesPerSpawn)
        {
            enemiesPerSpawn++;
            difficultyTimer = 0;
        }

        // giảm thời gian giữa các lần spawn
        if (spawnAcceleratorTimer >= spawnAccelerationInterval && spawnTime > minSpawnTime)
        {
            spawnTime -= spawnAccelerationRate;
            spawnTime = Mathf.Max(spawnTime, minSpawnTime); // không nhỏ hơn giới hạn
            spawnAcceleratorTimer = 0;
        }
        m_spawnTime -= Time.deltaTime;

        if (m_spawnTime <= 0)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                SpawnEnemy();
            }

            m_spawnTime = spawnTime;
        }
    }

    public void SpawnEnemy()
    {
        float ranXpoxs = Random.Range(-7f, 7f);
        Vector2 spawnPos = new Vector2(ranXpoxs, 7);

        if (enemyPrefabs != null && enemyPrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject randomEnemy = enemyPrefabs[randomIndex];
            Instantiate(randomEnemy, spawnPos, Quaternion.identity);
        }
    }

    public void TrySpawnPowerUp(Vector2 position)
    {
        foreach (var powerUp in powerUpDrops)
        {
            float roll = Random.Range(0f, 1f);
            if (roll <= powerUp.dropChance)
            {
                Instantiate(powerUp.powerUpPrefab, position, Quaternion.identity);
                break; 
            }
        }
    }


    public void SetScore(int value)
    {
        m_score = value;
    }

    public int GetScore()
    {
        return m_score;
    }

    public void ScoreIncrement(int value)
    {
        if (m_isGameOver)
            return;

        m_score += value;
        uim.SetScoreText("Score: " + m_score);
    }

    public void SetGameOverState(bool state)
    {
        m_isGameOver = state;
    }

    public bool IsGameOver()
    {
        return m_isGameOver;
    }

}
