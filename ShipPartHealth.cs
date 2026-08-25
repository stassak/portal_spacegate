using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartHealth : MonoBehaviour
{
    public int hitsToDestroy = 30;
    private int hitCount = 0;

    public GameObject destroyEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            hitCount++;
            Debug.Log("Hits : " + hitCount);

            Destroy(collision.gameObject);

            if (hitCount >= hitsToDestroy)
            {
                // Spawn explosion effect
                Instantiate(destroyEffect, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }
}
