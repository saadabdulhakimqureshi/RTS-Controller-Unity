using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    // Start is called before the first frame update
    protected float health;
    protected float maximumHealth;
    protected float attackDamage;

    public Unit()
    {

    }

    public Unit(float h, float aD)
    {
        this.health = h;
        this.maximumHealth = h;
        this.attackDamage = aD;
    }
    
    public virtual void Attack(GameObject target)
    {
        
    }

    public virtual void TakeDamage(float amount)
    {

    }

    public virtual void RefillHealth()
    {
        
    }

    public virtual bool CheckHealth()
    {
        return true;
    }
}
