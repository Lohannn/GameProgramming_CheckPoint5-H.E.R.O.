using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private float damage;

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float value)
    {
        damage = value;
    }
}
