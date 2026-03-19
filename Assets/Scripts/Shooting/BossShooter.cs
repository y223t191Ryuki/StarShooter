using UnityEngine;
using System.Collections;

public class BossShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    Coroutine shootingCoroutine;

    public void StartStraightShot(float interval)
    {
        StopShooting();
        shootingCoroutine = StartCoroutine(StraightShot(interval));
    }

    public void StartCircleShot(float interval, int count)
    {
        StopShooting();
        shootingCoroutine = StartCoroutine(CircleShot(interval, count));
    }

    public void StopShooting()
    {
        if (shootingCoroutine != null)
            StopCoroutine(shootingCoroutine);
    }

    IEnumerator StraightShot(float interval)
    {
        while (true)
        {
            GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            b.GetComponent<BossBullet>().SetDirection(Vector2.left);
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator CircleShot(float interval, int count)
    {
        while (true)
        {
            for (int i = 0; i < count; i++)
            {
                float angle = i * (360f / count);
                Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

                GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                b.GetComponent<BossBullet>().SetDirection(dir);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}