using UnityEngine;
using UnityEngine.UI;

public class TextGlitch : MonoBehaviour
{
    public Text text;
    public float glitchChance = 0.05f;
    public float glitchDuration = 0.05f;
    public float flickerSpeed = 0.05f;
    public float minAlpha = 0.6f;
    public float maxAlpha = 1.0f;

    string originalText;

    void Start()
    {
        originalText = text.text;
        StartCoroutine(Flicker());
    }

    void Update()
    {
        if (Random.value < glitchChance)
        {
            StartCoroutine(Glitch());
        }
    }

    System.Collections.IEnumerator Glitch()
    {
        text.text = "G@M3 0V3R...";
        yield return new WaitForSeconds(glitchDuration);
        text.text = originalText;
    }

    System.Collections.IEnumerator Flicker()
    {
        while (true)
        {
            Color c = text.color;
            c.a = Random.Range(minAlpha, maxAlpha);
            text.color = c;

            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}