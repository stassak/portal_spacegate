using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDestroy : MonoBehaviour
{
    public GameObject goDestroy;
    public float timeDestroy = 60f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(goDestroy,timeDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
