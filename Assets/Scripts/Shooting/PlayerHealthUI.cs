using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image hpGaugeImage;  // HPゲージ用Image

    void Update()
    {
        if (hpGaugeImage != null)
        {
            hpGaugeImage.fillAmount = (float)playerHealth.currentHP / playerHealth.maxHP;
        }
    }
}