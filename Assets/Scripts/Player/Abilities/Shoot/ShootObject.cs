using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class ShootObject : IAbilityObject
{
    public GameObject projectilePrefab;
    public Transform start, end;
    public Utils.ObjectType objectType;

    public ShootObject(GameObject projectilePrefab, Transform start, Transform end, Utils.ObjectType objectType)
    {
        this.projectilePrefab = projectilePrefab;
        this.start = start;
        this.end = end;
        this.objectType = objectType;
    }
}

