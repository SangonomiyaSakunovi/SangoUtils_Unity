

using SangoUtils_Logger;
using SangoUtils_NetOperation;

public class DefaultIOCPRequest : BaseNetRequest
{
    protected override void DefaultOperationRequest()
    {

    }

    public override void OnOperationResponse(string message)
    {
        SangoLogger.Error("A strange message Received.");
    }
}

public class DefaultIOCPEvent : BaseNetEvent
{
    public override void OnEventData(string message)
    {
        SangoLogger.Warning("A Net EventData have no Implement!");
    }
}

public class DefaultWebSocketRequest : BaseNetRequest
{
    public override void OnOperationResponse(string message)
    {
        SangoLogger.Warning("A Net OperationResponse have no Implement!");
    }

    protected override void DefaultOperationRequest()
    {
        throw new System.NotImplementedException();
    }
}

public class DefaultWebSocketEvent : BaseNetEvent
{
    public override void OnEventData(string message)
    {
        SangoLogger.Warning("A Net EventData have no Implement!");
    }
}

public class DefaultWebSocketBroadcast : BaseNetBroadcast
{
    public override void DefaultOperationBroadcast()
    {
        throw new System.NotImplementedException();
    }

    public override void OnBroadcast(string message)
    {
        SangoLogger.Warning("A Net Broadcast have no Implement!");
    }
}

public class WebSocketEvent_Ping : BaseNetEvent
{
    public override void OnEventData(string message)
    {
        SangoLogger.Log("A ping message from the Server: " + message);
    }
}