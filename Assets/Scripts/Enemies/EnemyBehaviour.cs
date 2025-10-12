using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject deathExplosion;

    [Header("Enemy Settings")]
    [SerializeField] private int scoreValue;
    [SerializeField] private GameObject scoreGain;

    private EnemyAudioPlayer ap;
    private SpriteRenderer sr;

    private void Start()
    {
        ap = GetComponent<EnemyAudioPlayer>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void TakeDamage()
    {
        GameObject points = Instantiate(scoreGain);
        points.transform.SetParent(GameObject.FindGameObjectWithTag("ScoreGainCanvas").transform, true);
        points.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        points.GetComponent<ScoreGained>().SetText(scoreValue.ToString());

        PlayerData.AddScore(scoreValue);

        GameObject blow = Instantiate(deathExplosion);
        blow.transform.position = transform.position;
        Destroy(blow, 0.3f);
        ap.PlayAudio(ap.DEATH);
        Destroy(gameObject);
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
