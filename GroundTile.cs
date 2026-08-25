using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    ZoneManager nextZone;


    // Start is called before the first frame update
    void Start()
    {
        nextZone = GameObject.FindObjectOfType<ZoneManager>();
    }

    private void OnTriggerExit(Collider other)
    {
       
        Destroy(gameObject, 10);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
