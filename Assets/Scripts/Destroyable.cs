using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public bool isAlive;
    public GameObject Explosion;
    void Start()
    {
        if (!isAlive)// Checking if our object has been destroyed.
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoseHealth(float power)// When a unit starts attacking the game object it loses it's health.
    {
        //Debug.Log("Here");
        if (health > 0)
        {
            health -= power * Time.deltaTime;
        }

        else 
        {
            Explode();
        }
    }

    public void CheckIsAlive(Action<bool> destroyed)// When a unit starts attacking the game object it loses it's health.
    {
        if (health <= 0)
        {
            Explode();
            destroyed.Invoke(true);
            
        }
        else
        {
            destroyed.Invoke(false);
        }
    }

    public void Explode()// When the object loses all of it's health it is destroyed and an explosion occurs.
    {
        Explosion.SetActive(true);
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
