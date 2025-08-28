using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    public static SpawnManager Instance;
    
    [Header("Enemy Spawn Settings")]
    [SerializeField] private List<BaseEnemy> enemyPrefabs = new();
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float minSpawnTime = 1f;
    [SerializeField] private float maxSpawnTime = 3f;
    [SerializeField] private int maximumEnemies = 50;
    
    [Header("Powerup Settings")]
    [SerializeField] private List<BasePowerUp> powerUps = new ();
    
    [Header("Wave System")]
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int maxWave = 10;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float timeBetweenWaves = 2f;
    
    private int _enemiesRemainingInWave;
    private int _enemiesAlive;
    private Transform _player;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    
    private void Start()
    {
        StartCoroutine(GameLoop());
    }
    
    private IEnumerator GameLoop()
    {

        while (currentWave < maxWave)
        {
            currentWave++;
        
            _enemiesRemainingInWave = enemiesPerWave + (currentWave * 2);
        
            if (UiManager.Instance) UiManager.Instance.UpdateWaveText(currentWave);
            
            yield return new WaitForSeconds(timeBetweenWaves);
            
            yield return StartCoroutine(SpawnEnemies());
            
            yield return new WaitUntil(() => _enemiesAlive <= 0);
        }
    }
    
    private IEnumerator SpawnEnemies()
    {
        while (_enemiesRemainingInWave > 0 && _enemiesAlive < maximumEnemies)
        {
            SpawnEnemy();
            _enemiesRemainingInWave--;
            _enemiesAlive++;
            
            float spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnEnemy()
    {
        if(!_player || enemyPrefabs.Count == 0) return;
        
        Vector2 spawnDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = _player.position + new Vector3(spawnDirection.x, spawnDirection.y, 0) * spawnRadius;
        
        BaseEnemy enemy = enemyPrefabs[Random.Range(0,enemyPrefabs.Count)];
        
        Instantiate(enemy, spawnPosition, Quaternion.identity);
    }
    
    public void EnemyDestroyed()
    {
        _enemiesAlive--;
    }
    
    public void SpawnPowerup(Vector3 position)
    {
        int randomIndex = Random.Range(0, powerUps.Count);
        Instantiate(powerUps[randomIndex], position, Quaternion.identity);
    }
}
