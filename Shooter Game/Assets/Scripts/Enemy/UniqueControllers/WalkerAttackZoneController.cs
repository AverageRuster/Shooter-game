using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerAttackZoneController : MonoBehaviour
{
    public bool canWalkerStartAttack = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canWalkerStartAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canWalkerStartAttack = false;
        }
    }
}
