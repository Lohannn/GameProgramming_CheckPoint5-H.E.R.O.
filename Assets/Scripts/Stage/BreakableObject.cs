using System.Collections;
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

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
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
                    Destroy(gameObject);
                }
            }
        }

        if (collision.CompareTag("Explosion"))
        {
            GameObject points = Instantiate(scoreGain);
            points.transform.SetParent(GameObject.FindGameObjectWithTag("ScoreGainCanvas").transform, true);
            points.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            points.GetComponent<ScoreGained>().SetText(scoreValue.ToString());

            PlayerData.AddScore(scoreValue);

            Destroy(gameObject);
        }
    }
}
