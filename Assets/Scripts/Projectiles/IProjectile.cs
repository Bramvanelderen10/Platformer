using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

abstract public class IProjectile : MonoBehaviour
{
    [HideInInspector]
    public Utils.ObjectType source;
    [HideInInspector]
    public Vector3 start, end;

    protected Utils.Direction direction;
    protected Rigidbody2D rb2d;
    public float speed = 15f;
    public float maxDistance = 200f;
    public float damage;

    void Awake()
    {
        rb2d = transform.GetComponent<Rigidbody2D>();
    }

    abstract public void ReleaseProjectile();

    protected void OnCollisionEnter2D(Collision2D coll)
    {
        bool destroy = true;
        if (coll.gameObject.tag == "Player")
        {
            PlayerController pc = coll.gameObject.GetComponent<PlayerController>();
            if (source == pc.type)
            {
                destroy = false;
            } else
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
            } else
            {
                destroy = false;
            }
        }
        if (coll.gameObject.tag == "Projectile")
        {
            destroy = false;
        }

        if (destroy)
            Destroy(transform.gameObject);
    }

    protected void Flip(Utils.Direction dir)
    {
        if (direction == Utils.Direction.LEFT)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}

