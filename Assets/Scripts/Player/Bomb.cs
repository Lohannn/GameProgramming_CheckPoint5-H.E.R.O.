using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private float timeToExplode;

    private BombAudioPlayer ap;
    private VfxPoolManager pool;

    private void Start()
    {
        ap = GetComponent<BombAudioPlayer>();
        pool = GameObject.FindGameObjectWithTag("VFXPool").GetComponent<VfxPoolManager>();
    }

    private void OnEnable()
    {
        StartCoroutine(ExplosionTimer(timeToExplode));
    }

    private IEnumerator ExplosionTimer(float time)
    {
        yield return new WaitForSeconds(time);
        ap.PlayAudio(ap.EXPLOSION);
        Vector2 explosionPosition = new Vector2(transform.position.x + 0.07f, transform.position.y);
        pool.GetBombExplosion(explosionPosition, transform.rotation);
        gameObject.SetActive(false);
    }
}