using System;

public class NetEventMessageBroadcast : BaseNetEventMessageBroadcast
{
    public override void OnMessageReceived(string message)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class WebsocketEventMessage_Head_SunUp
{
    public string MessageHead { get; set; } = "";
    public string MessageBody { get; set; } = "";
}

[Serializable]
public class WebsocketEventMessage_Login_SunUp
{
    public string client_id { get; set; } = "";
}
