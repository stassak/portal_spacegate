using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
   // public float topBoundBullet = 40.0f;
    public float lowerBoundDestroy = 300.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /* if (transform.position.z > topBoundBullet)
         {
             Destroy(gameObject);
         }
         else */
        if (transform.position.z > lowerBoundDestroy)
        {
            Debug.Log(" GO Destroyed ");
            Destroy(gameObject);// prevent to destroy game Object
        }
    }

    //countin amount of the monster 
  /*  private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CaptureZone"))
        {
            PlayerManager.instance.MonsterCapture();
            Destroy(gameObject);
        }



    }*/
}
