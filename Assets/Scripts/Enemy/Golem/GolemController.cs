using UnityEngine;
using System.Collections;
using System;

public class GolemController : IEnemy
{
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
        throw new NotImplementedException();
    }

    public override void Attack(Transform target, Transform start)
    {
        throw new NotImplementedException();
    }
}
