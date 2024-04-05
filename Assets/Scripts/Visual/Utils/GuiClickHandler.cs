using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GuiClickHandler : MonoBehaviour, IPointerClickHandler
{
    public event Action onLeft;
    public event Action onRight;
    public event Action onMiddle;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onLeft.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            onRight.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            onMiddle.Invoke();
        }
    }
}