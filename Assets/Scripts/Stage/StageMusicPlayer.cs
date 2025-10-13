using UnityEngine;

public class StageMusicPlayer : MonoBehaviour
{
    [Header("Audio List")]
    private static StageMusicPlayer instance;
    [SerializeField] private AudioClip[] audios;
    private AudioSource audioSource;

    public int MAINMENU = 0;
    public int STAGE = 1;
    public int VICTORY = 2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PlayAudio(MAINMENU);
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
