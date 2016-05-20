using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour {

    private enum JumpDirection
    {
        RIGHT,
        LEFT,
        UNDEFINED
    }

    private enum Wall
    {
        RIGHT,
        LEFT
    }

    public CollisionChecker wallChecker;

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool canJump = false;

    public float moveForce = 365f;
    public float movementSpeed = 10f;
    public float jumpSpeed = 3f;
    public float jumpVelocity = 12f;
    public int maxJumps = 2;

    private bool wallLeft;
    private bool wallRight;

    private float maxVelocity;
    private int jumpCounter = 0;
    private float jumpTime = 0;
    private bool isJumping = false;

    private bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;
    
    private float maxAccelerationSpeed;
    private float currentVelocity = 0f;


    // Use this for initialization
    void Awake () {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        canJump = true;
        anim.Play("PlayerRun");

        
    }
    void Start()
    {
        maxVelocity = 1 * movementSpeed;
        //Trigger update every 0.2 seconds
        //This determines how fast the player can accelerate from 0 to max speed
        System.Timers.Timer timer = new System.Timers.Timer();
        timer.Interval = 100;
        timer.Elapsed += UpdateMaxSpeed;
        timer.Enabled = true;
    }
	
	// Update is called once per frame
	void Update () {       
        grounded = false;
        if (!wallChecker.IsNearWalls())
        {
                grounded = wallChecker.IsGrounded();
        }
        switch (wallChecker.CheckSidesForWalls())
        {
            case Utils.Direction.RIGHT:
                grounded = wallChecker.CheckGround(Utils.Direction.LEFT);
                break;
            case Utils.Direction.LEFT:
                grounded = wallChecker.CheckGround(Utils.Direction.RIGHT);
                break;
            case Utils.Direction.BOTH:
                grounded = wallChecker.CheckGround(Utils.Direction.BOTH);
                break;
        }
        if (grounded && (Time.time - jumpTime) > 0.2)
        {
            jumpCounter = 0;
            canJump = true;
            isJumping = false;
            wallLeft = false;
            wallRight = false;
        }        
        if (!grounded)
        {
            wallLeft = (facingRight) ? wallChecker.CheckWall(Utils.Direction.LEFT) : wallChecker.CheckWall(Utils.Direction.RIGHT);
            wallRight = (facingRight) ? wallChecker.CheckWall(Utils.Direction.RIGHT) : wallChecker.CheckWall(Utils.Direction.LEFT);
            
            if (rb2d.velocity.x > 0 && wallRight)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y / 2);
            }
            if (rb2d.velocity.x < 0 && wallLeft)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y / 2);
            }
        }
        if (Input.GetButtonDown("jump"))
        {
            //If wall jump dont count jump
            if (wallChecker.CheckSidesForWalls() != Utils.Direction.NONE
                && !grounded
                )
            {
                //Set Timer so other animitions wont get played
                jumpTime = Time.time;
                //Do jump

                anim.SetBool("WallJump", true);

                float velocity = movementSpeed * 2;
                if (!facingRight)
                    velocity *= -1;

                if (wallChecker.CheckSidesForWalls() == Utils.Direction.RIGHT)
                {
                    velocity *= -1;
                    
                }
                rb2d.velocity = new Vector2(velocity, jumpVelocity * 1.2f);

                jumpCounter = 1;
                canJump = true;
            } else if (canJump)
            {
                //Set Timer so other animitions wont get played
                jumpTime = Time.time;
                //Do jump
                anim.Play("PlayerJump");
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpVelocity);


                isJumping = true;
                jumpCounter++;
                //Check if maximum jumps in a row has been reached
                if (jumpCounter == maxJumps)
                    canJump = false;
            }   
        }
    }

    public void EndWallJump()
    {
        anim.SetBool("WallJump", false);
        anim.Play("PlayerFallOnly");
    }

    void FixedUpdate()
    {
        float input = 0;
        if (Input.GetButton("left"))
        {
            input -= 1;
        }
        if (Input.GetButton("right"))
        {
            input += 1;
        }


        float axis = Input.GetAxis("horizontal");
        float h;
        if (input + axis > 1)
        {
            h = 1;
        } else if (input + axis < -1)
        {
            h = -1;
        } else
        {
            h = input + axis;
        }

        if (h > 0 && wallRight)
        {
            h = 0;
        }
        if (h < 0 && wallLeft)
        {
            h = 0;
        }

        if (h == 0 && grounded && (Time.time - jumpTime) > 0.2)
        {
            anim.Play("PlayerIdle");
        }

        if (h != 0 && grounded && (Time.time - jumpTime) > 0.2)
        {
            anim.Play("PlayerRun");
        }

        if (!grounded && !isJumping)
        {
            anim.Play("PlayerFall");
        }
        
        if (grounded)
        {
            float velocity = h * movementSpeed;
            velocity = GetVelocity(velocity);
            rb2d.velocity = new Vector2(velocity, rb2d.velocity.y);            
        } else
        {

            if (h != 0)
            {
                rb2d.AddForce(new Vector2(h * 1000 * jumpSpeed * Time.deltaTime, 0));
            }
            else
            {
                Vector2 vel = rb2d.velocity;
                if (vel.x > 0)
                {
                    vel.x -= 0.8f;
                    if (vel.x < 0)
                    {
                        vel.x = 0;
                    }                                    
                }
                if (vel.x < 0)
                {
                    vel.x += 0.8f;
                    if (vel.x > 0)
                    {
                        vel.x = 0;
                    }
                }
                rb2d.velocity = vel;
            }            
        }

        if (rb2d.velocity.x > maxVelocity)
            {
                rb2d.velocity = new Vector2(maxVelocity, rb2d.velocity.y);
            }

            if (rb2d.velocity.x < (maxVelocity * -1))
            {
                rb2d.velocity = new Vector2((maxVelocity * -1), rb2d.velocity.y);
            }

            //Check if flip is needed
            Flip(h);

            currentVelocity = rb2d.velocity.x;
        }
    
    private float GetVelocity(float velocity)
    {
        if (velocity > 0 && velocity > maxAccelerationSpeed)
        {
            velocity = maxAccelerationSpeed;
        }

        if (velocity < 0 && velocity < (maxAccelerationSpeed * -1))
        {
            velocity = maxAccelerationSpeed * -1;
        }

        return velocity;
    }

    public void Flip(float direction)
    {
        if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
        {
            facingRight = !facingRight;
            wallChecker.facingRight = facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    //UpdateMaxSpeed will determine what the current max speed is for the player for the next interval
    //Is multithreaded
    private void UpdateMaxSpeed(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        if (currentVelocity == 0)
        {
            maxAccelerationSpeed = maxVelocity / 5;
        }
        else
        {
            maxAccelerationSpeed *= 2.2F;
        }

        if (maxAccelerationSpeed > maxVelocity)
            maxAccelerationSpeed = maxVelocity;

        if (maxAccelerationSpeed < (maxVelocity * -1))
            maxAccelerationSpeed = (maxVelocity * -1);
    }
}
