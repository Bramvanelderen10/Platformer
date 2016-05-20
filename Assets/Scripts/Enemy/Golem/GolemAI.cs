using UnityEngine;
using System.Collections;
using System;
using System.Timers;
using System.Collections.Generic;

public class GolemAI : AiInterface {

    public float walkDistance = 30f;
    public float speed = 500f;
    public Utils.Direction direction = Utils.Direction.LEFT;
    public GameObject ProjectilePrefab;
    public Transform shootStart;
    public float[] projectileDirectionsOffset = new float[3];


    public Int32 cooldown = 4;
    private Int32 cooldownTimer = 0;

    private List<IProjectile> projectiles = new List<IProjectile>();

    private System.Timers.Timer timer;

    void Start()
    {
        cc = GetComponent<CollisionChecker>();
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(0f, 0f);

        anim.Play("GolemRun");
    }

    protected override void Move()
    {
        if (!isAttacking)
        {
            anim.Play("GolemRun");
            if (!cc.CheckGround(direction) || cc.CheckWall(direction))
            {
                direction = (direction == Utils.Direction.RIGHT) ? Utils.Direction.LEFT : Utils.Direction.RIGHT;
            }

            float walkSpeed = (direction == Utils.Direction.RIGHT) ? speed : speed * -1;
            rb2d.velocity = new Vector2(walkSpeed * Time.deltaTime, rb2d.velocity.y);

            Flip(direction);
        } else
        {
            rb2d.velocity = new Vector2(0, 0);
        }
        
    }

    protected override void Attack()
    {
        if (!isAttacking && (Utils.GetUnixTime() - cooldownTimer) > cooldown)
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, 7f, 1 << LayerMask.NameToLayer("Player"));
            if (collider)
            {
                float distance = shootStart.position.x - collider.transform.position.x;

                if (direction == Utils.Direction.RIGHT && distance > 0)
                    direction = Utils.Direction.LEFT;
                if (direction == Utils.Direction.LEFT && distance <= 0)
                    direction = Utils.Direction.RIGHT;

                Flip(direction);

                isAttacking = true;
                cooldownTimer = Utils.GetUnixTime();
                Transform destination = collider.transform;
                foreach (float offset in projectileDirectionsOffset)
                {
                    Vector3 vector = destination.position;
                    vector.y += offset;

                    GameObject go = UnityEngine.Object.Instantiate(ProjectilePrefab);
                    IProjectile fb = go.GetComponent<IProjectile>();
                    fb.start = new Vector3(shootStart.position.x, shootStart.position.y, shootStart.position.z);
                    fb.end = vector;
                    fb.source = type;
                    projectiles.Add(fb);
                }
                anim.Play("GolemAttack");     
                

                timer = new System.Timers.Timer();
                timer.Interval = 1000;
                timer.Elapsed += StopAttacking;
                timer.Enabled = true;
            }
        }
    }

    private void StopAttacking(object sender, ElapsedEventArgs e)
    {
        isAttacking = false;
        timer.Enabled = false;
    }

    public void FireProjectile()
    {
        foreach (RockController rc in projectiles)
        {
            rc.ManualStart();
        }
        projectiles = new List<IProjectile>();
    }
}
