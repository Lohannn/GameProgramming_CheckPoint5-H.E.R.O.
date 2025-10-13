using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private float damage;

    private void OnEnable()
    {
        StartCoroutine(DisableTimer());
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    private IEnumerator DisableTimer()
    {
        yield return new WaitForSecondsRealtime(0.35f);
        gameObject.SetActive(false);
    }
}
