using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTurning : MonoBehaviour
{
    private Vector3 rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = new Vector3(
            Random.Range (-5, 0),
            Random.Range(-1, 1),
            Random.Range(-2, 2)
            );
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
