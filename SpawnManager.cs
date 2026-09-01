using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [System.Serializable]
    public class SpawnArea
    {
        public Vector3 center;   // middle of the sector
        public float range = 200f; // how wide the sector is
        public float interval = 0.1f; // how often groups spawn
        [HideInInspector] public float nextSpawnTime;
    }

    public GameObject[] enemyPrefabs;
    public int numberOfSpawnAreas = 5; // number of sectors
    public float maxCenterRange = 500f; // how far from origin sectors can be
    public float defaultRange = 200f;
    public float defaultInterval = 1f;

    [Header("Group Settings")]
    public int minGroupSize = 3;
    public int maxGroupSize = 8;

    private SpawnArea[] spawnAreas;

    void Start()
    {
        // Generate random spawn areas
        spawnAreas = new SpawnArea[numberOfSpawnAreas];
        for (int i = 0; i < numberOfSpawnAreas; i++)
        {
            spawnAreas[i] = new SpawnArea();
            spawnAreas[i].center = new Vector3(
                Random.Range(-maxCenterRange, maxCenterRange),
                Random.Range(-maxCenterRange, maxCenterRange),
                Random.Range(-maxCenterRange, maxCenterRange)
            );
            spawnAreas[i].range = defaultRange;
            spawnAreas[i].interval = defaultInterval;
            spawnAreas[i].nextSpawnTime = Time.time + spawnAreas[i].interval;

            Debug.Log($"Spawn area {i} created at {spawnAreas[i].center}");
        }
    }

    void Update()
    {
        foreach (var area in spawnAreas)
        {
            if (Time.time >= area.nextSpawnTime)
            {
                SpawnGroup(area);
                area.nextSpawnTime = Time.time + area.interval;
            }
        }
    }

    void SpawnGroup(SpawnArea area)
    {
        int groupSize = Random.Range(minGroupSize, maxGroupSize + 1);

        // Reference to the marker manager
        SpawnMarkerManager markerManager = FindObjectOfType<SpawnMarkerManager>();
        Terrain activeTerrain = Terrain.activeTerrain;

        for (int i = 0; i < groupSize; i++)
        {
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);

            Vector3 spawnPos = new Vector3(
                Random.Range(area.center.x - area.range, area.center.x + area.range),
                0f, // temporary value for now, we’ll fix it below
                Random.Range(area.center.z - area.range, area.center.z + area.range)
            );

            // ✅ Adjust Y position according to terrain height (if terrain exists)
            if (activeTerrain != null)
            {
                float terrainY = activeTerrain.SampleHeight(spawnPos);
                spawnPos.y = terrainY + Random.Range(15f, 50f); // spawn 15–50 units above the surface
            }
            else
            {
                spawnPos.y = Random.Range(20f, 80f); // fallback if no terrain found
            }

            GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPos, Quaternion.identity);

            // 🟢 Create a marker that follows this enemy
            if (markerManager != null)
                markerManager.CreateMarker(enemy.transform);
        }

        Debug.Log($"Spawned group of {groupSize} enemies in sector at {area.center}");
    }

}

