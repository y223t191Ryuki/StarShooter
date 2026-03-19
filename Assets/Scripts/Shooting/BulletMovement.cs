using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed = 10f;
    public static int bulletDamage = 1; // 弾のダメージ
    public bool isHoming = false;
    public int homingLevel = 1;
    public float baseTurnSpeed = 180f;
    public float rotateSpeed = 360f;
    Transform target;

    void Start()
    {
        Destroy(gameObject, 2f);
        if (isHoming)
        {
            GameObject enemy = GameObject.FindWithTag("Enemy");
            if (enemy != null)
                target = enemy.transform;
        }
    }

    void Update()
    {
        if (isHoming && target != null)
        {
            float turnSpeed = baseTurnSpeed;

            if (homingLevel == 2)
            {
                turnSpeed = baseTurnSpeed * 1.5f;
            }
            else if (homingLevel >= 3)
            {
                turnSpeed = baseTurnSpeed * 2.2f;
            }

            Vector2 dir = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                turnSpeed * Time.deltaTime
            );
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        transform.Rotate(0, 0, Random.Range(-0.01f, 0.01f));
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // EnemyMovement派生クラスを探してTakeDamageを呼び出す
            EnemyMovement enemyMovement = collision.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                // 動的にTakeDamageメソッドを呼び出す
                var method = enemyMovement.GetType().GetMethod("TakeDamage");
                if (method != null)
                {
                    method.Invoke(enemyMovement, new object[] { bulletDamage });
                }
            }
            Destroy(gameObject);           // 弾を破壊
        }

        if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<BossController>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Bullet"))
        {
            ScoreManager.instance.AddScore(10);
            Destroy(collision.gameObject);
        }
    }

    public static void IncreaseDamage()
    {
        bulletDamage += 1;
    }
}