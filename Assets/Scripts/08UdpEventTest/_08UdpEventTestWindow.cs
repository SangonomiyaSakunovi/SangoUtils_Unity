using TMPro;
using UnityEngine;

public class _08UdpEventTestWindow : MonoBehaviour
{
    public TMP_Text _receiveDataString;
    public GameObject _receivedObj;

    public static _08UdpEventTestWindow Instance;

    private void Start()
    {
        Instance = this;
        UdpEventService.Instance.OnInit();
        InitEvent();
    }

    private void InitEvent()
    {
        _08UdpEvent @event = UdpEventService.Instance.GetUdpEvent<_08UdpEvent>(UdpEventListenPortId.typeInPort);
    }

    public void OnReceivedObj(string str)
    {
        _receivedObj.SetActive(true);
        _receiveDataString.text = str;
    }
}
