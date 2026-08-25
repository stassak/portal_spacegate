using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsterSpawnManager : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;

   
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomAsteriod", 1f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
      //  HandleSpawn();
    }

  /*  private void HandleSpawn()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnRandomAsteriod();
        }
    }*/

    void SpawnRandomAsteriod()
    {
        int asteriodIndex = Random.Range(0, asteroidPrefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-700, -500), 0, 600);

        Instantiate(asteroidPrefabs[asteriodIndex], spawnPos, asteroidPrefabs[asteriodIndex].transform.rotation);
    }
}
