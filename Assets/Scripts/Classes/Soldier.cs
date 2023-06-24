using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    // Start is called before the first frame update

    public Soldier(): base()
    {
        
    }

    public Soldier(float h, float aD):base(h, aD)
    {
        Debug.Log("Inside Constructor of child.");
    }
    
    public void Attack(GameObject target)
    {
        if (target.GetComponent<EnemyController>() != null)
            target.GetComponent<EnemyController>().TakeDamage(attackDamage);
        if (target.GetComponent<RTSController>() != null)
        {
            target.GetComponent<RTSController>().TakeDamage(attackDamage);
        }
            
    }

    public void TakeDamage(float amount)
    {
        health-=amount;
    }

    public bool CheckHealth()
    {
        if (health <= 0) return true;

        return false;
    }

    public void RefillHealth()
    {
        health = maximumHealth;
    }

    public void SoldierPower(float amount)
    {

    }
}
