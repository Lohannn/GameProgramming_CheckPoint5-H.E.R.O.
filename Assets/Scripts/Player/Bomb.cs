using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private GameObject explosionPreFab;
    [SerializeField] private float timeToExplode;

    private BombAudioPlayer ap;
    void Start()
    {
        ap = GetComponent<BombAudioPlayer>();

        StartCoroutine(ExplosionTimer(timeToExplode));
    }

    private IEnumerator ExplosionTimer(float time)
    {
        yield return new WaitForSeconds(time);
        ap.PlayAudio(ap.EXPLOSION);
        Vector2 explosionPosition = new Vector2(transform.position.x + 0.07f, transform.position.y);
        Instantiate(explosionPreFab, explosionPosition, transform.rotation);
        Destroy(gameObject);
    }
}