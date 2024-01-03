using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPointerListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject listenerObject { get; set; }
    public object[] commands { get; set; } = null;

    public Vector2 clickDownPosition { get; set; }
    public Vector2 clickUpPosition { get; set; }

    public Action<GameObject, object[]> onPointerClickCallBack0 { get; set; }
    public Action<GameObject, object[]> onPointerDownCallBack0 { get; set; }
    public Action<GameObject, object[]> onPointerUpCallBack0 { get; set; }
    public Action<GameObject, object[]> onPointerDragCallBack0 { get; set; }

    public Action<PointerEventData, GameObject, object[]> onPointerClickCallBack1 { get; set; }
    public Action<PointerEventData, GameObject, object[]> onPointerDownCallBack1 { get; set; }
    public Action<PointerEventData, GameObject, object[]> onPointerUpCallBack1 { get; set; }
    public Action<PointerEventData, GameObject, object[]> onPointerDragCallBack1 { get; set; }

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
