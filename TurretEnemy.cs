using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    [Header("Targeting")]
    public float detectionRange = 500f;
    public float rotationSpeed = 2f;
    private Rigidbody enemyRBTurret;
    public Transform player;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    public float projectileSpeed = 1000f;

    private float nextFireTime = 0f;

    //[SerializeField] private float lifeTime = 5f;
    void Start()
    {
        enemyRBTurret = GetComponent<Rigidbody>();
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                transform.LookAt(player.position);
            }
        }

        Destroy(gameObject);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            // Rotate smoothly toward player
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);

            // Fire if cooldown elapsed
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null || player == null)
            return;

        transform.LookAt(player.position);

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        if (rb != null)
            rb.velocity = firePoint.forward * projectileSpeed;

        // Auto destroy to save memory
        Destroy(proj, 5f);
    }

   /* private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }*/
}
