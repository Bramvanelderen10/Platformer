using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class RockController : IProjectile
{
    public float arrivalTime = 0.7f;

    // Use this for initialization
    void Start()
    {
        //rb2d = transform.GetComponent<Rigidbody2D>();
        //transform.position = start;
        //float distance = end.x - start.x;

        //float multiplier = (arrivalTime * 40) / (distance * 4);
        //Vector2 velocity = new Vector2((end.x - start.x) / arrivalTime, end.y - start.y / arrivalTime + 0.5f * Mathf.Abs(Physics2D.gravity.y) * arrivalTime * arrivalTime);
        //rb2d.velocity = velocity;
    }

    public void ManualStart()
    {
        rb2d = transform.GetComponent<Rigidbody2D>();
        transform.position = start;
        float distance = end.x - start.x;        
        Vector2 velocity = new Vector2((end.x - start.x) / arrivalTime, end.y - start.y / arrivalTime + 0.5f * Mathf.Abs(Physics2D.gravity.y) * arrivalTime * arrivalTime);
        rb2d.velocity = velocity;
    }

    public override void ReleaseProjectile()
    {
        
    }
}

