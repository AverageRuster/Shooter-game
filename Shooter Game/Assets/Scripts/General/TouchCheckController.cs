using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCheckController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerController.ignoreTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerController.ignoreTouch = false;
    }
}
