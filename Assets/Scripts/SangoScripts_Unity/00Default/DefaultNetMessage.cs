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
        SangoLogger.Error("A strange message Received.");
    }
}

public class DefaultWebSocketRequest : BaseNetRequest
{
    public override void OnOperationResponse(string message)
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }
}