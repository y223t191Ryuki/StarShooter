using UnityEngine;

public class EnemyMoveFollow : EnemyMovement
{
    public int health = 3; // 追従敵のHP

    protected override void Move()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * speed * Time.deltaTime);
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
            ScoreManager.instance.AddScore(150);
        }
        base.Die(cause);
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }
}