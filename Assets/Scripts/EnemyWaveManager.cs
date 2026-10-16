using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform[] spawnPoints;

    [SerializeField] int startingEnemies = 1;
    [SerializeField] int enemiesPerWave = 1;
    [SerializeField] float spawnDelay = 1f;
    [SerializeField] float timeBetweenWaves = 3f;

    int currentWave = 0;
    int enemiesAlive = 0;

    List<Transform> availableSpawnPoints = new List<Transform>();

    Dictionary<GameObject, Queue<GameObject>> enemyPools = new Dictionary<GameObject, Queue<GameObject>>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            currentWave++;
            int enemiesToSpawn = startingEnemies + (currentWave - 1) * enemiesPerWave;

            availableSpawnPoints.Clear();
            availableSpawnPoints.AddRange(spawnPoints);

            yield return StartCoroutine(SpawnWave(enemiesToSpawn));

            while (enemiesAlive > 0)
            {
                yield return null;
            }
        }
    }

    IEnumerator SpawnWave(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints == null || spawnPoints.Length == 0 ||
            enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("Assign enemy prefabs and spawn points!");
            return;
        }

        if (availableSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No unused spawn points left!");
            return;
        }

        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject chosenPrefab = enemyPrefabs[enemyIndex];

        if (chosenPrefab == null)
        {
            Debug.LogWarning("An enemy prefab slot is empty!");
            return;
        }

        int spawnIndex = Random.Range(0, availableSpawnPoints.Count);
        Transform spawnPoint = availableSpawnPoints[spawnIndex];

        availableSpawnPoints.RemoveAt(spawnIndex);

        // Create a pool for this enemy type if needed.
        if (!enemyPools.ContainsKey(chosenPrefab))
        {
            enemyPools[chosenPrefab] = new Queue<GameObject>();
        }

        Queue<GameObject> pool = enemyPools[chosenPrefab];

        GameObject enemyObject;

        if (pool.Count > 0)
        {
            // Reuse an inactive enemy.
            enemyObject = pool.Dequeue();
        }
        else
        {
            // Create a new enemy only when the pool is empty.
            enemyObject = Instantiate(chosenPrefab);
            enemyObject.SetActive(false);

            EnemyScript newEnemy = enemyObject.GetComponent<EnemyScript>();

            if (newEnemy == null)
            {
                Debug.LogError("Enemy prefab needs an EnemyScript!");
                Destroy(enemyObject);
                return;
            }

            newEnemy.SetPoolInfo(this, chosenPrefab);
        }

        enemyObject.transform.position = spawnPoint.position;
        enemyObject.transform.rotation = Quaternion.identity;

        EnemyScript enemy = enemyObject.GetComponent<EnemyScript>();
        enemy.ResetForSpawn();

        enemyObject.SetActive(true);
        enemiesAlive++;
    }

    public void ReturnEnemy(EnemyScript enemy)
    {
        if (enemy == null)
        {
            return;
        }

        enemiesAlive--;

        GameObject prefab = enemy.PrefabSource;

        // Deactivate the enemy instead of destroying it.
        enemy.gameObject.SetActive(false);

        if (prefab != null && enemyPools.ContainsKey(prefab))
        {
            enemyPools[prefab].Enqueue(enemy.gameObject);
        }
    }

    public void EnemyDefeated()
    {
        enemiesAlive--;
    }
}
