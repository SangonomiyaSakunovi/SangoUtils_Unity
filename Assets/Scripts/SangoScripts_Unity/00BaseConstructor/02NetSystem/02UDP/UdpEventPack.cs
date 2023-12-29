using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public abstract class UdpEventPack
{
    public int listenProtId;
    public UdpClient udpListenerClient;
    public IPEndPoint udpListenerIpEndPoint;

    public Type dataType;

    protected string dataReceivedStr;

    public abstract void OnDataReceived(IAsyncResult asyncResult);
}

public class UdpEventPack<T> : UdpEventPack where T : class
{
    public override void OnDataReceived(IAsyncResult asyncResult)
    {
        try
        {
            UdpEventPack udpPack = asyncResult.AsyncState as UdpEventPack;
            byte[] dataReceivedBytes = udpPack.udpListenerClient.EndReceive(asyncResult, ref udpPack.udpListenerIpEndPoint);
            dataReceivedStr = Encoding.UTF8.GetString(dataReceivedBytes, 0, dataReceivedBytes.Length);
            SangoLogger.Log("UdpListenPortId:[" + listenProtId + "], ReceivedStr: " + dataReceivedStr);
            if (!string.IsNullOrEmpty(dataReceivedStr))
            {
                T data;
                if (typeof(T).Name == "String")
                {
                    data = dataReceivedStr as T;
                }
                else
                {
                    data = JsonUtils.DeJsonString<T>(dataReceivedStr);
                }
                if (data != null)
                {
                    UdpData udpData = new UdpData<T>(udpPack.listenProtId, data, typeof(T));
                    UdpEventService.Instance?.AddUpdEventReceivedData(udpData);
                }
            }
            udpPack.udpListenerClient.BeginReceive(udpPack.OnDataReceived, udpPack);
        }
        catch (Exception ex)
        {
            SangoLogger.Error(ex.Message);
            throw;
        }
    }
}

public abstract class UdpData
{
    public int listenProtId;
    public Type dataType;

    public abstract void OnDataSync();
}

public class UdpData<T> : UdpData where T : class
{
    public T dataReceived;

    public UdpData(int listenProtId, T dataReceived, Type dataType)
    {
        this.listenProtId = listenProtId;
        this.dataReceived = dataReceived;
        this.dataType = dataType;
    }

    public override void OnDataSync()
    {
        UdpEventService.Instance?.UdpEventBroadcast<T>(dataReceived, listenProtId);
    }
}
