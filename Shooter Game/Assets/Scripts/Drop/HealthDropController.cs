using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDropController : MonoBehaviour
{
    [SerializeField] private float healthDropDeactivationTime = 0;
    [SerializeField] private float healthDropAmount = 0;
    [SerializeField] private DropController dropController = null;

    private void Update()
    {
        dropController.StartDeactivasion(healthDropDeactivationTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.playerHealth += healthDropAmount;
            dropController.deactivasionStarted = false;
            gameObject.SetActive(false);
        }
    }
}
