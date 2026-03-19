using UnityEngine;

public class EnemyMoveWave : EnemyMovement
{
    public float frequency = 5f;
    public float amplitude = 1f;
    public int health = 1; // 波動敵のHP

    float time;

    protected override void Move()
    {
        time += Time.deltaTime;
        Vector3 pos = transform.position;
        pos.x -= speed * Time.deltaTime;
        pos.y += Mathf.Sin(time * frequency) * amplitude * Time.deltaTime;
        transform.position = pos;
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