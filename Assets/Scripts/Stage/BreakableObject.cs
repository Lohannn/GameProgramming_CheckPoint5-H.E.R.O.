using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BreakableObject : MonoBehaviour
{
    [Header("Breakable Object Settings")]
    [SerializeField] private GameObject scoreGain;
    [SerializeField] private int scoreValue;
    [SerializeField] private float maxHealth;
    private float currentHealth;

    private bool isBlinking;
    private bool canBeDamaged = true;

    private SpriteRenderer sr;
    private VfxPoolManager pool;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        pool =  GameObject.FindGameObjectWithTag("VFXPool").GetComponent<VfxPoolManager>();
        currentHealth = maxHealth;
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;

        StartCoroutine(DamageCooldown());

        if (!isBlinking)
        {
            StartCoroutine(DamageBlink());
        }
    }

    private IEnumerator DamageBlink()
    {
        isBlinking = true;
        float maxTime = 0.2f;
        for (float i = 0; i < maxTime; i += Time.deltaTime)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.02f);
        }
        sr.enabled = true;
        isBlinking = false;
    }

    private IEnumerator DamageCooldown()
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(0.5f);
        canBeDamaged = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack"))
        {
            if (canBeDamaged)
            {
                TakeDamage(collision.GetComponent<Attack>().GetDamage());

                if (currentHealth <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        if (collision.CompareTag("Explosion"))
        {
            pool.GetScoreText(scoreValue, Camera.main.WorldToScreenPoint(transform.position), Quaternion.identity);
            PlayerData.AddScore(scoreValue);

            gameObject.SetActive(false);
        }
    }
}
