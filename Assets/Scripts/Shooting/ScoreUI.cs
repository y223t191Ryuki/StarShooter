using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public Text scoreText;
    
    // ゲージ用のUI要素
    public Image upgradeGauge; // 強化ゲージ（fillableなImage）
    void Update()
    {
        scoreText.text = "Score: " + ScoreManager.instance.score;
        UpdateUpgradeGauge();
    }

    void UpdateUpgradeGauge()
    {
        if (ScoreManager.instance == null) return;

        // ゲージの進捗を更新
        if (upgradeGauge != null)
        {
            upgradeGauge.fillAmount = ScoreManager.instance.GetUpgradeProgress();
        }
    }
}