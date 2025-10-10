using System.Collections;
using UnityEngine;

public class PlayerExample : MonoBehaviour
{
    [Header("Hurt Settings")]
    //[SerializeField] private float knockbackForce;
    //private bool isHurted;
    [SerializeField] private float hurtTime;
    [SerializeField] private Color knockColor;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxJumpTime;
    private float currentJumpTime;
    [SerializeField] private Transform sensorGround;
    [SerializeField] private Vector2 sensorSize;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private float gravity;

    [Header("Move Settings")]
    [SerializeField] private float speed;
    private float movement;

    [Header("Attack Settings")]
    [SerializeField] private GameObject playerAttackPrefab;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float timeToDestroyAttack;

    [Header("HurtSettings")]
    [SerializeField] private float KnockbackForce;
    [SerializeField] private float KnockbackTime;


    private bool isDucking = false;
    private bool inKnockback = false;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D col;
    //private PlayerAudios pa;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        //pa = GetComponent<PlayerAudios>();
    }

    void Update()
    {
        if (inKnockback) return;

        PlayerInputs();

        LookMoveDirection();
    }

    private void FixedUpdate()
    {
        OnMove();
        OnJump();
    }

    private void PlayerInputs()
    {
        movement = Input.GetAxisRaw("Horizontal") * speed;

        OnDuck();

        if (Input.GetButtonDown("Jump") && OnGround())
        {
            //pa.PlayAudio(pa.JUMP);
            currentJumpTime = maxJumpTime;
        }
        else if(Input.GetButton("Jump") && currentJumpTime > 0)
        {
            currentJumpTime -= Time.deltaTime;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            currentJumpTime = 0.0f;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    private void OnMove()
    {
        if (isDucking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            return;
        }

        rb.linearVelocity = new Vector2(movement, rb.linearVelocityY);
    }

    private void OnJump()
    {
        if (currentJumpTime > 0.1)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }
    }

    private void OnDuck()
    {
        if (Input.GetAxisRaw("Vertical") < 0 && OnGround())
        {
            col.size = new Vector2(col.size.x, 1.0692f);
            col.offset = new Vector2(col.offset.x, -0.4654f);
            isDucking = true;
        }
        else
        {
            col.offset = new Vector2(col.offset.x, -0.3364736f);
            col.size = new Vector2(col.size.x, 1.327053f);
            isDucking = false;
        }
    }

    private void LookMoveDirection()
    {
        if (movement > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (movement < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    public bool OnGround()
    {
        return Physics2D.OverlapBox(sensorGround.position, sensorSize, 0.0f, layerGround);
    }

    public float GetMovement()
    {
        return movement;
    }

    public float GetJumpSpeed()
    {
        return rb.linearVelocityY;
    }

    public bool GetDucking()
    {
        return isDucking;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Hurt(hurtTime));
        }
    }

    private IEnumerator Hurt(float t)
    {
        //pa.PlayAudio(pa.HURT, 0.3f);

        KnockbackApply(new Vector2(movement, transform.position.y));

        for (int i = 0; i < 1; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(t);
            sr.color = Color.white;
            yield return new WaitForSeconds(t);
        }
    }

    private void KnockbackApply(Vector2 hitDirection)
    {
        if (inKnockback) return;

        
    }

    private IEnumerator KnovkbackRecover(float t)
    {
        yield return new WaitForSeconds(t);
    }

    private void Attack()
    {
        GameObject attack = Instantiate(playerAttackPrefab);
        attack.transform.position = attackPoint.position;
        attack.transform.eulerAngles = transform.eulerAngles;
        attack.transform.parent = attackPoint.transform;

        //pa.PlayAudio(pa.HIT, 0.2f);
        Destroy(attack, timeToDestroyAttack);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(sensorGround.position, sensorSize);
    }
}
