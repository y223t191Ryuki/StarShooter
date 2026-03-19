using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval = 0.5f;
    public enum ShotType
    {
        Normal,     // 通常1方向
        ThreeWay,   // 3way
        Homing      // 追尾
    }

    public ShotType currentShotType = ShotType.Normal;
    public int shotLevel = 1;
    public int maxShotLevel = 4;

    private float shootTimer = 0f;
    public PlayerHealth playerHealth;
    [Header("Visual Effects")]
    public GameObject spreadEffect;
    public GameObject homingEffect;

    void Update()
    {
        // ★ HPが0以下なら何もしない
        if (playerHealth != null && playerHealth.currentHP <= 0)
        return;

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval;
        }
    }

    void Shoot()
    {
        switch (currentShotType)
        {
            case ShotType.Normal:
                ShootNormal();
                break;

            case ShotType.ThreeWay:
                ShootSpread();
                break;

            case ShotType.Homing:
                ShootHoming();
                break;
        }
    }

    void ShootNormal()
    {
        SeManager.Instance.PlayPlayerShotSe();
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }

    void ShootSpread()
    {
        int bulletCount = 3;
        float spreadAngle = 15f;

        if (shotLevel == 2)
        {
            bulletCount = 5;
            spreadAngle = 25f;
        }
        else if (shotLevel >= 3)
        {
            bulletCount = 7;
            spreadAngle = 35f;
        }

        if (bulletCount == 1)
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            return;
        }

        float angleStep = spreadAngle * 2f / (bulletCount - 1);
        float startAngle = -spreadAngle;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            Instantiate(bulletPrefab, transform.position, rot);
        }
        SeManager.Instance.PlayPlayerShotSe();
    }

    void ShootHoming()
    {
        SeManager.Instance.PlayPlayerShotSe();
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        BulletMovement bm = bullet.GetComponent<BulletMovement>();
        if (bm != null)
        {
            bm.isHoming = true;
            bm.homingLevel = shotLevel;
        }
    }

    public void IncreaseFireRate()
    {
        shootInterval *= 0.9f;
    }

    // ★ アップグレード用
    public void ChangeShotType(ShotType type)
    {
        currentShotType = type;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (spreadEffect != null)
            spreadEffect.SetActive(currentShotType == ShotType.ThreeWay);
        if (homingEffect != null)
            homingEffect.SetActive(currentShotType == ShotType.Homing);
    }

    public void LevelUpShot()
    {
        if (shotLevel < maxShotLevel)
        {
            shotLevel++;
        }
    }
}