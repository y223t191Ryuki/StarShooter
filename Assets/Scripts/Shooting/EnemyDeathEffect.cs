using UnityEngine;

public class EnemyDeathEffect : MonoBehaviour
{
    public GameObject explosionPrefab;

    public void PlayDeathEffect()
    {
        if (explosionPrefab != null)
        {
            Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }
}