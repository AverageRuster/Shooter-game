using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmortalityDropController : MonoBehaviour
{
    [SerializeField] private float immortalityDropDeactivationTime = 0;
    [SerializeField] private DropController dropController = null;

    private void Update()
    {
        dropController.StartDeactivasion(immortalityDropDeactivationTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.ActivateImmortality();
            dropController.deactivasionStarted = false;
            gameObject.SetActive(false);
        }
    }
}
