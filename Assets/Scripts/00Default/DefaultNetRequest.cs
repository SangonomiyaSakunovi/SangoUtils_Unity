public class DefaultNetRequest : BaseNetRequest
{
    public override void DefaultRequest()
    {

    }

    public override void OnOperationResponse(string message)
    {
        SangoLogger.Error("A strange message Received.");
    }
}
