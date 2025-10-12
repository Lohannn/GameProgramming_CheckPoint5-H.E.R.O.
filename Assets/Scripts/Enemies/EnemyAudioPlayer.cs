using UnityEngine;

public class EnemyAudioPlayer : MonoBehaviour
{
    [Header("Audio List")]
    [SerializeField] private AudioClip[] audios;

    private AudioSource audioSource;

    public int DEATH = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(int audioIndex)
    {
        AudioSource.PlayClipAtPoint(audios[audioIndex], transform.position);
    }
}
