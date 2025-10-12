using UnityEngine;

public class StageAudioPlayer : MonoBehaviour
{
    [Header("Audio List")]
    [SerializeField] private AudioClip[] audios;

    private AudioSource audioSource;

    public int ADDSCORE = 0;
    public int STAGEEND = 1;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(int audioIndex)
    {
        audioSource.PlayOneShot(audios[audioIndex]);
    }
}
