using UnityEngine;
using System.Collections;
using System;
using System.Timers;

public class Dash : IAbility<DashObject> {

    public bool isDashing = false;
    public float dashDistance = 5f;

    private float interval;
    private Vector3 finalPosition;
    private Vector3 currentPosition;
    private System.Timers.Timer timer;
    private int counter;

    public Dash()
    {
        abilityType = PlayerController.Abilities.DASH;
        cooldown = -1;
    }

    public override bool IsUnlocked()
    {

        return unlocked;
    }

    public override void Unlock()
    {
        unlocked = true;
    }

    public override IAbilityObject Execute(DashObject t)
    {
        CollisionChecker cc = t.transform.gameObject.GetComponent<CollisionChecker>() as CollisionChecker;

        if (!isDashing && unlocked && (Utils.GetUnixTime() - coolDownTime) > cooldown)
        {
            

            float dash = cc.GetDistanceUntilCollision(t.direction, dashDistance);
            //float dash = (t.direction == Direction.RIGHT) ? temp_dist : temp_dist * -1;

            currentPosition = t.transform.localPosition;
            Vector3 position = t.transform.localPosition;
            position.x += dash;
            finalPosition = position;

            counter = 5;
            interval = dash / counter;

            isDashing = true;
            
            timer = new System.Timers.Timer();
            timer.Interval = 10;
            timer.Elapsed += UpdateDash;
            timer.Enabled = true;            
        }
        
        return t;
    }

    public Vector3 GetCurrentPosition()
    {

        return currentPosition;
    }

    private void UpdateDash(object sender, ElapsedEventArgs e)
    {
        Vector3 position = currentPosition;
        position.x += interval;
        currentPosition = position;
        counter--;
        if (counter == 0)
        {
            timer.Enabled = false;
            isDashing = false;
            counter = 5;
            coolDownTime = Utils.GetUnixTime();
        }
    }

    public override PlayerController.Abilities GetAbilityType()
    {

        return abilityType;
    }
}
