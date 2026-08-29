using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthEnemy : MonoBehaviour
{
    [SerializeField] private int currentHits = 0;
    [SerializeField] private int hitsToDestroy = 10;

    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject nextLevelUI;

    private bool isDead = false;

    void Start()
    {
        if (nextLevelUI != null)
            nextLevelUI.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHits += damage;

        Debug.Log("-----------------------Boss Damage Taken----------------: " + currentHits);

        if (currentHits >= hitsToDestroy)
        {
            DestroyBoss();
        }
    }

    void DestroyBoss()
    {
        if (isDead) return;
        isDead = true;

        GameState.IsGameOver = true;

        // spawn explosion FIRST
        if (explosionEffect != null)
        {
            // Instantiate(explosionEffect, transform.position, Quaternion.identity);//normal position of the explossion
            Vector3 explosionPos =Camera.main.transform.position + Camera.main.transform.forward * 12f;

            GameObject exp = Instantiate(explosionEffect,explosionPos,Quaternion.identity);
        
                  exp.transform.localScale = Vector3.one * 50f;

                Destroy(exp, 1f);
    }


    // show UI AFTER short delay
    StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        // wait so player sees explosion
        //  yield return new WaitForSeconds(1.5f);

        Time.timeScale = 0.10f;
        yield return new WaitForSecondsRealtime(1.0f);

        if (nextLevelUI != null)
            nextLevelUI.SetActive(true);

        // NOW freeze game
        Time.timeScale = 0f;
        Destroy(gameObject);

    }
}
