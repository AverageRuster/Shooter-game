using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivePlayerProjectileController : MonoBehaviour
{
    [SerializeField] private GameObject projectileBody;
    [SerializeField] private AttackZoneController attackZoneController;
    [SerializeField] private PlayerProjectileController playerProjectileController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<EnemyController>().isEnemyActive)
        {
            float projectileDamage = playerProjectileController.projectileDamage;
            collision.gameObject.GetComponent<EnemyController>().enemyHealth -= projectileDamage;

            Explode(projectileDamage / 8);

            projectileBody.SetActive(false);
        }
    }

    public void Explode(float explosiveDamage)
    {
        List<GameObject> playerTargets = attackZoneController.playerTargets;
        List<GameObject> enemyTargets = attackZoneController.enemyTargets;
        for (int i = 0; i < playerTargets.Count; i++)
        {
            playerTargets[i].GetComponent<PlayerController>().playerHealth -= explosiveDamage;
        }
        for (int i = 0; i < enemyTargets.Count; i++)
        {

            enemyTargets[i].GetComponent<EnemyController>().enemyHealth -= explosiveDamage;
            Debug.Log("S");

        }
        attackZoneController.ResetLists();
    }
}
