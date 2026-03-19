using UnityEngine;
using System.Collections;

public class EnemyDamageEffect : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color originalColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void PlayDamageEffect()
    {
        StartCoroutine(DamageFlash());
    }

    IEnumerator DamageFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.08f);
        spriteRenderer.color = originalColor;
    }
}