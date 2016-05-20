using UnityEngine;
using System.Collections;
using System;

public class FireBallController : IProjectile {

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
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
}
