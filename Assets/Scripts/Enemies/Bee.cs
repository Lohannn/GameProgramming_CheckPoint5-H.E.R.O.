using System.Collections;
using UnityEngine;

public class Bee : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float timeToTurnHorizontal;
    [SerializeField] private float timeToTurnVertical;
    [SerializeField] private float horizontalSpeed = 1.0f;
    [SerializeField] private float verticalSpeed = 0.5f;

    private SpriteRenderer sr;
    private Animator anim;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        StartCoroutine(TurnHorizontalDirection(timeToTurnHorizontal));
        StartCoroutine(TurnVerticalDirection(timeToTurnVertical));
    }

    void Update()
    {
        transform.Translate(horizontalSpeed * Time.deltaTime * Vector2.right);
        transform.Translate(verticalSpeed * Time.deltaTime * Vector2.down);
    }

    private IEnumerator TurnHorizontalDirection(float time)
    {
        yield return new WaitForSeconds(time);
        sr.flipX = !sr.flipX;
        anim.SetTrigger("pTurn");
        horizontalSpeed = -horizontalSpeed;

        StartCoroutine(TurnHorizontalDirection(time));
    }

    private IEnumerator TurnVerticalDirection(float time)
    {
        yield return new WaitForSeconds(time);
        verticalSpeed = -verticalSpeed;

        StartCoroutine(TurnVerticalDirection(time));
    }
}
