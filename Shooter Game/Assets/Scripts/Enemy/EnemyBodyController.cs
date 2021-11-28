using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyController : MonoBehaviour
{
    private SpriteRenderer enemySpriteRenderer = null;
    private Color currentEnemyColor;
    private bool canColorBeChanged = true;
    private const float colorChangeCooldownTime = 0.1f;

    private void Start()
    {
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if ((enemySpriteRenderer.color.r > 0.7f || enemySpriteRenderer.color.g > 0.7f || enemySpriteRenderer.color.b > 0.7f) && canColorBeChanged)
        {
            currentEnemyColor = enemySpriteRenderer.color;
            currentEnemyColor.r -= 0.1f;
            currentEnemyColor.g -= 0.1f;
            currentEnemyColor.b -= 0.1f;
            enemySpriteRenderer.color = currentEnemyColor;
            StartCoroutine(ColorChangeCooldown());
        }
    }

    private IEnumerator ColorChangeCooldown()
    {
        canColorBeChanged = false;
        yield return new WaitForSeconds(colorChangeCooldownTime);
        canColorBeChanged = true;
    }
}
