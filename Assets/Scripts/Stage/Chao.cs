using UnityEngine;

public class Chao : MonoBehaviour
{
    private Animator anim;
    private bool success;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("pSuccess", success);
    }

    public void SuccessStage()
    {
        success = true;
    }
}
