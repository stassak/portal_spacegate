using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnePlaFighter : MonoBehaviour
{
    //public float smallFighterPositionX;

    public GameObject[] enemyFightersOrbit;
    public GameObject[] enemyFightersSmall;
    public GameObject[] energyBlock;


    private int spawnXRange = 30;
    private float startDelay = 0.5f;
    private float spawnInterval = 10f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomEnemy", startDelay , spawnInterval);
        InvokeRepeating("Spawn2RandomEnemy", startDelay, spawnInterval);
        InvokeRepeating("SpawnEnergy", 100 , 100);
    }

    // Update is called once per frame
    void Update()
    {
        

       /* if(Input.GetKeyDown(KeyCode.B))
        {
            SpawnRandomEnemy();
        }*/
    }

    void SpawnRandomEnemy()
    {

        float rangeX = 150f;
        float rangeY = 50f;
        float rangeZ = 5500f;
        if (enemyFightersOrbit == null || enemyFightersOrbit.Length == 0) return;

        int enemyIndex  = Random.Range(0 , 3);
        Vector3 spawnPos = new Vector3(Random.Range(200,-200), Random.Range(-250, -400), rangeZ);
        Instantiate(enemyFightersOrbit[enemyIndex], spawnPos, enemyFightersOrbit[enemyIndex].transform.rotation);
    }

    void Spawn2RandomEnemy()
    {
        float rangeX = 150f;
        float rangeY = 800f;
        float rangeZ = 5500f;

        if (enemyFightersSmall == null || enemyFightersSmall.Length == 0) return;

        int enemyIndex = Random.Range(0, enemyFightersSmall.Length);
        Vector3 spawnPos = new Vector3(Random.Range(100, -100), Random.Range(100,-200), rangeZ);
        Instantiate(enemyFightersSmall[enemyIndex], spawnPos, enemyFightersSmall[enemyIndex].transform.rotation);
    }

    void SpawnEnergy()
    {
        int positionX = -300;
        int positionY = 0;
        int positionZ = 100;
        int indexEnergy = 0;
        Instantiate(energyBlock[indexEnergy] ,new Vector3(positionX, positionY, positionZ), energyBlock[indexEnergy].transform.rotation);
    }

    public void StopSpawning()
    {
        CancelInvoke();
    }

}
