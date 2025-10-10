using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private GameObject explosionPreFab;
    [SerializeField] private float timeToExplode;

    void Start()
    {
        StartCoroutine(ExplosionTimer(timeToExplode));
    }

    private IEnumerator ExplosionTimer(float time)
    {
        yield return new WaitForSeconds(time);
        Vector2 explosionPosition = new Vector2(transform.position.x + 0.07f, transform.position.y);
        GameObject explosion = Instantiate(explosionPreFab, explosionPosition, transform.rotation);
        Destroy(explosion, 0.6f);
        Destroy(gameObject);
    }
}
