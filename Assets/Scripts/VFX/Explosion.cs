using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float timeToDestroy;

    void Start()
    {
        StartCoroutine(ExplosionTimer(timeToDestroy));
    }

    private IEnumerator ExplosionTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Destroy(gameObject);
    }
}
