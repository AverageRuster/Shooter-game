using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDropController : MonoBehaviour
{
    [SerializeField] private float ammoDropDeactivationTime = 0;
    private DropController dropController = null;
    private AudioSource ammoDropAudioSource = null;
    private bool canAmmoDropBeDeactivated = false;
    [SerializeField] private AudioClip[] ammoDropSounds = null;

    private void Start()
    {
        dropController = GetComponent<DropController>();
        ammoDropAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        dropController.StartDeactivasion(ammoDropDeactivationTime);

        if (!ammoDropAudioSource.isPlaying && canAmmoDropBeDeactivated)
        {
            canAmmoDropBeDeactivated = false;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.playerTotalAmmoCount += player.playerAmmoDropCount;
            dropController.deactivasionStarted = false;
            ammoDropAudioSource.PlayOneShot(ammoDropSounds[Random.Range(0, ammoDropSounds.Length)]);
            canAmmoDropBeDeactivated = true;


        }
    }
}
