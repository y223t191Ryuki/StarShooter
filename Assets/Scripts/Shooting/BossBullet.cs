using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 5f;
    Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        transform.Rotate(0, 0, Random.Range(-0.01f, 0.01f));
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // ダメージ量は適宜調整
            }
            Destroy(gameObject);
        }
    }
}