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
    }

    public void Attack()
    {
        anim.SetTrigger("pAttack");
    }
}
