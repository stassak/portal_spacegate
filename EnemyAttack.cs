using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 20f;
    public float stopDistance = 150f;    // <-- NEW
    private Rigidbody enemyRB;
    private Transform player;

    [Header("Shooting")]
    public GameObject projectileGreenPref;
    public Transform firePoint;
    public float fireRate = 2f;
    public float shootingDistance = 400f;

    private float nextFireTime = 0f;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (enemyRB != null)
            enemyRB.useGravity = false;

        if (player != null)
            transform.LookAt(player.position);
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);

        // --- ROTATE TOWARD PLAYER ---
        transform.rotation = Quaternion.LookRotation(dir);

        // --- MOVE ONLY IF FARTHER THAN STOP DISTANCE ---
        if (distance > stopDistance)
        {
            enemyRB.velocity = dir * speed;
        }
        else
        {
            enemyRB.velocity = Vector3.zero; // stop moving
        }

        // --- SHOOT IF WITHIN SHOOTING RANGE ---
        if (distance <= shootingDistance && Time.time >= nextFireTime)
        {
            ShootAtPlayer();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Optional safe collision with player
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDir = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDir * 50f, ForceMode.Impulse);
            }

            Debug.Log("Enemy touched player — no destruction.");
        }
    }

    private void ShootAtPlayer()
    {
        if (projectileGreenPref == null || firePoint == null || player == null)
            return;

        transform.LookAt(player.position);

        GameObject proj = Instantiate(projectileGreenPref, firePoint.position, firePoint.rotation);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * 100f;
        }
    }
}
