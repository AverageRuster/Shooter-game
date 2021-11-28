using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMagnetZoneController : MonoBehaviour
{
    [SerializeField] private SoulController soulController = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            soulController.canSoulMoveToPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            soulController.canSoulMoveToPlayer = false;
        }
    }
}
