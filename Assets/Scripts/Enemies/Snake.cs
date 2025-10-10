using System.Collections;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float timeToTurn;
    [SerializeField] private float direction = 1.5f;

    void Start()
    {
        StartCoroutine(TurnDirection(timeToTurn));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * Vector2.right);
    }

    private IEnumerator TurnDirection(float time)
    {
        yield return new WaitForSeconds(time);
        direction = -direction;

        StartCoroutine(TurnDirection(time));
    }
}
