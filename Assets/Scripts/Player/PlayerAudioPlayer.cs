using UnityEngine;

public class PlayerAudioPlayer : MonoBehaviour
{
    [Header("Audio List")]
    [SerializeField] private AudioClip[] audios;

    private AudioSource audioSource;
    private float audioVolume;

    public int ATTACK = 0;
    public int VICTORY = 1;
    public int DEATH = 2;
    public int BOMB = 3;
    public int BONUSLIFE = 4;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioVolume = audioSource.volume;

        PlayerData.OnBonusLifeGained += PlayBonusLifeAudio;
    }

    public void PlayAudio(int audioIndex)
    {
        if (audioIndex == 0)
        {
            audioSource.volume = 0.2f;
            audioSource.PlayOneShot(audios[audioIndex]);
        }
        else
        {
            audioSource.volume = audioVolume;
            audioSource.PlayOneShot(audios[audioIndex]);
        }
    }

    public void PlayBonusLifeAudio()
    {
        if (audioSource != null)
        {
            audioSource.volume = audioVolume;
            audioSource.PlayOneShot(audios[BONUSLIFE]);
        }
    }
}
