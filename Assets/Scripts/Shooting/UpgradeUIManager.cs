using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class UpgradeUIManager : MonoBehaviour
{
    public static UpgradeUIManager instance;

    public GameObject upgradePanel; // UIパネル
    public Button option1Button;
    public Button option2Button;
    public Text option1NameText;
    public Text option1DescText;
    public Text option2NameText;
    public Text option2DescText;
    [SerializeField] private GameObject firstSelectedButton;

    private UpgradeOption[] currentOptions = new UpgradeOption[2];
    private GameObject player;
    private bool isUpgrading = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        if (option1Button != null)
            option1Button.onClick.AddListener(() => SelectOption(0));
        if (option2Button != null)
            option2Button.onClick.AddListener(() => SelectOption(1));
    }

    public void ShowUpgradeUI()
    {
        if (isUpgrading) return;
        StartCoroutine(UpgradeSequence());
    }

    IEnumerator UpgradeSequence()
{
    isUpgrading = true;

    // ① Enemyを全破壊
    EnemyMovement[] enemies = Object.FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None);
    foreach (EnemyMovement enemy in enemies)
    {
        if (enemy is BossMoveBasic) continue;
        enemy.ForceKill();
        SeManager.Instance.PlayEnemyKillSe();
    }

    // ② 爆散を見るために少し待つ（TimeScaleの影響を受けない）
    yield return new WaitForSecondsRealtime(0.5f);

    // ③ ゲームをポーズ
    Time.timeScale = 0f;

    // ④ 強化オプション生成
    currentOptions = GetRandomOptions(2);

    option1NameText.text = currentOptions[0].name;
    option1DescText.text = currentOptions[0].description;
    option2NameText.text = currentOptions[1].name;
    option2DescText.text = currentOptions[1].description;

    upgradePanel.SetActive(true);
    EventSystem.current.SetSelectedGameObject(firstSelectedButton);

}

    void SelectOption(int optionIndex)
    {
        if (optionIndex >= 0 && optionIndex < 2 && currentOptions[optionIndex] != null)
        {
            currentOptions[optionIndex].Apply(player);
        }

        // UIを隠す
        upgradePanel.SetActive(false);
        isUpgrading = false;
        Time.timeScale = 1f; // ゲームを再開
        SeManager.Instance.PlayButtonPushSe();
    }

    UpgradeOption[] GetRandomOptions(int count)
    {
        PlayerShot ps = player.GetComponent<PlayerShot>();
        PlayerShot.ShotType currentShot = ps != null ? ps.currentShotType : PlayerShot.ShotType.Normal;

        UpgradeOption.UpgradeType[] allTypes;

        if (currentShot == PlayerShot.ShotType.Normal)
        {
            allTypes = new UpgradeOption.UpgradeType[]
            {
                UpgradeOption.UpgradeType.IncreaseDamage,
                UpgradeOption.UpgradeType.IncreaseFireRate,
                UpgradeOption.UpgradeType.IncreaseMoveSpeed,
                UpgradeOption.UpgradeType.FullHeal,
                UpgradeOption.UpgradeType.ChangeShotThreeWay,
                UpgradeOption.UpgradeType.ChangeShotHoming
            };
        }
        else
        {
            allTypes = new UpgradeOption.UpgradeType[]
            {
                UpgradeOption.UpgradeType.IncreaseDamage,
                UpgradeOption.UpgradeType.IncreaseFireRate,
                UpgradeOption.UpgradeType.IncreaseMoveSpeed,
                UpgradeOption.UpgradeType.FullHeal,
                UpgradeOption.UpgradeType.UpgradeShotLevelUp
            };
        }

        UpgradeOption[] options = new UpgradeOption[count];
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, allTypes.Length);
            UpgradeOption.UpgradeType selectedType = allTypes[randomIndex];

            // 重複を避ける
            while (i > 0 && options[i - 1].type == selectedType)
            {
                randomIndex = Random.Range(0, allTypes.Length);
                selectedType = allTypes[randomIndex];
            }

            options[i] = CreateUpgradeOption(selectedType);
        }

        return options;
    }

    UpgradeOption CreateUpgradeOption(UpgradeOption.UpgradeType type)
    {
        switch (type)
        {
            case UpgradeOption.UpgradeType.IncreaseDamage:
                return new UpgradeOption(type, "攻撃力上昇", "テキに与えるダメージが増加する");
            case UpgradeOption.UpgradeType.IncreaseFireRate:
                return new UpgradeOption(type, "連射速度上昇", "弾のクールダウン時間が10%短縮される");
            case UpgradeOption.UpgradeType.IncreaseMoveSpeed:
                return new UpgradeOption(type, "移動速度上昇", "自機の素早さが10%上昇する");
            case UpgradeOption.UpgradeType.FullHeal:
                return new UpgradeOption(type, "HP回復", "HPが3回復する");
            case UpgradeOption.UpgradeType.ChangeShotNormal:
                return new UpgradeOption(type, "通常ショット", "弾が通常の1方向ショットに変更される");

            case UpgradeOption.UpgradeType.ChangeShotThreeWay:
                return new UpgradeOption(type, "3Wayショット", "弾が3方向に発射されるようになる");

            case UpgradeOption.UpgradeType.ChangeShotHoming:
                return new UpgradeOption(type, "追尾ショット", "弾が敵を追尾するようになる");
            case UpgradeOption.UpgradeType.UpgradeShotLevelUp:
                return new UpgradeOption(type, "弾種レベル上昇", "現在使用中の弾種が強化される");
            default:
                return null;
        }
    }
}
