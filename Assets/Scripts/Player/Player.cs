using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private StageData data;

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
    [SerializeField] private Transform playerAttackPosition;
    private bool onAttackCooldown;

    [Header("Bomb Plant Settings")]
    [SerializeField] private Transform bombPosition;

    private bool isDead;
    private bool onDeathScene;
    private bool isReadyToResume;
    private bool wasDeadByTime;
    private float deathPosition;
    private bool inSafeZone;
    private bool hasWon;

    private SpriteRenderer sr;
    private Collider2D col;
    private Rigidbody2D rb;
    private PlayerAudioPlayer ap;
    private PlayerAnimatorManager pam;
    private StageMusicPlayer sap;
    private VfxPoolManager pool;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        ap = GetComponent<PlayerAudioPlayer>();
        pam = GetComponent<PlayerAnimatorManager>();
        sap = GameObject.FindGameObjectWithTag("StageMusicPlayer").GetComponent<StageMusicPlayer>();
        pool = GameObject.FindGameObjectWithTag("VFXPool").GetComponent<VfxPoolManager>();
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
        if (isDead || onDeathScene || isReadyToResume || hasWon) return;

        movement = Input.GetAxisRaw("Horizontal") * speed; //Movimento horizontal
        fly = flySpeed; //Voar

        if (Input.GetButtonDown("Jump") && !onAttackCooldown) //Ataque
        {
            pam.Attack();
            ap.PlayAudio(ap.ATTACK);
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

        if (GameObject.FindGameObjectWithTag("Bomb") == null && GameObject.FindGameObjectWithTag("Explosion") == null)
        {
            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && 
                OnGround() && !inSafeZone &&
                movement == 0 && rb.gravityScale == 1 &&
                data.GetBombQuantity() > 0) //Plantar bomba
            {
                pam.BombPlant();
                ap.PlayAudio(ap.BOMB);
                pool.GetBomb(bombPosition.position, Quaternion.identity);
                data.RemoveBomb();
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

    private void FlyingCheck() //Detectar qual gravidade usar
    {
        if (!isDead && !onDeathScene && !isReadyToResume && !hasWon)
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

        pool.GetPlayerAttack(damage, playerAttackPosition.position, transform.rotation);
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
        return (!OnGround() || 
            (rb.linearVelocityY <= -1.5f || rb.linearVelocityY > 1.5f) || 
            currentTimeToFly >= 0.1f || onDeathScene);
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

    private void OnDarkRoom()
    {
        sr.color = new Color32(128, 128, 128, 255);
    }

    private void OnDarkRoomOut()
    {
        sr.color = Color.white;
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

    public void OnDeathByTime()
    {
        wasDeadByTime = true;
        Death();
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
        ap.PlayAudio(ap.DEATH);
        rb.gravityScale = 0.0f;
        rb.linearVelocity = Vector2.zero;
        currentTimeToFly = 0;
        movement = 0;
        fly = 0;
    }

    private IEnumerator FinalDeathTimer()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        print("Voltou pro menu");
        Time.timeScale = 1.0f;
        PlayerData.ResetData();
        PlayerData.ResetBonusLifeCounter();
        SceneManager.LoadScene("Stage1Scene");
    }

    private IEnumerator DeathByTimeTimer()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator DragPlayerToGroundForVictory(Chao chao)
    {
        movement = 0;
        fly = 0;
        rb.linearVelocity = Vector2.zero;

        while (!OnGround())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, -1 * flySpeed);
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = Vector2.zero;
        pam.Victory();
        ap.PlayAudio(ap.VICTORY);
        Time.timeScale = 0.0f;
        rb.gravityScale = 0.0f;

        chao.SuccessStage();
    }

    private void OnBecameInvisible()
    {
        if (!wasDeadByTime)
        {
            if (isDead)
            {
                if (PlayerData.life > 0)
                {
                    PlayerData.life -= 1;
                    if (isDead && onDeathScene)
                    {
                        transform.position = new Vector2(transform.position.x, Camera.main.transform.position.y + 7.0f);
                        pam.Revive();
                        isDead = false;
                    }
                }
                else
                {
                    StartCoroutine(FinalDeathTimer());
                }
            }
        }
        else
        {
            if (isDead)
            {
                if (PlayerData.life > 0)
                {
                    PlayerData.life -= 1;
                    StartCoroutine(DeathByTimeTimer());
                }
                else
                {
                    StartCoroutine(FinalDeathTimer());
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SafeZone"))
        {
            inSafeZone = true;
        }

        if (collision.CompareTag("Lamp"))
        {
            collision.GetComponent<Lamp>().LightsOut();
        }

        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Death();
        }

        if (collision.CompareTag("Explosion"))
        {
            Death();
        }

        if (collision.CompareTag("Chao"))
        {
            hasWon = true;
            PlayerData.AddScore(collision.GetComponent<Chao>().GetScoreValue());
            currentTimeToFly = 0;
            StartCoroutine(DragPlayerToGroundForVictory(collision.GetComponent<Chao>()));

            if (sap != null)
            {
                sap.PlayAudio(sap.VICTORY);
            }

            StartCoroutine(data.ScoreCollector());
        }

        if (collision.CompareTag("CameraSwitcher"))
        {
            collision.GetComponent<CameraPositionManager>().ChangeCameraPosition();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Darkness"))
        {
            OnDarkRoom();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SafeZone"))
        {
            inSafeZone = false;
        }

        if (collision.CompareTag("Darkness"))
        {
            OnDarkRoomOut();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MagmaWall"))
        {
            Death();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = sensorColor;
        Gizmos.DrawCube(sensorGround.position, size);
    }
}
