using UnityEngine;

[System.Serializable]
public class StageEvent
{
    public float time;              // 何秒後に出すか
    public GameObject enemyPrefab;  // 出現させる敵
    public Vector2 position;        // 出現位置
}