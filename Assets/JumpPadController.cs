using UnityEngine;
using System.Collections;

public class JumpPadController : MonoBehaviour {

    public Transform CheckUp;
    public float jumpVelocity = 20f;

    private Animator anim;
    private Rigidbody2D playerRb2d;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        playerRb2d = null;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        RaycastHit2D rc2d = Physics2D.Linecast(transform.position, CheckUp.position, 1 << LayerMask.NameToLayer("Player"));
        if (rc2d)
        {
            anim.Play("JumpPadBoost");
        }
	}

    public void ExecuteBoost()
    {
        RaycastHit2D rc2d = Physics2D.Linecast(transform.position, CheckUp.position, 1 << LayerMask.NameToLayer("Player"));
        if (rc2d)
        {
            GameObject player = rc2d.transform.gameObject;
            Rigidbody2D rb2d = player.GetComponent<Rigidbody2D>();
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity);
        }
    }

    public void ReturnToIdle()
    {
        anim.SetBool("Jump", false);
    }
}
