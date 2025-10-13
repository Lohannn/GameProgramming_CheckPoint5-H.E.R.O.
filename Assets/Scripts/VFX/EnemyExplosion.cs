using System.Collections;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DisableTimer());
    }

    private IEnumerator DisableTimer()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        gameObject.SetActive(false);
    }
}
