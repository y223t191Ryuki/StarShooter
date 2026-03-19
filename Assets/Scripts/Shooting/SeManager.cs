using UnityEngine;

public class SeManager : MonoBehaviour
{
    /// <summary>
    /// サウンドマネージャー
    /// </summary>
    [SerializeField]
    SoundManager soundManager;
    float PlayerShotlastPlayTime;
    float EnemyDeathlastPlayTime;
    float EnemyDamagelastPlayTime;
    public float Interval = 0.05f;
    [SerializeField] public AudioClip buttonPushSe;
    [SerializeField] public AudioClip PlayerDeathSe; 
    [SerializeField] public AudioClip PlayerDamageSe;
    [SerializeField] public AudioClip PlayerShotSe;
    [SerializeField] public AudioClip EnemyDeathSe;
    [SerializeField] public AudioClip EnemyDamageSe;
    [SerializeField] public AudioClip BossDeathSe;
    [SerializeField] public AudioClip EnemyKillSe;

    
    public static SeManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void PlayButtonPushSe() => soundManager.PlaySe(buttonPushSe);
    public void PlayPlayerDeathSe() => soundManager.PlaySe(PlayerDeathSe);
    public void PlayPlayerDamageSe() => soundManager.PlaySe(PlayerDamageSe);
    public void PlayPlayerShotSe()
    {
        if (Time.time - PlayerShotlastPlayTime < Interval) return;
        soundManager.PlaySe(PlayerShotSe , 0.5f);
        PlayerShotlastPlayTime = Time.time;
    }
    public void PlayEnemyDeathSe()
    {
        if (Time.time - EnemyDeathlastPlayTime < Interval) return;
        soundManager.PlaySe(EnemyDeathSe);
        EnemyDeathlastPlayTime = Time.time;
    }
    public void PlayEnemyDamageSe()
    {
        if (Time.time - EnemyDamagelastPlayTime < Interval) return;
        soundManager.PlaySe(EnemyDamageSe);
        EnemyDamagelastPlayTime = Time.time;
    }
    public void PlayBossDeathSe() => soundManager.PlaySe(BossDeathSe);
    public void PlayEnemyKillSe() => soundManager.PlaySe(EnemyKillSe , 0.2f);

}