using UnityEngine;

public class BombAudioPlayer : MonoBehaviour
{
    [Header("Audio List")]
    [SerializeField] private AudioClip[] audios;

    private AudioSource audioSource;

    public int EXPLOSION = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(int audioIndex)
    {
        AudioSource.PlayClipAtPoint(audios[audioIndex], transform.position);
    }
}
