using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float timeToDestroy;

    private void OnEnable()
    {
        StartCoroutine(ExplosionTimer(timeToDestroy));
    }

    private IEnumerator ExplosionTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        gameObject.SetActive(false);
    }
}
