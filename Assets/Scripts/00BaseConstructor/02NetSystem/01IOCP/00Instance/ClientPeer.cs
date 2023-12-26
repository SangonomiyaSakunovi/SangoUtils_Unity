using SangoNetProtol;
using SangoUtils_Common.Utils;
using System;

public class ClientPeer : IClientPeer_IOCP
{
    #region private
    private string _uid = "";
    private long _lastMessageTimestamp = long.MinValue;
    #endregion

    public string UID { get { return _uid; } }

    protected override void OnConnected()
    {
        SangoLogger.Log("Connect to Server.");
    }

    protected override void OnDisconnected()
    {
        SangoLogger.Log("We now DisConnect.");
    }

    protected override void OnReceivedMessage(byte[] byteMessages)
    {
        SangoNetMessage sangoNetMessage = ProtoUtils.DeProtoBytes<SangoNetMessage>(byteMessages);
        long messageTimestamp = Convert.ToInt64(sangoNetMessage.NetMessageTimestamp);
        if (messageTimestamp > _lastMessageTimestamp)
        {
            _lastMessageTimestamp = messageTimestamp;
            NetService.Instance.AddNetMessageProxy(sangoNetMessage);
        }
    }
}
