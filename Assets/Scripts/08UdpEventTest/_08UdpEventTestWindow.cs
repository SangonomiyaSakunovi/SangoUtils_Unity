using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _08UdpEventTestWindow : MonoBehaviour
{
    private void Start()
    {
        UdpEventService.Instance.OnInit();
        InitEvent();
    }

    private void InitEvent()
    {
        _08UdpEvent @event = UdpEventService.Instance.GetUdpEvent<_08UdpEvent>(UdpEventListenPortId.typeInPort);
    }
}
