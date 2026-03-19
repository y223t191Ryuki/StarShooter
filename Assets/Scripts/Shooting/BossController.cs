using UnityEngine;

public class BossController : MonoBehaviour
{
    public int maxHP = 300;
    int currentHP;
    public enum BossPhase
    {
        Enter,
        Phase1,
        Phase2,
        Phase3,
        Dead
    }
    public BossPhase phase = BossPhase.Enter;

    BossShooter shooter;

    EnemyMovement movement;

    private BossPhase previousPhase;

    void Start()
    {
        currentHP = maxHP;
        movement = GetComponent<EnemyMovement>();
        shooter = GetComponent<BossShooter>();
        previousPhase = phase;
    }

    void Update()
    {
        if (phase == BossPhase.Enter)
        {
            EnterMove();
        }
        else if (phase != previousPhase)
        {
            UpdatePhase();
            previousPhase = phase;
        }
    }

    void UpdatePhase()
    {
        switch (phase)
        {
            case BossPhase.Phase1:
                shooter.StartStraightShot(0.5f);
                break;

            case BossPhase.Phase2:
                SetMovement(false);
                shooter.StartCircleShot(0.5f, 16);
                break;

            case BossPhase.Phase3:
                SetMovement(true);
                movement.speed = 8f;
                shooter.StartCircleShot(0.5f, 24);
                break;
        }
    }

    void SetMovement(bool active)
    {
        if (movement != null)
            movement.enabled = active;
    }

    void EnterMove()
    {
        // 画面内まで移動したら Phase1 へ
        if (transform.position.x > 5f)
        {
            transform.Translate(Vector2.left * Time.deltaTime * 3f);
        }
        else
        {
            phase = BossPhase.Phase1;
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        GetComponent<EnemyDamageEffect>()?.PlayDamageEffect();
        SeManager.Instance.PlayEnemyDamageSe();

        if (currentHP <= 0)
        {
            phase = BossPhase.Dead;
            Die();
        }
        else if (currentHP <= maxHP * 0.4f)
        {
            phase = BossPhase.Phase3;
        }
        else if (currentHP <= maxHP * 0.7f)
        {
            phase = BossPhase.Phase2;
        }
    }

    public void Die()
    {
        GetComponent<EnemyDeathEffect>()?.PlayDeathEffect();
        SeManager.Instance.PlayBossDeathSe();
        ScoreManager.instance.AddScore(1000);
        Destroy(gameObject);
    }

    public void SetHealth(int newHealth)
    {
        maxHP = newHealth;
        currentHP = newHealth;
    }
}