using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBodyController : MonoBehaviour
{
    [SerializeField] private GameObject soul = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.playerSouls++;// soul.GetComponent<SoulController>().soulCount;
            SoulPickupIndicatorController.ResetColor();
            soul.GetComponent<SoulController>().StopAllActiveCoroutines();
            soul.GetComponent<SoulController>().soulAnimator.Play("SoulDestruction");
        }
    }
}
