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

    [Header("Bomb Plant Settings")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform bombPosition;

    private bool isDead;
    private bool onDeathScene;
    private bool isReadyToResume;
    private float deathPosition;

    private SpriteRenderer sr;
    private Collider2D col;
    private Rigidbody2D rb;
    private PlayerAnimatorManager pam;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        pam = GetComponent<PlayerAnimatorManager>();
    }
    
    void Update()
    {
        PlayerInputs();

        DirectionLook();

        FlyingCheck();

        DeathSystem();
    }

    private void FixedUpdate()
    {
        OnMove();
        OnFly();
    }

    private void PlayerInputs()
    {
        if (isDead || onDeathScene || isReadyToResume) return;

        movement = Input.GetAxisRaw("Horizontal") * speed; //Movimento horizontal
        fly = flySpeed; //Voar

        if (Input.GetButtonDown("Jump") && !onAttackCooldown) //Ataque
        {
            pam.Attack();
            StartCoroutine(OnAttack());
            StartCoroutine(AttackCooldown());
        }

        //Mecânica de Vôo
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) //ativar gravidade 0 para o vôo
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            currentTimeToFly = 0.11f;
        }
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))) //Timer para liberar vôo
        {
            currentTimeToFly += Time.deltaTime;
        }
        else if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))) //Travando o vôo
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            currentTimeToFly = timeToFly;
        }

        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && currentTimeToFly > 0.1f) //Diminuindo o timer para começar a cair
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

        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && OnGround() && movement == 0 && rb.gravityScale == 1) //Plantar bomba
        {
            pam.BombPlant();
            Instantiate(bombPrefab, bombPosition.position, Quaternion.identity);
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

    private void FlyingCheck() //Detectar qual gravidade usar
    {
        if (!isDead && !onDeathScene && !isReadyToResume)
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

    private IEnumerator AttackCooldown() //Cooldown para não ser possível spammar o ataque e assim bugar a animação
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

    public bool IsFlying() //Detectar para saber se precisa ou não estar na animação de vôo
    {
        return (!OnGround() || rb.linearVelocityY != 0.0f || currentTimeToFly >= 0.1f || onDeathScene);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetTime()
    {
        return Time.timeScale;
    }

    public bool OnAttackCooldown()
    {
        return onAttackCooldown;
    }

    private void DeathSystem()
    {
        if (isDead)
        {
            OnDeath();
        }

        if (!isDead && onDeathScene)
        {
            OnRevive();
        }

        if (isReadyToResume)
        {
            ResumeAfterDeath();
        }

        if (onDeathScene && !isDead)
        {
            if (transform.position.y <= deathPosition)
            {
                onDeathScene = false;
                isReadyToResume = true;
                col.enabled = true;
            }
        }
    }

    private void ResumeAfterDeath()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            isReadyToResume = false;
            onDeathScene = false;
            Time.timeScale = 1.0f;
        }
    }

    private void OnDeath()
    {
        transform.Translate(6.0f * Time.unscaledDeltaTime * Vector2.down);
    }

    private void OnRevive()
    {
        transform.Translate(3.0f * Time.unscaledDeltaTime * Vector2.down);
    }

    private void Death()
    {
        deathPosition = transform.position.y;
        onDeathScene = true;
        Time.timeScale = 0.0f;
        col.enabled = false;
        isDead = true;
        pam.Death();
        rb.gravityScale = 0.0f;
        rb.linearVelocity = Vector2.zero;
        currentTimeToFly = 0;
        movement = 0;
        fly = 0;
    }

    private void OnBecameInvisible()
    {
        if (isDead && onDeathScene)
        {
            transform.position = new Vector2(transform.position.x, 12.0f);
            isDead = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Death();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = sensorColor;
        Gizmos.DrawCube(sensorGround.position, size);
    }
}
