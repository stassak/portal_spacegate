using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackGround : MonoBehaviour
{
   [SerializeField] private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < startPos.z - 10000)
        {
            transform.position = startPos;
        }
    }
}
