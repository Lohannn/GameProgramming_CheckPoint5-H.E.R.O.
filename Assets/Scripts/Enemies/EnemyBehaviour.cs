using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private int scoreValue;
    [SerializeField] private GameObject scoreGain;

    private void TakeDamage()
    {
        GameObject points = Instantiate(scoreGain);
        points.transform.SetParent(GameObject.FindGameObjectWithTag("ScoreGainCanvas").transform, true);
        points.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        points.GetComponent<ScoreGained>().SetText(scoreValue.ToString());

        PlayerData.AddScore(scoreValue);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack") || collision.CompareTag("Explosion"))
        {
            TakeDamage();
        }
    }
}
