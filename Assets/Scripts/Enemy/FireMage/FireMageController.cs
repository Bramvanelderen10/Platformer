using UnityEngine;
using System.Collections;
using System;

public class FireMageController : IEnemy
{
    public float rangedAttackRange = 13f;
    public float meleeAttackRange = 1f;

    public string meleeState, rangeState;

    public GameObject projectilePrefab;

    public float cooldownMelee = 1f, cooldownRanged = 3f;
    private float cooldownTimer;
    private float cooldown;

    private bool isRangedAttack = false;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void AddDamage(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
            Kill();
    }

    protected override void Kill()
    {
        Destroy(transform.gameObject);
    }

    public override string GetAttackAnimation(Transform target)
    {
        float cooldown = (isRangedAttack) ? cooldownRanged : cooldownMelee;
        if (Time.time - cooldownTimer < cooldown)
        {
            return null;
        }
        cooldownTimer = Time.time;

        float distance = Mathf.Abs(Vector2.Distance(target.position, transform.position));

        string animation = null;
        if (distance <= rangedAttackRange && distance > meleeAttackRange)
        {
            animation = rangeState;
            isRangedAttack = true;
        } else if (distance <= meleeAttackRange)
        {
            animation = meleeState;
            isRangedAttack = false;
        }

        return animation;
    }

    public override void Attack(Transform target, Transform start)
    {
        Vector2 destination = new Vector2(target.position.x, target.position.y);

        GameObject go = Instantiate(projectilePrefab);
        IProjectile fb = go.GetComponent<IProjectile>();
        fb.maxDistance = Mathf.Abs(Vector2.Distance(target.position, start.position) + 2f);
        fb.start = start.position;
        fb.end = destination;
        fb.source = type;
        fb.ReleaseProjectile();
    }
}
