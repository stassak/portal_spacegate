using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorSpawn : MonoBehaviour
{
    [SerializeField]
    public int spawnSectorCordinates;


    private float spawnInterval = 5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRandomSectors()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(1,10),
            Random.Range(1, 10),
            Random.Range(1, 10)
            );
    }
}
