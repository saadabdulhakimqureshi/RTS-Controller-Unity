using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    // Start is called before the first frame update
    public bool alive;
    public float explosionDamage;
    public float health;
    public float explosionForce;
    public float radius;
    public GameObject Explosion;
    public GameObject Barrel;
    public GameObject DestroyedBarrel;
    public Sphere Sphere;
    public Collider Collider;
    void Start()
    {
        Barrel.SetActive(true);
        DestroyedBarrel.SetActive(false);
        if (health == 0)
        {
            health = 1;
        }
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsAlive();   
    }

    public void LoseHealth(float power)// When a unit starts attacking the game object it loses it's health.
    {
        //Debug.Log("Here");
        health -= power * Time.deltaTime;
    }

    public void CheckIsAlive()// When a unit starts attacking the game object it loses it's health.
    {
        if (health <= 0)
        {
            
            Explode();
        }
    }

    public void Explode()// When the object loses all of it's health it is destroyed and an explosion occurs.
    {
        alive = false;
        Explosion.SetActive(true);
        foreach (GameObject rtsUnit in Sphere.objectsInSphere)
        {
            if (rtsUnit != null)
            {
                if (rtsUnit.GetComponent<RTSController>() != null)
                {
                    rtsUnit.GetComponent<RTSController>().TakeDamage(explosionDamage);
                }

                Rigidbody[] rigidBodies = rtsUnit.GetComponentsInChildren<Rigidbody>();

                foreach (Rigidbody rigidbody in rigidBodies)
                {
                    rigidbody.AddExplosionForce(explosionForce, transform.position, radius);
                }
            }
        }
        Collider.enabled = false;
        Barrel.SetActive(false);
        DestroyedBarrel.SetActive(true);
        Destroy(gameObject, 10f);
    }



}
