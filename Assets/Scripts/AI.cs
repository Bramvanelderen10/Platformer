using UnityEngine;
using System.Collections;
using System;

public class AI : MonoBehaviour {

    private enum Detection
    {
        EDGE_LEFT, //Only grounded on left side
        EDGE_RIGHT, //Only grounded on right side
        GROUNDED, //Fully Grounded
        IN_AIR, //Fully in air
    }

    public enum Strategy
    {
        FOLLOW,
        PATROL,
        IDLE,
    }

    public enum Hostility
    {
        FIRE_ON_SIGHT,
        FIRE_IN_RANGE,
        NON_HOSTILE
    }

    private enum Direction
    {
        LEFT,
        RIGHT
    }

    public GameObject[] platforms;    
    public GameObject target;
    public float movespeed = 5f;
    public float viewDistance = 13f;
    public Hostility hostility;
    public Strategy strategy;
    public Transform groundCheckLeft, groundCheckRight;
    public Transform[] wallChecksLeft;
    public Transform[] wallChecksRight;

    
    public GameObject spriteContainer;
    public Transform shootStart, shootEnd;

    public string runState, idleState, attackState;

    public bool isFacingRight = true;

    private Rigidbody2D rb2d;
    private Animator anim;
    private IEnemy enemy;
    private bool pauseAI = false;

    private Transform temp_target;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemy = GetComponent<IEnemy>();

        anim.Play(idleState);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {

        DetectEnemy();

        if (strategy == Strategy.PATROL)
            Patrol();

        if (strategy == Strategy.FOLLOW)
            Follow();
        
    }
    
    void DetectEnemy()
    {
        if (DetectGrounded() != Detection.IN_AIR)
        {
            if (hostility == Hostility.FIRE_ON_SIGHT)
            {
                float distance = (isFacingRight) ? viewDistance : viewDistance * -1;

                Vector3 start = shootStart.position;
                Vector3 end = new Vector3(start.x + distance, start.y);
                RaycastHit2D rc2d = Physics2D.Linecast(start, end, 1 << LayerMask.NameToLayer("Player"));
                if (rc2d)
                {
                    temp_target = rc2d.collider.gameObject.transform;

                    string stateName = enemy.GetAttackAnimation(temp_target);
                    if (stateName != null)
                    {
                        pauseAI = true;
                        rb2d.velocity = new Vector2(0, 0);
                        anim.Play(stateName);
                    }
                }
            }

            if (hostility == Hostility.FIRE_IN_RANGE)
            {                
                Collider2D collider = Physics2D.OverlapCircle(transform.position, viewDistance, 1 << LayerMask.NameToLayer("Player"));
                if (collider)
                {
                    float distance = collider.transform.position.x - transform.position.x;
                    temp_target = collider.gameObject.transform;

                    string stateName = enemy.GetAttackAnimation(temp_target);
                    if (stateName != null)
                    {
                        if ((isFacingRight && distance < 0) || (!isFacingRight && distance > 0))
                            Flip();

                        pauseAI = true;
                        rb2d.velocity = new Vector2(0, 0);
                        anim.Play(stateName);
                    }
                }
            }
        } 
    }

    /*  
    * Patrol across platforms
    * Call this in FIXED UPDATE!
    */
    void Patrol()
    {
        DetectWallCollision(true);
        Move(true);
    }

    void Follow()
    {
        Vector3 targetPosition = target.transform.position;
        if (Mathf.Abs(targetPosition.x - transform.position.x) < viewDistance)
        {
            Direction direction = (targetPosition.x - transform.position.x > 0) ? Direction.RIGHT : Direction.LEFT;

            //Flip if facing the wrong direction
            if ((isFacingRight && direction == Direction.LEFT) || (!isFacingRight) && direction == Direction.RIGHT)
                Flip();
            
            Move(false);
            DetectWallCollision(false);
        } else
        {
            anim.Play(idleState);
        }
            
    }

