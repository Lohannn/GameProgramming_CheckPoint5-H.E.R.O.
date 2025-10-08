using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    private float movement;

    [Header("Fly Settings")]
    [SerializeField] private float flySpeed;
    [SerializeField] private float timeToFly;
    [SerializeField] private float playerGravity;

    [Header("Ground Detector Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform sensorGround;
    [SerializeField] private Vector2 size;
    [SerializeField] private Color32 sensorColor;

    [Header("Attack Settings")]
    [SerializeField] private GameObject playerAttack;
    [SerializeField] private Transform playerAttackPosition;
    private bool onAttackCooldown;

    private SpriteRenderer sr;
    private Rigidbody2D rb;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        PlayerInputs();

        DirectionLook();
    }

    private void FixedUpdate()
    {
        OnMove();
    }

    private void PlayerInputs()
    {
        movement = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump") && !onAttackCooldown)
        {
            StartCoroutine(OnAttack());
            StartCoroutine(AttackCooldown());
        }
    }

    private void OnMove()
    {
        rb.linearVelocity = new Vector2(movement, rb.linearVelocity.y);
    }

    private IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.06f);
        GameObject attack = Instantiate(playerAttack);
        attack.transform.position = playerAttackPosition.position;
        attack.transform.parent = transform;
        Destroy(attack, 0.35f);
    }

    private IEnumerator AttackCooldown()
    {
        onAttackCooldown = true;
        yield return new WaitForSeconds(0.07f);
        onAttackCooldown = false;
    }

    private void DirectionLook()
    {
        if (movement > 0)
        {
            transform.eulerAngles = new(0, 0, 0);
        }
        else if (movement < 0)
        {
            transform.eulerAngles = new(0, 180, 0);
        }
    }

    public float GetDirection()
    {
        return movement;
    }

    public bool OnGround()
    {
        return Physics2D.OverlapBox(sensorGround.position, size, 0, groundLayer);
    }

    public bool OnAttackCooldown()
    {
        return onAttackCooldown;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = sensorColor;
        Gizmos.DrawCube(sensorGround.position, size);
    }
}
