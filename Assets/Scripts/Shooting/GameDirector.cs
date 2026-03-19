using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public GameObject bossPrefab;
    public float bossSpawnTime = 60f;

    void Start()
    {
        Invoke(nameof(SpawnBoss), bossSpawnTime);
    }

    void SpawnBoss()
    {
        Instantiate(bossPrefab, new Vector2(9f, 0f), Quaternion.identity);
        Debug.Log("Boss出現！！");
    }
}