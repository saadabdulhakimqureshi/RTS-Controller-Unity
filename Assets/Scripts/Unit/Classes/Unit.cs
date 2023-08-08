using CodeMonkey.HealthSystemCM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit: MonoBehaviour
{
    // Start is called before the first frame update
    public HealthSystemComponent healthSystemComponent;
    public float attackDamage;
    public Transform BulletSpawn;
    public Transform Bullet;

    /*    public Unit()
        {

        }

        public Unit(float health, float attackDamage, Character character)
        {
            this.health = health;
            this.maximumHealth = health;
            this.attackDamage = attackDamage;
            this.character = character;
        }*/

    private void Start()
    {
        healthSystemComponent = GetComponent<HealthSystemComponent>();
    }
    public virtual void Attack(Transform target, string type)
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

    public virtual void ThrowGrenade()
    {

    }
}
