using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Shoot : IAbility<ShootObject>
{
    public Shoot()
    {
        abilityType = PlayerController.Abilities.Shoot;
        cooldown = -1;
    }

    public override IAbilityObject Execute(ShootObject t)
    {
        if (unlocked && (Utils.GetUnixTime() - coolDownTime) > cooldown)
        {
            GameObject go = UnityEngine.Object.Instantiate(t.projectilePrefab);
            FireBallController fb = go.GetComponent<FireBallController>();
            fb.start = t.start.position;
            fb.end = t.end.position;
            fb.source = t.objectType;
            fb.ReleaseProjectile();
            coolDownTime = Utils.GetUnixTime();
        }

        return t;
    }

    public override PlayerController.Abilities GetAbilityType()
    {

        return abilityType;
    }

    public override bool IsUnlocked()
    {

        return unlocked;
    }

    public override void Unlock()
    {
        unlocked = true;
    }
}

