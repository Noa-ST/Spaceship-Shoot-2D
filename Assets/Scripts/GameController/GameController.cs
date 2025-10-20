using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [Header("Spawn Enemy")]
    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] float spawnTime = 2f;
    [SerializeField] float minSpawnTime = 0.5f;
    [SerializeField] float spawnAccelerationRate = 0.05f;
    [SerializeField] float spawnAccelerationInterval = 5f;
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
    // Đã xóa: bool _isGameOver; // Chỉ sử dụng state để quản lý trạng thái
    int _score;

    public int Score { get => _score; }
    private GameState _state;
    public GameState state
    {
        get => _state;
        set
        {
            Debug.Log($"Trạng thái game thay đổi từ {_state} sang {value}");
            _state = value;
        }
    }

    public override void Awake()
    {
        MakeSingleton(false);
        state = GameState.Playing;
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            Debug.LogError("Danh sách enemyPrefabs trống hoặc null!");
        }
    }

    public override void Start()
    {
        _spawnTime = spawnTime;
        AudioController.Ins?.PlayBackgroundMusic();
    }

    void Update()
    {
        // **QUAN TRỌNG: Chỉ sinh enemy khi trạng thái là Playing.**
        if (state != GameState.Playing)
        {
            // Debug.Log("Game đã dừng, không sinh enemy!"); 
            return;
        }

        timeSinceStart += Time.deltaTime;
        difficultyTimer += Time.deltaTime;
        spawnAcceleratorTimer += Time.deltaTime;

        // Tăng số lượng enemy spawn
        if (difficultyTimer >= difficultyIncreaseInterval && enemiesPerSpawn < maxEnemiesPerSpawn)
        {
            enemiesPerSpawn++;
            difficultyTimer = 0;
        }

        // Giảm thời gian giữa các lần spawn
        if (spawnAcceleratorTimer >= spawnAccelerationInterval && spawnTime > minSpawnTime)
        {
            spawnTime -= spawnAccelerationRate;
            spawnTime = Mathf.Max(spawnTime, minSpawnTime);
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
        // Kiểm tra trạng thái lần nữa trước khi sinh.
        if (state != GameState.Playing)
        {
            // Debug.Log("Không sinh enemy vì game đã kết thúc!");
            return;
        }

        float ranXpos = Random.Range(-7f, 7f);
        Vector2 spawnPos = new Vector2(ranXpos, 7);

        if (enemyPrefabs != null && enemyPrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject randomEnemy = enemyPrefabs[randomIndex];
            if (randomEnemy != null)
            {
                Instantiate(randomEnemy, spawnPos, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Enemy prefab tại index " + randomIndex + " là null!");
            }
        }
    }

    public void TrySpawnPowerUp(Vector2 position)
    {
        foreach (var powerUp in powerUpDrops)
        {
            if (powerUp.powerUpPrefab == null) continue;
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
        // Giả định Pref.BestScore được định nghĩa ở đâu đó
        // Bạn nên kiểm tra xem Pref có được khởi tạo và chứa phương thức setter này không.
        // Pref.BestScore = _score; 

        if (UIManager.Ins)
        {
            UIManager.Ins.UpdateScore(_score);
        }
        else
        {
            Debug.LogWarning("UIManager không tồn tại!");
        }
    }

    public void SetGameOver()
    {
        Debug.Log("SetGameOver được gọi!");
        // **Đặt trạng thái Game Over ngay lập tức.**
        state = GameState.GameOver; 

        _spawnTime = 0;
        DestroyAllWithTag(GameTag.Enemy.ToString());
        DestroyAllWithTag(GameTag.Player.ToString());
        Time.timeScale = 0f;

        if (UIManager.Ins)
        {
            UIManager.Ins.gameoverDialog.Show(true);
        }
        else
        {
            Debug.LogWarning("UIManager không tồn tại!");
        }
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
        Debug.Log($"Hủy {objects.Length} đối tượng với tag {tag}");
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
    }

    public void ResetGame()
    {
        _score = 0;
        timeSinceStart = 0;
        difficultyTimer = 0;
        spawnAcceleratorTimer = 0;
        enemiesPerSpawn = 1;
        spawnTime = 2f;
        _spawnTime = spawnTime;
        // Đã xóa: _isGameOver = false;
        state = GameState.Playing;
        Time.timeScale = 1f;
    }
}