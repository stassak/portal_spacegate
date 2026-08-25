using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObjects : MonoBehaviour
{
   
    

    public GameObject impactEffect;

    private Vector3 moveDirection;

    void Start()
    {
        Destroy(gameObject,300f); // auto-destroy
    }

    void Update()
    {
        // Move in the chosen direction
      
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject impactProjectile = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(impactProjectile, 2);
        Destroy(gameObject);
    }
   
}
