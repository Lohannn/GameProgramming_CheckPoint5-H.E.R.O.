using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    //private Player player;

    //private void Start()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    //}

    //private void Update()
    //{
    //    if (player != null)
    //    {
    //        Time.timeScale = player.GetTime();
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack") || collision.CompareTag("Explosion"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            Destroy(gameObject);
        }
    }
}
