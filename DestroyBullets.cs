using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullets : MonoBehaviour
{
    [SerializeField] private float lifeTimeBul = 5f;  

    // Start is called before the first frame update
    void Start()
    {
            Destroy(gameObject,lifeTimeBul);
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnCollisionEnter(Collision collision)
    {

        Destroy(gameObject,5f);
    }
}
