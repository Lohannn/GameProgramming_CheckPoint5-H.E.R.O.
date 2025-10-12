using UnityEngine;

public class StageMusicPlayer : MonoBehaviour
{
    [Header("Audio List")]
    [SerializeField] private AudioClip[] audios;

    private AudioSource audioSource;

    public int MAINMENU = 0;
    public int STAGE = 1;
    public int VICTORY = 2;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PlayAudio(STAGE);
    }

    public void PlayAudio(int audioIndex)
    {
        audioSource.resource = audios[audioIndex];

        if (audioIndex == 2)
        {
           audioSource.loop = false;
           audioSource.Play();
        }
        else
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
