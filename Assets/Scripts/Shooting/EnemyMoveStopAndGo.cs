using UnityEngine;

public class EnemyMoveStopAndGo : EnemyMovement
{
    public float stopX = 3f;
    public float stopTime = 1.5f;
    public int health = 2; // 停止敵のHP

    bool stopped = false;
    bool hasStoppedOnce = false;

    protected override void Move()
    {
        if (!stopped)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            if (!hasStoppedOnce && transform.position.x <= stopX)
            {
                stopped = true;
                hasStoppedOnce = true;
                Invoke(nameof(Restart), stopTime);
            }
        }
    }

    void Restart()
    {
        stopped = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        GetComponent<EnemyDamageEffect>()?.PlayDamageEffect();
        SeManager.Instance.PlayEnemyDamageSe();
        if (health <= 0)
        {
            Die(DeathCause.Normal);
        }
    }

    public override void Die(DeathCause cause)
    {
        if (cause == DeathCause.Normal)
        {
            SeManager.Instance.PlayEnemyDeathSe();
            ScoreManager.instance.AddScore(100);
        }
        base.Die(cause);
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }
}