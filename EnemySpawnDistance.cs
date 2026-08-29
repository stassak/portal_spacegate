using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnDistance : MonoBehaviour
{
    public GameObject[] enemyFighters;


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
        int fighterIndex = Random.Range(0, enemyFighters.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-200, 200), Random.Range(-100, 100), 300);

        Instantiate(enemyFighters[fighterIndex], spawnPos, enemyFighters[fighterIndex].transform.rotation);
    }
}
