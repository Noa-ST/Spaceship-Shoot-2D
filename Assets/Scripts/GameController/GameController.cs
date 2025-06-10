using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
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

    float _spawnTime;

    float timeSinceStart;
    float difficultyTimer;
    float spawnAcceleratorTimer;

    [Header("Power Up")]
    [SerializeField] private List<PowerUpDrop> powerUpDrops;


    [Header("UI")]
    bool _isGameOver;
    int _score;

    public int Score { get => _score; }
    public GameState state;

    public override void Awake()
    {
        MakeSingleton(false);
        state = GameState.Playing;
    }

    public override void Start()
    {
        _spawnTime = 0;
        AudioController.Ins.PlayBackgroundMusic();
    }

    void Update()
    {
        if (state == GameState.GameOver)
            return;

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
        _spawnTime -= Time.deltaTime;

        if (_spawnTime <= 0)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                SpawnEnemy();
            }

            _spawnTime = spawnTime;
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

    public void ScoreIncrement(int value)
    {
        if (state != GameState.Playing) return;

        _score += value;
        Pref.BestScore = _score;

        if (UIManager.Ins)
        {
            UIManager.Ins.UpdateScore(_score);
        }
    }

    public void SetGameOver()
    {
        if (state == GameState.GameOver)
            return;

        state = GameState.GameOver;
        DestroyAllWithTag(GameTag.Enemy.ToString());
        DestroyAllWithTag(GameTag.Player.ToString()); 
        Time.timeScale = 0f;
        UIManager.Ins?.gameoverDialog.Show(true);
    }
    public void PauseGame()
    {
        state = GameState.Paused;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        state = GameState.Playing;
        Time.timeScale = 1f;
    }

    private void DestroyAllWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
    }
}
