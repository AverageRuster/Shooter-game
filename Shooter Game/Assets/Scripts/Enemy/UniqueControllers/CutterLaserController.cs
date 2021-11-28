using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterLaserController : MonoBehaviour
{
    [SerializeField] CutterController cutterController;
    public GameObject target;
    public bool isTargetTouchesLaser;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            target = collision.gameObject;
            isTargetTouchesLaser = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = null;
            isTargetTouchesLaser = false;
        }
    }
}
