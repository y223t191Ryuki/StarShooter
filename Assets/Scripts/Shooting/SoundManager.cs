using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    /// <summary>
    /// SE����
    /// </summary>
    [SerializeField]
    AudioSource seAudioSource;

    /// <summary>
    /// SE���ʂ̎擾�ݒ�
    /// </summary>
    public float SeVolume
    {
        //���ʂ̎擾
        get
        {
            return seAudioSource.volume;
        }

        //�͈͂̐ݒ�
        set
        {
            seAudioSource.volume = Mathf.Clamp01(value);
        }
    }

    /// <summary>
    /// �V���O���g��
    /// </summary>
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// SE�̍Đ�
    /// </summary>
    /// <param name="seClip">SE�\�[�X</param>
    public void PlaySe(AudioClip seClip, float volume = 1.0f)
    {
        if (seClip == null)
        {
            return;
        }

        //�Đ�
        seAudioSource.PlayOneShot(seClip, volume);
    }

}
