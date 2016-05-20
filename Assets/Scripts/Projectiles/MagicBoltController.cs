using UnityEngine;
using System.Collections;
using System;

public class MagicBoltController : IProjectile {
    private Animator anim;
    private BoxCollider2D bc;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        transform.position = start;        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float distanceTravelled = transform.position.x - start.x;
        if (distanceTravelled > maxDistance || distanceTravelled < (maxDistance * -1))
        {
            Destroy(transform.gameObject);
        }
	}

    public override void ReleaseProjectile()
    {
        float distance = end.x - start.x;
        float velocityHorizontal = speed;

        direction = Utils.Direction.RIGHT;
        if (distance < 0)
        {
            direction = Utils.Direction.LEFT;
            velocityHorizontal *= -1;
        }
        Flip(direction);
        rb2d.velocity = new Vector2(velocityHorizontal, rb2d.velocity.y);
    }

    protected new void OnCollisionEnter2D(Collision2D coll)
    {
        bool destroy = true;
        if (coll.gameObject.tag == "Player")
        {
            PlayerController pc = coll.gameObject.GetComponent<PlayerController>();
            if (source == pc.type)
            {
                destroy = false;
            }
            else
            {
                pc.AddDamage(damage);
            }
        }

        if (coll.gameObject.tag == "Enemy")
        {
            IEnemy ei = coll.gameObject.GetComponent<IEnemy>();
            if (source != ei.type)
            {
                ei.AddDamage(damage);
            }
            else
            {
                destroy = false;
            }
        }
        if (coll.gameObject.tag == "Projectile")
        {
            destroy = false;
        }

        if (destroy)
        {
            bc.enabled = false;
            transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y * 2, transform.localScale.z);
            anim.SetBool("hit", true);
            rb2d.velocity = new Vector2(0, 0);
        }
            
        
    }

    public void ExplosionTrigger()
    {
        Destroy(transform.gameObject);
    }
}
