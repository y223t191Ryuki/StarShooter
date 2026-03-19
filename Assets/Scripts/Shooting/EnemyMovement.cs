using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    public float speed = 3f;
    public enum DeathCause
    {
        Normal,      // 通常撃破（スコア加算）
        Forced       // 一斉爆破など（スコア加算なし）
    }

    protected Transform player;

    protected virtual void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    protected virtual void Update()
    {
        Move();
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }

    public void ForceKill()
    {
        Die(DeathCause.Forced);
    }
    public virtual void Die(DeathCause cause)
    {
        GetComponent<EnemyDeathEffect>()?.PlayDeathEffect();
        Destroy(gameObject);
    }

    protected abstract void Move();
}