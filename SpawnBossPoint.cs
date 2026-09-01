using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBossPoint : MonoBehaviour
{
    public GameObject[] enemyFighters;
   // [SerializeField] private float spawnYpoint = 800.0f;
  //  [SerializeField] private float spawnZpoint = 500.0f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomAsteriod", 2f, 2f);
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
        Vector3 spawnPos = new Vector3(Random.Range(-5000,5000), Random.Range( 11000, 12500) , Random.Range(1800, 3000));//  y spawn point : Random.Range(-100, 0) for z: 1200

        Instantiate(enemyFighters[fighterIndex], spawnPos, enemyFighters[fighterIndex].transform.rotation);
    }
}
