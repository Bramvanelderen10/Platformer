using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

abstract public class IEnemy : MonoBehaviour
{
    public float hitPoints;
    public Utils.ObjectType type = Utils.ObjectType.ENEMY;

    abstract public void AddDamage(float damage);

    abstract protected void Kill();

    abstract public string GetAttackAnimation(Transform target);

    abstract public void Attack(Transform target, Transform start);
}