    void Move(bool flipOnCollision)
    {
        if (!pauseAI)
        {
            Detection detected = DetectGrounded();

            //Check if walking to right and if ground stops on right side        
            if (isFacingRight && detected == Detection.EDGE_LEFT)
            {
                GameObject selectedPlatform = null;
                foreach (GameObject platform in platforms)
                {
                    Vector3 platformPosition = platform.transform.position;

                    if (platformPosition.x <= transform.position.x)
                        continue;


                    if (!selectedPlatform || selectedPlatform.transform.position.x > platform.transform.position.x)
                    {
                        //Only select platform if distance is not to big (10f)
                        if (Mathf.Abs(platform.transform.position.x - transform.position.x) < 10f)
                            selectedPlatform = platform;
                    }
                }

                //Flip and move if next platform is not found
                if (!selectedPlatform)
                {
                    if (flipOnCollision)
                    {
                        Flip();
                        rb2d.velocity = new Vector2(GetMovementSpeed(), rb2d.velocity.y);

                        anim.Play(runState);

                        pauseAI = true;
                        StartCoroutine(ReEnableAI(0.1f));
                    }
                    else
                    {
                        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                        anim.Play(idleState);
                    }
                }

                //Jump to next platform if found
                if (selectedPlatform)
                {
                    Vector3 end = selectedPlatform.transform.position;
                    Vector3 start = transform.position;
                    float arrivalTime = 1f;
                    Vector2 velocity = new Vector2((end.x - start.x) / arrivalTime, end.y - start.y / arrivalTime + 0.5f * Mathf.Abs(Physics2D.gravity.y) * arrivalTime * arrivalTime);
                    rb2d.velocity = new Vector2(velocity.x, velocity.y + 1f);

                    anim.Play(runState);

                    pauseAI = true;
                    StartCoroutine(ReEnableAI(0.2f));
                }

            } else

            //COPY PASTA
            //Check if walking to right and if ground stops on right side        
            if (!isFacingRight && detected == Detection.EDGE_RIGHT)
            {
                GameObject selectedPlatform = null;
                foreach (GameObject platform in platforms)
                {
                    Vector3 platformPosition = platform.transform.position;

                    if (platformPosition.x > transform.position.x)
                        continue;


                    if (!selectedPlatform || selectedPlatform.transform.position.x < platform.transform.position.x)
                    {
                        //Only select platform if distance is not to big (10f)
                        if (Mathf.Abs(platform.transform.position.x - transform.position.x) < 10f)
                            selectedPlatform = platform;
                    }
                }

                //Flip and move if next platform is not found
                if (!selectedPlatform)
                {
                    if (flipOnCollision)
                    {
                        Flip();
                        rb2d.velocity = new Vector2(GetMovementSpeed(), rb2d.velocity.y);

                        anim.Play(runState);

                        pauseAI = true;
                        StartCoroutine(ReEnableAI(0.1f));
                    }
                    else
                    {
                        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                        anim.Play(idleState);
                    }                    
                }

                //Jump to next platform if found
                if (selectedPlatform)
                {
                    Vector3 end = selectedPlatform.transform.position;
                    Vector3 start = transform.position;
                    float arrivalTime = 1f;
                    Vector2 velocity = new Vector2((end.x - start.x) / arrivalTime, end.y - start.y / arrivalTime + 0.5f * Mathf.Abs(Physics2D.gravity.y) * arrivalTime * arrivalTime);
                    rb2d.velocity = new Vector2(velocity.x, velocity.y + 1f);

                    anim.Play(runState);

                    pauseAI = true;
                    StartCoroutine(ReEnableAI(0.2f));
                }
            }
            else 
            //END

            if (detected == Detection.GROUNDED)
            {
                rb2d.velocity = new Vector2(GetMovementSpeed(), rb2d.velocity.y);
                anim.Play(runState);
            }
            else if (detected != Detection.IN_AIR)
            {
                rb2d.velocity = new Vector2(GetMovementSpeed(), rb2d.velocity.y);
                anim.Play(runState);
            }
        }
    }



    float GetMovementSpeed()
    {
        return (isFacingRight) ? movespeed : movespeed * -1;
    }

    Detection DetectGrounded()
    {
        bool left = false;
        bool right = false;

        Vector3 groundPosition = groundCheckLeft.position;
        Vector3 position = new Vector3(groundPosition.x, transform.position.y);
        left = Physics2D.Linecast(position, groundPosition, 1 << LayerMask.NameToLayer("Ground"));

        groundPosition = groundCheckRight.position;
        position = new Vector3(groundPosition.x, transform.position.y);
        right = Physics2D.Linecast(position, groundPosition, 1 << LayerMask.NameToLayer("Ground"));

        Detection detected;
        if (left && right)
        {
            detected = Detection.GROUNDED;
        } else if (left && !right)
        {
            detected = Detection.EDGE_LEFT;
        } else if (right && !left)
        {
            detected = Detection.EDGE_RIGHT;
        } else
        {
            detected = Detection.IN_AIR;
        }

        return detected;
    }


    // RECURSION METHOD
    void DetectWallCollision(bool flipOnCollision, bool first = true)
    {
        if (isFacingRight)
        {
            bool collide = false;
            foreach (Transform wallCheck in wallChecksRight)
            {
                Vector3 wallPosition = wallCheck.position;
                Vector3 position = new Vector3(transform.position.x, wallPosition.y);
                if (Physics2D.Linecast(position, wallPosition, 1 << LayerMask.NameToLayer("Ground")))
                    collide = true;
            }

            if (collide)
            {
                if (flipOnCollision)
                {
                    Flip();
                } else
                {
                    //Go up untill no wall collision is detected anymore
                    Vector3 position = transform.position;
                    if (first)
                    {
                        position.x = (isFacingRight) ? position.x + 1f : position.x - 1f;
                    }

                    position.y += 0.5f;
                    transform.position = position;
                    DetectWallCollision(false, false);  
                }
            } 
        }

        if (!isFacingRight)
        {
            bool collide = false;
            foreach (Transform wallCheck in wallChecksLeft)
            {
                Vector3 wallPosition = wallCheck.position;
                Vector3 position = new Vector3(transform.position.x, wallPosition.y);
                if (Physics2D.Linecast(position, wallPosition, 1 << LayerMask.NameToLayer("Ground")))
                    collide = true;
            }

            if (collide)
            {
                if (flipOnCollision)
                {
                    Flip();
                }
                else
                {
                    //Go up untill no wall collision is detected anymore
                    Vector3 position = transform.position;
                    if (first)
                    {
                        position.x = (isFacingRight) ? position.x + 1f : position.x - 1f;
                    }

                    position.y += 0.5f;
                    transform.position = position;
                    DetectWallCollision(false, false);
                }
            }
        }
    }

    void Flip()
    {
        Vector3 scale = spriteContainer.transform.localScale;
        scale.x *= -1;
        spriteContainer.transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }
    
    IEnumerator ReEnableAI(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResumeAi();
    }

    void ResumeAi()
    {
        pauseAI = false;
        anim.Play(idleState);
    }

    public void Attack()
    {
        enemy.Attack(temp_target, shootStart);
        temp_target = null;
    }    
}
