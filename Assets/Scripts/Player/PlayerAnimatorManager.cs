using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    private Animator anim;
    private Player player;


    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetInteger("pMove", (int)player.GetDirection());
        anim.SetBool("pOnGround", player.OnGround());
        anim.SetBool("pFlying", player.IsFlying());
        anim.SetBool("pDead", player.IsDead());
    }

    public void Attack()
    {
        anim.SetTrigger("pAttack");
    }

    public void BombPlant()
    {
        anim.SetTrigger("pBomb");
    }

    public void Death() {
        anim.SetTrigger("pDeath");
    }

    public void Revive()
    {
        anim.SetTrigger("pRevived");
    }

    public void Victory()
    {
        anim.SetTrigger("pVictory");
    }
}
