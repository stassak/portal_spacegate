using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class ZoneManager : MonoBehaviour
    {
    [Header("Setup")]
    public GameObject[] zonePrefabs;   // assign different zone prefabs (terrain, space sector, etc.)
    public Transform player;           // assign your player or ship
    public int viewDistance = 1;       // how many zones around the player (1 = 3x3 grid)
    public float zoneSize = 500f;      // size of one zone (width and depth)

    // track active zones
    private Dictionary<Vector2Int, GameObject> activeZones = new Dictionary<Vector2Int, GameObject>();
    private Vector2Int currentZone;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        UpdateZones(true);
    }

    void Update()
    {
        Vector2Int playerZone = GetZoneCoordinate(player.position);

        // check if player moved to a new zone
        if (playerZone != currentZone)
        {
            currentZone = playerZone;
            UpdateZones();
        }
    }

    Vector2Int GetZoneCoordinate(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / zoneSize);
        int z = Mathf.FloorToInt(pos.z / zoneSize);
        return new Vector2Int(x, z);
    }

    void UpdateZones(bool firstTime = false)
    {
        HashSet<Vector2Int> newZoneCoords = new HashSet<Vector2Int>();

        // find which zones should exist around the player
        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int z = -viewDistance; z <= viewDistance; z++)
            {
                Vector2Int coord = new Vector2Int(currentZone.x + x, currentZone.y + z);
                newZoneCoords.Add(coord);

                if (!activeZones.ContainsKey(coord))
                {
                    SpawnZone(coord);
                }
            }
        }

        // remove zones too far away
        List<Vector2Int> toRemove = new List<Vector2Int>();
        foreach (var kvp in activeZones)
        {
            if (!newZoneCoords.Contains(kvp.Key))
                toRemove.Add(kvp.Key);
        }

        foreach (var coord in toRemove)
        {
            Destroy(activeZones[coord]);
            activeZones.Remove(coord);
        }

        if (firstTime)
            currentZone = GetZoneCoordinate(player.position);
    }

    void SpawnZone(Vector2Int coord)
    {
        Vector3 pos = new Vector3(coord.x * zoneSize, 0f, coord.y * zoneSize);
        GameObject prefab = zonePrefabs[Random.Range(0, zonePrefabs.Length)];

        GameObject zone = Instantiate(prefab, pos, Quaternion.identity);
        zone.name = $"Zone_{coord.x}_{coord.y}";
        activeZones.Add(coord, zone);
    }
}

