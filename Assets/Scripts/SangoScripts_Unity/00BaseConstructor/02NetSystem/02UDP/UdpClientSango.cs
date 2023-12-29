using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public abstract class UdpClientSango
{
    public int _udpListenerPortId;
    protected Type _dataType;
    protected Thread _receiveUdpMessageThread;

    protected abstract void ThreadReceive<T>() where T : class;
}

public class UdpClientSango<T> : UdpClientSango where T : class
{
    public UdpClientSango(int udpListenerPort)
    {
        _udpListenerPortId = udpListenerPort;
        _dataType = typeof(T);
        ThreadReceive<T>();
    }

    protected override void ThreadReceive<K>() where K : class
    {
        _receiveUdpMessageThread = new Thread(() =>
        {
            IPEndPoint udpListenerIpEndPoint = new IPEndPoint(IPAddress.Any, _udpListenerPortId);
            UdpClient udpListenerClient = new UdpClient(udpListenerIpEndPoint);
            UdpEventPack<K> udpPack = new UdpEventPack<K>();
            udpPack.listenProtId = _udpListenerPortId;
            udpPack.udpListenerIpEndPoint = udpListenerIpEndPoint;
            udpPack.udpListenerClient = udpListenerClient;
            udpPack.dataType = _dataType;
            udpPack.udpListenerClient.BeginReceive(udpPack.OnDataReceived, udpPack);
        })
        {
            IsBackground = true
        };
        _receiveUdpMessageThread.Start();
    }
}