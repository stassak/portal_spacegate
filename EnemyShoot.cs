using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject projectileGreenPref;
    public Transform firePoint;
    public float fireRate = 2f;

    private Transform player;
    private float nextFireTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        if (Time.time >= nextFireTime)
        {
            ShootAtPlayer();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void ShootAtPlayer()
    {
        if (projectileGreenPref == null || firePoint == null || player == null)
            return;

        Vector3 direction = (player.position - firePoint.position).normalized;

       GameObject proj =  Instantiate(
            projectileGreenPref,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * 100f;
        }
    }
}
