using UnityEngine;
using System.Collections.Generic;

public class StageController : MonoBehaviour
{
    public List<StageEvent> events = new List<StageEvent>();
    public GameObject[] enemyPrefabs; // ランダムに出現させる敵のプレハブ配列
    public GameObject bossPrefab; // ボスのプレハブ
    public float specialWaveStartTime = 60f; // 特別なウェーブ開始時間（秒）
    public float specialWaveInterval = 2f; // 敵発射の間隔（秒）
    public int enemiesPerWave = 3; // 1回の発射時に出現させる敵の数
    public float bossSpawnInterval = 40f; // ボス発射の間隔（秒）
    public float intervalDecrement = 0.1f; // ボス発射時のインターバル削減量（秒）
    public float bossSpawnSpeedIncrease = 5f; // ボス発射時の速度増加量（秒）
    public float hpIncreaseMultiplier = 3f; // ボス発射時のHP倍率増加（1.2倍ずつ増加）

    float timer = 0f;
    int index = 0;
    float specialWaveTimer = 0f;
    bool specialWaveStarted = false;
    float nextBossSpawnTime = 80f; // 次のボス発射時間
    float enemyHPMultiplier = 2f; // 敵のHP倍率

    void Update()
    {
        timer += Time.deltaTime;

        // 通常のステージイベント
        if (index < events.Count && timer >= events[index].time)
        {
            Spawn(events[index]);
            index++;
        }

        // 60秒後の特別なウェーブ
        if (!specialWaveStarted && timer >= specialWaveStartTime)
        {
            specialWaveStarted = true;
            specialWaveTimer = 0f;
        }

        if (specialWaveStarted)
        {
            specialWaveTimer += Time.deltaTime;

            // 2秒ごとにランダムな敵を3体発射
            if (specialWaveTimer >= specialWaveInterval)
            {
                SpawnRandomEnemies();
                specialWaveTimer -= specialWaveInterval;
            }
        }

        // ボス発射（60秒ごと）
        if (timer >= nextBossSpawnTime)
        {
            SpawnBoss();
            nextBossSpawnTime += bossSpawnInterval;
            // スポーン間隔を削減
            specialWaveInterval -= intervalDecrement;
            bossSpawnInterval -= bossSpawnSpeedIncrease;
            if (bossSpawnInterval < 5f) // 最小間隔を5秒に制限
                bossSpawnInterval = 5f;
            if (specialWaveInterval < 0.5f) // 最小間隔を0.5秒に制限
                specialWaveInterval = 0.5f;
            enemiesPerWave += 1; // 発射数を1体増加
        }
    }

    void Spawn(StageEvent e)
    {
        Instantiate(e.enemyPrefab, e.position, Quaternion.identity);
    }

    void SpawnRandomEnemies()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("敵のプレハブが設定されていません");
            return;
        }

        // enemiesPerWave個の敵をランダムに発射
        for (int i = 0; i < enemiesPerWave; i++)
        {
            // ランダムなプレハブを選択
            GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // ランダムなY座標で出現
            float y = Random.Range(-3f, 3f);
            Vector2 spawnPos = new Vector2(10f, y);

            GameObject spawnedEnemy = Instantiate(randomPrefab, spawnPos, Quaternion.identity);
            
            // 敵のHP倍率を適用
            ApplyHPMultiplier(spawnedEnemy);
        }

        Debug.Log("敵を3体発射しました（HP倍率: " + enemyHPMultiplier + "）");
    }

    void ApplyHPMultiplier(GameObject enemy)
    {
        // EnemyMovement派生クラスを探してHPを増加させる
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            // SetHealthメソッドを動的に呼び出す
            var setHealthMethod = enemyMovement.GetType().GetMethod("SetHealth");
            if (setHealthMethod != null)
            {
                // 現在のhealthフィールドの値を取得
                var healthField = enemyMovement.GetType().GetField("health");
                if (healthField != null)
                {
                    int currentHealth = (int)healthField.GetValue(enemyMovement);
                    int newHealth = Mathf.Max(1, Mathf.RoundToInt(currentHealth * enemyHPMultiplier));
                    setHealthMethod.Invoke(enemyMovement, new object[] { newHealth });
                    Debug.Log("敵のHP倍率適用: " + currentHealth + " -> " + newHealth);
                }
            }
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab == null)
        {
            Debug.LogWarning("ボスのプレハブが設定されていません");
            return;
        }

        // ボスをランダムなY座標で発射
        float y = Random.Range(-4f, 4f);
        Vector2 spawnPos = new Vector2(10f, y);
        GameObject spawnedBoss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        // ボス発射時のHP倍率増加
        enemyHPMultiplier *= hpIncreaseMultiplier;

        // ボスのHP倍率を適用
        BossController bossController = spawnedBoss.GetComponent<BossController>();
        if (bossController != null)
        {
            int currentBossHP = bossController.maxHP;
            int newBossHP = Mathf.Max(1, Mathf.RoundToInt(currentBossHP * enemyHPMultiplier));
            bossController.SetHealth(newBossHP);
            Debug.Log("ボスHP倍率適用: " + currentBossHP + " -> " + newBossHP);
        }

        Debug.Log("ボスを発射しました！ 敵スポーン間隔: " + specialWaveInterval + " HP倍率: " + enemyHPMultiplier);
    }
}