using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 1.2f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        float y = Random.Range(-4f, 4f);
        Vector2 spawnPos = new Vector2(10f, y);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}