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

    private void SpawnEnemy()
    {
        if (spawnPoints == null || spawnPoints.Length == 0 ||
        enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("Assign enemy prefabs and spawn points!");
            return;
        }

        if (availableSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No unused spawn points left in this wave!");
            return;
        }

        // Choose a random unused spawn point.
        int spawnIndex = Random.Range(0, availableSpawnPoints.Count);
        Transform spawnPoint = availableSpawnPoints[spawnIndex];
        availableSpawnPoints.RemoveAt(spawnIndex);

        // Choose a random enemy prefab.
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject chosenEnemy = enemyPrefabs[enemyIndex];

        if (chosenEnemy == null)
        {
            Debug.LogWarning("One of the enemy prefab slots is empty!");
            return;
        }

        Instantiate(chosenEnemy, spawnPoint.position, Quaternion.identity);
        enemiesAlive++;
    }

    public void EnemyDefeated()
    {
        enemiesAlive--;
    }
}
