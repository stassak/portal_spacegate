using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner2D : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Enemy List")]
    public GameObject[] enemies;    // <-- assign all enemy prefabs here

    [Header("Spawn Settings")]
    public float spawnDistance = 2500f;
    public int spawnAmount = 5;
    public float spawnInterval = 5f;

    [Header("Spawn Mode")]
    public bool use3DSphere = true;   // true = 3D sphere, false = X/Z plane ring

    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemiesAroundPlayer();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemiesAroundPlayer()
    {
        if (player == null || enemies.Length == 0)
        {
            Debug.LogWarning("Spawner error: Missing player or enemy list!");
            return;
        }

        for (int i = 0; i < spawnAmount; i++)
        {
            // --- Pick random enemy ---
            GameObject enemyPrefab = enemies[Random.Range(0, enemies.Length)];
            if (enemyPrefab == null) continue;

            Vector3 randomDir;

            // --- Choose spawn mode ---
            if (use3DSphere)
            {
                randomDir = Random.onUnitSphere; // full 360 degrees in 3D
            }
            else
            {
                Vector2 dir2D = Random.insideUnitCircle.normalized;
                randomDir = new Vector3(dir2D.x, 0, dir2D.y); // X/Z plane only
            }

            Vector3 spawnPos = player.position + randomDir * spawnDistance;

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }

        Debug.Log($"Spawned {spawnAmount} enemies around the player.");
    }
}

