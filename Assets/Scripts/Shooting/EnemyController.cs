using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float enemySpeed = 3f;
    public int health = 1; // 敵のHP

    void Update()
    {
        transform.Translate(Vector2.left * enemySpeed * Time.deltaTime);
        if (transform.position.x < -10f)
            Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            ScoreManager.instance.AddScore(100);
            Destroy(gameObject);
        }
    }
}