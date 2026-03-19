using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.EventSystems;
using Fungus;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 5;
    public int currentHP;
    public GameObject EffectObject;
    public GameObject EffectObject2;
    public Text gameOverText;
    private bool isGameOver = false;
    public float invincibilityDuration = 1.5f; // 無敵時間（秒）
    private float invincibilityTimer = 0f;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;
    public float blinkInterval = 0.2f; // 点滅の間隔（秒）
    public Flowchart flowchart;
    [SerializeField] private GameObject ResetButton;
    [SerializeField] private EnemyDeathEffect deathEffect;
    [SerializeField] private float gameOverDelay = 0.6f;

    void Start()
    {
        // ゲーム時間をリセット（GameOverからのリロード対応）
        Time.timeScale = 1f;
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (deathEffect == null)
        {
            deathEffect = GetComponent<EnemyDeathEffect>();
        }
    }

    void Update()
    {
        // 無敵時間の更新
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
                // 無敵時間終了時は表示を戻す
                if (spriteRenderer != null)
                {
                    Color color = spriteRenderer.color;
                    color.a = 1f;
                    spriteRenderer.color = color;
                }
            }
            else
            {
                // 点滅処理
                if (spriteRenderer != null)
                {
                    // binkInterval に基づいて点滅
                    float blinkPhase = (invincibilityTimer % (blinkInterval * 2)) / (blinkInterval * 2);
                    Color color = spriteRenderer.color;
                    color.a = blinkPhase < 0.5f ? 0.5f : 1f;
                    spriteRenderer.color = color;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        // 無敵時間中はダメージを受けない
        if (isInvincible)
        {
            Debug.Log("無敵時間中のため、ダメージを受けませんでした");
            return;
        }

        currentHP -= damage;
        SeManager.Instance.PlayPlayerDamageSe();
        Debug.Log("プレイヤー HP: " + currentHP);

        // 無敵時間を開始
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;

        if (currentHP <= 0)
        {
            StartCoroutine(GameOverSequence());
        }
    }

    void GameOver()
    {
        if (isGameOver) return; // 既にゲームオーバーなら何もしない
        isGameOver = true;
        Debug.Log("GAME OVER");
        flowchart.ExecuteBlock("Gameover");
    }

    public void FullHeal()
    {
        int healAmount = 3;
        currentHP = Mathf.Min(currentHP + healAmount, maxHP);
        Debug.Log("HPを回復した！ 現在HP: " + currentHP);
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    public void ResetTime(){
        Time.timeScale = 1f;
    }

    IEnumerator GameOverSequence()
    {
        if (isGameOver) yield break;

        // プレイヤー爆散演出
        if (deathEffect != null)
        {
            deathEffect.PlayDeathEffect();
        }

        flowchart.ExecuteBlock("MusicStop");
        SeManager.Instance.PlayPlayerDeathSe();

        EffectObject.SetActive(false);
        EffectObject2.SetActive(false);

        // 見た目を消す
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = false;
        }

        // 爆散を見るために少し待つ（Time.timeScale に依存しない）
        yield return new WaitForSecondsRealtime(gameOverDelay);

        GameOver();
    }
}