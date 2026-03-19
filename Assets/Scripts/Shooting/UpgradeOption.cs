using UnityEngine;

public class UpgradeOption
{
    public enum UpgradeType
    {
        IncreaseDamage,      // 攻撃力上昇
        IncreaseFireRate,    // 連射速度上昇
        IncreaseMoveSpeed,   // 移動速度上昇
        FullHeal,             // HP全回復
        ChangeShotNormal,    // 通常弾に変更
        ChangeShotThreeWay,  // 3Way弾に変更
        ChangeShotHoming,     // 追尾弾に変更
        UpgradeShotLevelUp   // ショットレベルアップ
    }

    public UpgradeType type;
    public string name;
    public string description;

    public UpgradeOption(UpgradeType upgradeType, string upgradeName, string upgradeDescription)
    {
        type = upgradeType;
        name = upgradeName;
        description = upgradeDescription;
    }

    public void Apply(GameObject player)
    {
        switch (type)
        {
            case UpgradeType.IncreaseDamage:
                BulletMovement.IncreaseDamage();
                Debug.Log("攻撃力が上昇した！");
                break;
            case UpgradeType.IncreaseFireRate:
                PlayerShot playerShot = player.GetComponent<PlayerShot>();
                if (playerShot != null) playerShot.IncreaseFireRate();
                Debug.Log("連射速度が上昇した！");
                break;
            case UpgradeType.IncreaseMoveSpeed:
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                if (playerMovement != null) playerMovement.IncreaseMoveSpeed();
                Debug.Log("移動速度が上昇した！");
                break;
            case UpgradeType.FullHeal:
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null) playerHealth.FullHeal();
                Debug.Log("HPが全回復した！");
                break;

            case UpgradeType.ChangeShotNormal:
                PlayerShot psNormal = player.GetComponent<PlayerShot>();
                if (psNormal != null) psNormal.ChangeShotType(PlayerShot.ShotType.Normal);
                Debug.Log("弾が通常ショットに変更された！");
                break;

            case UpgradeType.ChangeShotThreeWay:
                PlayerShot psThree = player.GetComponent<PlayerShot>();
                if (psThree != null) psThree.ChangeShotType(PlayerShot.ShotType.ThreeWay);
                Debug.Log("弾が3Wayショットに変更された！");
                break;

            case UpgradeType.ChangeShotHoming:
                PlayerShot psHoming = player.GetComponent<PlayerShot>();
                if (psHoming != null) psHoming.ChangeShotType(PlayerShot.ShotType.Homing);
                Debug.Log("弾が追尾ショットに変更された！");
                break;

            case UpgradeType.UpgradeShotLevelUp:
                PlayerShot psLevelUp = player.GetComponent<PlayerShot>();
                if (psLevelUp != null)
                {
                    psLevelUp.LevelUpShot();
                }
                Debug.Log("ショットレベルが上昇した！");
                break;
        }
    }
}
