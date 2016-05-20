using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public enum Abilities
    {
        DASH,
        Shoot,
    }

    
    public Transform shootStart, shootEnd;
    public GameObject projectilePrefab;
    public float hitPoints = 100f;

    [HideInInspector]
    public float maxHitPoints;
    [HideInInspector]
    public Utils.ObjectType type = Utils.ObjectType.PLAYER;

    private Dash dash;
    private Shoot shoot;    

	// Use this for initialization
	void Awake () {       
        dash = new Dash();
        dash.Unlock();

        shoot = new Shoot();
        shoot.Unlock();
        maxHitPoints = hitPoints;
    }
	
	// Update is called once per frame
	void Update () {
        HandleInput();
        if (dash.isDashing)
        {
            transform.localPosition = dash.GetCurrentPosition();
        }
    }

    void HandleInput()
    {
        if (Input.GetButtonDown("right_dash"))
        {
            dash.Execute(new DashObject(transform, Utils.Direction.RIGHT));
        }
        if (Input.GetButtonDown("left_dash"))
        {
            dash.Execute(new DashObject(transform, Utils.Direction.LEFT));
        }

        if (Input.GetButtonDown("shoot"))
        {
            shoot.Execute(new ShootObject(projectilePrefab, shootStart, shootEnd, type));
        }
    }

    public void AddDamage(float damage)
    {
        hitPoints -= damage;
    }    

}
