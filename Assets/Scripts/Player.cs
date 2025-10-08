using System.Collections;
using Unity.VisualScripting;
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
    private float currentTimeToFly;
    private float fly;

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
    private PlayerAnimatorManager pam;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        pam = GetComponent<PlayerAnimatorManager>();
    }
    
    void Update()
    {
        PlayerInputs();

        DirectionLook();

        FlyingCheck();
    }

    private void FixedUpdate()
    {
        OnMove();
        OnFly();
    }

    private void PlayerInputs()
    {
        movement = Input.GetAxisRaw("Horizontal") * speed;
        fly = Input.GetAxisRaw("Vertical") * flySpeed;

        if (Input.GetButtonDown("Jump") && !onAttackCooldown)
        {
            pam.Attack();
            StartCoroutine(OnAttack());
            StartCoroutine(AttackCooldown());
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            currentTimeToFly = 0.11f;
        }
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            currentTimeToFly += Time.deltaTime;
        }
        else if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            currentTimeToFly = timeToFly;
        }

        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && currentTimeToFly > 0.1f)
        {
            if (!OnGround())
            {
                currentTimeToFly -= Time.deltaTime;
            }
            else
            {
                currentTimeToFly = 0.0f;
            }

        }
    }

    private void OnMove()
    {
        rb.linearVelocity = new Vector2(movement, rb.linearVelocity.y);
    }

    private void OnFly()
    {
        if (currentTimeToFly >= timeToFly)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fly);
        }
    }

    private void FlyingCheck()
    {
        if (currentTimeToFly < 0.1f)
        {
            rb.gravityScale = 1;
        }
        else
        {
            rb.gravityScale = 0;
        }
    }

    private IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.06f);
        GameObject attack = Instantiate(playerAttack);
        attack.transform.position = playerAttackPosition.position;
        attack.transform.rotation = transform.rotation;
        attack.transform.parent = transform;
        Destroy(attack, 0.35f);
    }

    private IEnumerator AttackCooldown()
    {
        onAttackCooldown = true;
        yield return new WaitForSeconds(0.5f);
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

    public float GetFlySpeed()
    {
        return fly;
    }

    public bool OnGround()
    {
        return Physics2D.OverlapBox(sensorGround.position, size, 0, groundLayer);
    }

    public bool IsFlying()
    {
        return (!OnGround() || rb.linearVelocityY != 0.0f || currentTimeToFly >= 0.1f);
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
