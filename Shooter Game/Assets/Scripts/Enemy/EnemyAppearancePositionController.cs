using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAppearancePositionController : MonoBehaviour
{
    private bool canyAppearancePositionRotate = true;
    private float appearancePositionRotationCooldownTime = 0.025f;

    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, 1f);
        /*
        if (canyAppearancePositionRotate)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 5f);
            StartCoroutine(AppearancePositionRotationCooldown());
        }
        */
    }

    /*
    private IEnumerator AppearancePositionRotationCooldown()
    {
        canyAppearancePositionRotate = false;
        yield return new WaitForSeconds(appearancePositionRotationCooldownTime);
        canyAppearancePositionRotate = true;
    }
    */
}
