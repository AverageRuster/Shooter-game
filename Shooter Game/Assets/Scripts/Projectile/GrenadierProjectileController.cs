using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierProjectileController : MonoBehaviour
{
    Rigidbody2D rb;

    private const float maxProjectileSpeed = 5;
    public float projectileDamage = 0;

    private AudioSource grenadeAudioSource;
    private const float grenadeSpeed = 1000;
    public Vector3 grenadeMovementVector = Vector3.zero;

    public float explosionCooldownTime = 1.5f;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetupDefaults();
    }

    private void SetupDefaults()
    {
        grenadeAudioSource = GetComponent<AudioSource>();
    }




    private void Update()
    {
        grenadeMovementVector = (PlayerController.playerPosition - transform.position).normalized;

    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxProjectileSpeed)
        {
            rb.AddForce(grenadeMovementVector * grenadeSpeed * Time.fixedDeltaTime);
        }
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (!player.isPlayerImmortal)
            {
                player.playerHealth -= projectileDamage;
            }
            gameObject.SetActive(false);
        }
    }
}
