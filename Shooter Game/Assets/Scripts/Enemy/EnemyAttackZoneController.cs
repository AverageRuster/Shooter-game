using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackZoneController : MonoBehaviour
{
    private EnemyController EnemyController;

    private void Start()
    {
        EnemyController = transform.parent.GetComponent<EnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EnemyController.target = collision.gameObject;
            EnemyController.canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EnemyController.target = null;
            EnemyController.canAttack = false;
        }
    }
}
