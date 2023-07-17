using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;
    public float spawnRadius; 
    public GameObject[] enemyPrefabs;
    public int maxEnemyCount = 5; 

    private List<GameObject> enemyPool = new List<GameObject>();
    private int currentEnemyCount = 0; 

    private void Start()
    {
        InitializeEnemyPool();
    }

    private void Update()
    {
        if (currentEnemyCount < maxEnemyCount) 
        {
            StartCoroutine(SpawnEnemyCoroutine());
        }
    }

    private void InitializeEnemyPool()
    {
        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            for (int i = 0; i < maxEnemyCount; i++)
            {
                GameObject newEnemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
                newEnemy.SetActive(false);
                enemyPool.Add(newEnemy);
            }
        }
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        int enemyCountToSpawn = Mathf.Min(maxEnemyCount - currentEnemyCount, 1);

        for (int i = 0; i < enemyCountToSpawn; i++)
        {
            GameObject enemy = GetPooledEnemy();
            if (enemy == null)
            {
                Debug.LogWarning("No available enemy in the pool!");
                yield break;
            }

            NavMeshHit navMeshHit;
            Vector3 randomPosition = RandomPointInCircle(spawnPoint.position, spawnRadius);
            if (NavMesh.SamplePosition(randomPosition, out navMeshHit, 10f, NavMesh.AllAreas))
            {
                enemy.transform.position = navMeshHit.position;
                enemy.SetActive(true);
                currentEnemyCount++; 

                
                HealthController healthController = enemy.GetComponent<HealthController>();
                healthController.OnDeath += DecreaseCurrentEnemyCount;
            }

            yield return null;
        }
    }

    private GameObject GetPooledEnemy()
    {
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeSelf)
            {
                return enemy;
            }
        }

        return null;
    }

    private void DecreaseCurrentEnemyCount()
    {
        currentEnemyCount--;
    }

    private Vector3 RandomPointInCircle(Vector3 center, float radius)
    {
        Vector2 randomCirclePoint = Random.insideUnitCircle.normalized * radius;
        Vector3 randomPosition = center + new Vector3(randomCirclePoint.x, 0f, randomCirclePoint.y);
        return randomPosition;
    }
}
