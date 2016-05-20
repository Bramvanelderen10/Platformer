using UnityEngine;
using System.Collections;
using System;
using System.Timers;
using System.Collections.Generic;

public class FireMageAI : AiInterface {

    public float walkDistance = 30f;
    public float speed = 10f;
    public Utils.Direction direction = Utils.Direction.LEFT;
    public GameObject ProjectilePrefab;
    public Transform shootStart;
    public float shootDistance = 20f;
    public float meleeDistance = 1f;

    public float jumpDistance = 4f;

    public float cooldown = 4f;
    private float cooldownTimer = 0;

    void Start()
    {
        cc = GetComponent<CollisionChecker>();
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(0f, 0f);

        anim.Play("FireMageIdle");
    }

    protected override void Move()
    {
        if (!isAttacking)
        {
            if (cc.CheckGround(Utils.Direction.BOTH))
            {
                Collider2D collider = Physics2D.OverlapCircle(transform.position, 7f, 1 << LayerMask.NameToLayer("Player"));
                
                if (collider)
                {

                    Vector3 target = collider.transform.position;

                    Utils.Direction enemyDirection;
                    Vector2 velocity = new Vector2(0, 0);

                    //if target is right
                    if (target.x - transform.position.x > 0)
                    {
                        enemyDirection = Utils.Direction.RIGHT;

                        //If grounded and no wall in front
                        if (cc.CheckGround(enemyDirection) || !cc.CheckWall(enemyDirection))
                        {
                            //If wall is incoming jump on wall
                            RaycastHit2D[] raycasts = new RaycastHit2D[3];

                            raycasts[0] = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y + 1f), new Vector2(transform.position.x + 1.5f, transform.position.y + 1f));
                            raycasts[1] = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x + 1.5f, transform.position.y + 0f));
                            raycasts[2] = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y - 1f), new Vector2(transform.position.x + 1.5f, transform.position.y - 1f));


                            velocity.x = speed;
                            velocity.y = rb2d.velocity.y;
                        }

                        if (!cc.CheckGround(enemyDirection) && !cc.CheckWall(enemyDirection))
                        {
                            Vector2 angle = new Vector2(transform.position.x, transform.position.y);
                            RaycastHit2D[] results = Physics2D.CircleCastAll(transform.position, jumpDistance, Vector2.right, 1f, 1 << LayerMask.NameToLayer("Ground"));
                            Vector3 targetVector = new Vector3(0, 0, transform.position.z);
                            bool platformFound = false;
                            foreach (RaycastHit2D result in results)
                            {
                                //Is in the direction of the target
                                if (transform.position.x - result.point.x < 0)
                                {
                                    platformFound = true;
                                    targetVector.x = result.point.x + 2f;
                                    targetVector.y = result.point.y + 2f;
                                    targetVector.z = transform.position.z;
                                    break;
                                }
                            }

                            if (platformFound)
                            {
                                float arrivalTime = 1f;
                                velocity = new Vector2((targetVector.x - transform.position.x) / arrivalTime, targetVector.y - transform.position.y / arrivalTime + 0.5f * Mathf.Abs(Physics2D.gravity.y) * arrivalTime * arrivalTime);
                            }
                        }
                    }

                    //If Target is left
                    if (target.x - transform.position.x < 0)
                    {
                        enemyDirection = Utils.Direction.LEFT;

                        if (cc.CheckGround(enemyDirection) || !cc.CheckWall(enemyDirection))
                        {
                            velocity.x = speed * -1;
                            velocity.y = rb2d.velocity.y;
                        }

                        if (!cc.CheckGround(enemyDirection) && !cc.CheckWall(enemyDirection))
                        {
                            Vector2 angle = new Vector2(transform.position.x, transform.position.y);
                            RaycastHit2D[] results = Physics2D.CircleCastAll(transform.position, jumpDistance, Vector2.left, 1f, 1 << LayerMask.NameToLayer("Ground"));
                            Vector3 targetVector = new Vector3(0, 0, transform.position.z);
                            bool platformFound = false;
                            foreach (RaycastHit2D result in results)
                            {
                                //Is in the direction of the target
                                if (Vector2.Distance(transform.position, result.point) > 0)
                                {
                                    platformFound = true;
                                    targetVector.x = result.point.x - 2f;
                                    targetVector.y = result.point.y + 3f;
                                    targetVector.z = transform.position.z;
                                    break;
                                }
                            }

                            if (platformFound)
                            {
                                float arrivalTime = 1f;
                                velocity = new Vector2((targetVector.x - transform.position.x) / arrivalTime, targetVector.y - transform.position.y / arrivalTime + 0.5f * Mathf.Abs(Physics2D.gravity.y) * arrivalTime * arrivalTime);
                            }
                        }
                    }


                    rb2d.velocity = velocity;

                    if (velocity.x != 0 || velocity.y != 0)
                    {
                        anim.Play("FireMageRun");
                    }
                    float walkspeed = ((target.x - transform.position.x) > 0) ? speed : speed * -1;

                }
            }
            

            //if (cc.CheckGround(Utils.Direction.BOTH))
            //{
            //    anim.Play("FireMageRun");
            //    if (!cc.CheckGround(direction) || cc.CheckWall(direction))
            //    {
            //        direction = (direction == Utils.Direction.RIGHT) ? Utils.Direction.LEFT : Utils.Direction.RIGHT;
            //    }

            //    float walkSpeed = (direction == Utils.Direction.RIGHT) ? speed : speed * -1;
            //    rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
            //}

            //Flip(direction);

        }
        else
        {
            rb2d.velocity = new Vector2(0, 0);
        }
        
    }

    protected override void Attack()
    {

        if (!isAttacking && (Time.time - cooldownTimer) > cooldown && cc.CheckGround(Utils.Direction.BOTH))
        {

            float distance = (direction == Utils.Direction.LEFT) ? shootDistance * -1: shootDistance;
            Vector2 destination = new Vector2(shootStart.position.x + distance, shootStart.position.y);
            RaycastHit2D[] results = Physics2D.LinecastAll(shootStart.position, destination);
            bool hit = false;
            GameObject player = null;
            foreach (RaycastHit2D result in results)
            {
                if (result.transform.gameObject.CompareTag("Player"))
                {
                    hit = true;
                    player = result.transform.gameObject;
                }
            }

            if (hit && player !=  null)
            {               
                float distanceToPlayer = shootStart.position.x - player.transform.position.x;
                distanceToPlayer = (direction == Utils.Direction.LEFT) ? distanceToPlayer * -1 : distanceToPlayer;

                if (distanceToPlayer > meleeDistance || distanceToPlayer < (meleeDistance * -1)) {                  


                    anim.Play("FireMageRangeAttack");
                    isAttacking = true;
                }

            }  
        }
    }

    public void FireProjectile()
    {
        float distance = (direction == Utils.Direction.LEFT) ? shootDistance * -1 : shootDistance;

        Vector2 destination = new Vector2(shootStart.position.x + distance, shootStart.position.y);

        GameObject go = Instantiate(ProjectilePrefab);
        IProjectile fb = go.GetComponent<IProjectile>();
        fb.maxDistance = Mathf.Abs(distance);
        fb.start = shootStart.position;
        fb.end = destination;
        fb.source = type;
        fb.ReleaseProjectile();

        cooldownTimer = Time.time;
    }

    public void StopAttacking()
    {
        isAttacking = false;
        anim.Play("FireMageIdle");
    }
}
