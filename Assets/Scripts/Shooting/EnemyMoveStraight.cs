using UnityEngine;

public class EnemyMoveStraight : EnemyMovement
{
    public int health = 1; // 直進敵のHP

    protected override void Move()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
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