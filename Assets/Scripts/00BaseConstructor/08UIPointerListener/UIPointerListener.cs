using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPointerListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject listenerObject;
    public object[] commands = null;

    public Vector2 clickDownPosition;
    public Vector2 clickUpPosition;

    public Action<GameObject, object[]> onPointerClickCallBack0;
    public Action<GameObject, object[]> onPointerDownCallBack0;
    public Action<GameObject, object[]> onPointerUpCallBack0;
    public Action<GameObject, object[]> onPointerDragCallBack0;

    public Action<PointerEventData, GameObject, object[]> onPointerClickCallBack1;
    public Action<PointerEventData, GameObject, object[]> onPointerDownCallBack1;
    public Action<PointerEventData, GameObject, object[]> onPointerUpCallBack1;
    public Action<PointerEventData, GameObject, object[]> onPointerDragCallBack1;

    public void OnPointerClick(PointerEventData eventData)
    {
        onPointerClickCallBack0?.Invoke(listenerObject, commands);
        onPointerClickCallBack1?.Invoke(eventData, listenerObject, commands);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDownCallBack0?.Invoke(listenerObject, commands);
        onPointerDownCallBack1?.Invoke(eventData, listenerObject, commands);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUpCallBack0?.Invoke(listenerObject, commands);
        onPointerUpCallBack1?.Invoke(eventData, listenerObject, commands);
    }

    public void OnDrag(PointerEventData eventData)
    {
        onPointerDragCallBack0?.Invoke(listenerObject, commands);
        onPointerDragCallBack1?.Invoke(eventData, listenerObject, commands);
    }
}
