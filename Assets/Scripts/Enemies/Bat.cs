using System.Collections;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float timeToTurn;
    [SerializeField] private float direction = 0.5f;

    void Start()
    {
        StartCoroutine(TurnDirection(timeToTurn));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * Vector2.down);
    }

    private IEnumerator TurnDirection(float time)
    {
        yield return new WaitForSeconds(time);
        direction = -direction;

        StartCoroutine(TurnDirection(time));
    }
}
