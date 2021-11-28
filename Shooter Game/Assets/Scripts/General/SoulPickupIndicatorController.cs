using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulPickupIndicatorController : MonoBehaviour
{
    public static Image soulPickupIndicatorImage = null;
    private bool canColorBeChanged = true;
    private float colorChangeCooldownTime = 0.1f;
    private bool allCoroutinesStopped = false;

    private void Start()
    {
        soulPickupIndicatorImage = GetComponent<Image>();
        allCoroutinesStopped = false;
    }

    private void Update()
    {
        if (GameManager.gameOver)
        {
            if (!allCoroutinesStopped)
            {
                StopAllCoroutines();
                allCoroutinesStopped = true;
            }
        }

        if (soulPickupIndicatorImage.color.a > 0)
        {
            Color newColor = soulPickupIndicatorImage.color;
            newColor.a -= 0.01f;
            soulPickupIndicatorImage.color = newColor;
            StartCoroutine(ColorChangeCooldown());
        }
    }

    public static void ResetColor()
    {
        Color newColor = soulPickupIndicatorImage.color;
        newColor.a = 0.1f;
        soulPickupIndicatorImage.color = newColor;
    }

    private IEnumerator ColorChangeCooldown()
    {
        canColorBeChanged = false;
        yield return new WaitForSeconds(colorChangeCooldownTime);
        canColorBeChanged = true;
    }
}
