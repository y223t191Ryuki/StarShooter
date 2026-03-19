using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score = 0;
    private int lastUpgradeScore = 0; // 最後に強化した時点のスコア
    private int upgradeThreshold = 500; // 強化UIを表示するスコア閾値

    void Awake()
    {
        instance = this;
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("スコア: " + score);

        // 500点増えるごとに強化UI を表示
        if (score - lastUpgradeScore >= upgradeThreshold)
        {
            lastUpgradeScore += upgradeThreshold;
            upgradeThreshold += 250; // 次の閾値を増加
            TriggerUpgradeUI();
        }
    }

    void TriggerUpgradeUI()
    {
        if (UpgradeUIManager.instance != null)
        {
            UpgradeUIManager.instance.ShowUpgradeUI();
        }
    }

    // 次の強化までの進捗率を取得（0～1の値）
    public float GetUpgradeProgress()
    {
        int scoreInThisCycle = score - lastUpgradeScore;
        return Mathf.Clamp01((float)scoreInThisCycle / upgradeThreshold);
    }

    // 次の強化まであと何点必要かを取得
    public int GetScoreUntilUpgrade()
    {
        return upgradeThreshold - (score - lastUpgradeScore);
    }

    // 現在のスコアから最後の強化スコアまでの差分を取得
    public int GetScoreInCurrentCycle()
    {
        return score - lastUpgradeScore;
    }
}