using UnityEngine;
using UnityEngine.UI;

public class AdviceText : MonoBehaviour
{
    public Text adviceText;

    [TextArea(2, 4)]
    public string[] advices;

    void Start()
    {
        if (advices.Length == 0) return;

        int index = Random.Range(0, advices.Length);
        adviceText.text = advices[index];
    }
}