using UnityEngine;
using System.Collections;
using System;

public abstract class IAbility<T>{

    protected PlayerController.Abilities abilityType { get; set; }

    protected bool unlocked= false;

    protected float cooldown = 0;
    protected Int32 coolDownTime = 0;

    abstract public bool IsUnlocked();

    abstract public void Unlock();

    abstract public IAbilityObject Execute(T t);

    abstract public PlayerController.Abilities GetAbilityType();
}
