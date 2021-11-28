using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileController : MonoBehaviour
{
    [HideInInspector] public float projectileSpeed = 0;
    [HideInInspector] public float projectileDamage = 0;
    public int projectileType = 0;

    public bool canProjectileDamage = true;

    private Rigidbody2D rb;

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
        rb.MovePosition(transform.position + transform.TransformDirection(Vector3.up * projectileSpeed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (projectileType == 0 && collision.CompareTag("Enemy") && collision.gameObject.GetComponent<EnemyController>().isEnemyActive)
        {
            if (canProjectileDamage)
            {
                collision.gameObject.GetComponent<EnemyController>().enemyHealth -= projectileDamage;
                canProjectileDamage = false;
            }
            gameObject.SetActive(false);
        }
        if (projectileType == 0 && collision.CompareTag("Projectile"))
        {
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
