using UnityEngine;
using System.Collections;

public class DashObject : IAbilityObject {
    public Transform transform { get; set; }
    public Utils.Direction direction;

    public DashObject(Transform transform, Utils.Direction direction)
    {
        this.transform = transform;
        this.direction = direction;
    }
}
