using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZoneController : MonoBehaviour
{
    public List<GameObject> playerTargets = new List<GameObject>();
    public List<GameObject> enemyTargets = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTargets.Add(collision.gameObject);
        }
        if (collision.CompareTag("Enemy") && collision.gameObject.GetComponent<EnemyController>().isEnemyActive)
        {
            enemyTargets.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTargets.Remove(collision.gameObject);
        }
        if (collision.CompareTag("Enemy") && collision.gameObject.GetComponent<EnemyController>().isEnemyActive)
        {
            enemyTargets.Remove(collision.gameObject);
        }
    }

    public void ResetLists()
    {
        playerTargets = new List<GameObject>();
        enemyTargets = new List<GameObject>();
    }
}
