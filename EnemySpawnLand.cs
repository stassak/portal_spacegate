using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnLand : MonoBehaviour
{
    public GameObject[] enemyLand;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("LandEnemySpawn", 0.5f, 20f);
    }

    // Update is called once per frame
    void Update()
    {
      //  LandEnemySpawn(); // every frame calling this method in update
    }

    private void LandEnemySpawn()
    {
        int landIndex = Random.Range(0, enemyLand.Length);
        Vector3 spawnPos = new Vector3(-300, 1200, -900);

        Instantiate(enemyLand[landIndex], spawnPos, enemyLand[landIndex].transform.rotation);
    }
}
