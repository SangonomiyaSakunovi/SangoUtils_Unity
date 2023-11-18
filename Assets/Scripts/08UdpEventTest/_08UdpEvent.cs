using UnityEngine;

public class _08UdpEvent : BaseUdpEvent
{
    public override void OnInit(int eventPortId)
    {
        base.OnInit(eventPortId);
    }

    public override void OnEventDataReceived<T>(T data)
    {
        string value = data as string;
        if (value != null)
        {
            Debug.Log("收到的数据: [ " + value + " ]");
        }
    }
}
