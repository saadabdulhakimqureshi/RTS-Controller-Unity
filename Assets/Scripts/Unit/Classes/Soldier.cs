using CodeMonkey.HealthSystemCM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    public void Start()
    {
    }


    public override void Attack(Transform target, string tag)
    {

        Transform bullet = Instantiate(Bullet, BulletSpawn.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        
        RTSController rTSController = target.GetComponent< RTSController>();
        Transform targetTransform;
        if (rTSController != null)
            targetTransform = rTSController.TargetTransform;
        else
            targetTransform = target.transform;

        bulletScript.Setup(targetTransform.position, (targetTransform.position - BulletSpawn.position).normalized, tag, attackDamage);
    }

    public override void TakeDamage(float damage)
    {
        healthSystemComponent.healthSystem.Damage(damage);
        //health -= damage*Time.deltaTime;
    }

    public override bool CheckHealth()
    {
        if (healthSystemComponent != null)
        {
            if (healthSystemComponent.healthSystem.IsDead()) return true;

        }
        return false;
    }

    public override void RefillHealth()
    {
        //health = maximumHealth;
        healthSystemComponent.healthSystem.Heal(healthSystemComponent.healthAmountMax);
    }

    public override void ThrowGrenade()
    {

    }
}
