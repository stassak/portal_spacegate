using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public enum ShotType { Projectile, Laser }
    public ShotType shotType = ShotType.Projectile;

    public enum OwnerType { Player, Enemy }
    public OwnerType owner;

    [Header("Projectile Settings")]
    [SerializeField] private float speed = 40f;
    [SerializeField] private float lifeTime = 5f;

    [Header("Laser Settings")]
    public float laserDuration = 0.1f;

    [Header("Explosion")]
    public GameObject enemyExplosionPrefab;

    public GameObject impactEffect;

    private Vector3 moveDirection;
    private float laserTimer;

    void Start()
    {
        if (shotType == ShotType.Projectile)
            Destroy(gameObject, lifeTime);
        else
            laserTimer = laserDuration;
    }

    void Update()
    {
        if (shotType == ShotType.Projectile)
        {
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        }
        else
        {
            laserTimer -= Time.deltaTime;
            if (laserTimer <= 0f)
                Destroy(gameObject);
        }
    }

    public void Launch(Vector3 direction)
    {
        moveDirection = direction.normalized;
        transform.forward = moveDirection;
    }

    //  PROJECTILE HIT
    private void OnCollisionEnter(Collision collision)
    {
        if (shotType == ShotType.Laser)
            return;

        HandleHit(collision.gameObject, collision.contacts[0].point);
    }

    //  LASER HIT
    /* private void OnTriggerEnter(Collider other)
     {
         if (shotType != ShotType.Laser)
             return;

         HandleHit(other.gameObject, other.transform.position);
     }*/
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BossEnemy"))
        {
            BossHealthEnemy boss = other.GetComponent<BossHealthEnemy>();

            if (boss != null)
            {
                boss.TakeDamage(1);
            }

            Destroy(gameObject);
        }
    }*/

    

    void SpawnEnemyExplosion(Vector3 pos)
    {
        if (enemyExplosionPrefab == null)
            return;

        GameObject explosion = Instantiate(enemyExplosionPrefab, pos, Quaternion.identity);
        Destroy(explosion, 1f);
    }
    //  SINGLE HIT HANDLER
    /*void HandleHit(GameObject target, Vector3 hitPoint)
    {
        // PLAYER BULLET → ANYTHING
        if (owner == OwnerType.Player)
        {
            SpawnEnemyExplosion(hitPoint);

            // If it's an enemy, destroy it
            if (target.CompareTag("Enemy"))
            {
                Destroy(target);
            }
        }

        // ENEMY BULLET → PLAYER
        else if (owner == OwnerType.Enemy && target.CompareTag("Player"))
        {
            if (PLayerManager.Instance != null)
                PLayerManager.Instance.OnPlayerHit(1);
        }

        Destroy(gameObject);
    }
    */

    void HandleHit(GameObject target, Vector3 hitPoint)
    {
        // PLAYER BULLET
        if (owner == OwnerType.Player)
        {
            SpawnEnemyExplosion(hitPoint);

            // Normal enemies
            if (target.CompareTag("Enemy"))
            {
                Destroy(target);
            }

            
            // check the boss damage
            if (target.CompareTag("BossEnemy"))
            {
                Debug.Log("BOSS HIT*******************************************************");

                BossHealthEnemy boss = target.GetComponent<BossHealthEnemy>();

                if (boss != null)
                {
                    boss.TakeDamage(1);
                }
            }
        }

        // ENEMY BULLET → PLAYER
        else if (owner == OwnerType.Enemy && target.CompareTag("Player"))
        {
            if (PLayerManager.Instance != null)
                PLayerManager.Instance.OnPlayerHit(1);
        }

        Destroy(gameObject);
    }

    void SpawnImpact(Vector3 pos)
    {
        if (impactEffect == null) return;

        GameObject impact = Instantiate(impactEffect, pos, Quaternion.identity);
        Destroy(impact, 1.5f);
    }
}