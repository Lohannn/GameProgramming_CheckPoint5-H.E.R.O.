using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private int scoreValue;

    private EnemyAudioPlayer ap;
    private SpriteRenderer sr;
    private VfxPoolManager pool;

    private void Start()
    {
        ap = GetComponent<EnemyAudioPlayer>();
        sr = GetComponent<SpriteRenderer>();
        pool = GameObject.FindGameObjectWithTag("VFXPool").GetComponent<VfxPoolManager>();
    }

    private void TakeDamage()
    {
        pool.GetScoreText(scoreValue, Camera.main.WorldToScreenPoint(transform.position), Quaternion.identity);
        PlayerData.AddScore(scoreValue);

        pool.GetEnemyExplosion(transform.position, Quaternion.identity);
        ap.PlayAudio(ap.DEATH);
        gameObject.SetActive(false);
    }

    public void OnDarkness()
    {
        if (sr != null)
        {
            sr.color = new Color32(128, 128, 128, 255);
        }
    }

    public void OnDarknessOut()
    {
        if (sr != null)
        {
            sr.color = Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack") || collision.CompareTag("Explosion"))
        {
            TakeDamage();
        }
    }
}
