using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public GameObject impactEffectLaser;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (impactEffectLaser != null)
            {
                GameObject fx = Instantiate(impactEffectLaser, other.transform.position, Quaternion.identity);
                Destroy(fx,0.5f);
            }
            Destroy(other.gameObject);
        }
    }
}
