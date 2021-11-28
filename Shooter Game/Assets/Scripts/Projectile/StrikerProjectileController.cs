using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikerProjectileController : MonoBehaviour
{
    public float projectileSpeed = 0;
    public float projectileDamage = 0;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x) > 50 || Mathf.Abs(transform.position.y) > 50)
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.TransformDirection(Vector3.up * projectileSpeed * Time.deltaTime));
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
