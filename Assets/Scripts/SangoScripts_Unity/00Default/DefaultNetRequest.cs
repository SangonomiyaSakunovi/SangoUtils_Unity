public class DefaultNetRequest : BaseNetRequest
{
    protected override void DefaultOperationRequest()
    {

    }

    public override void OnOperationResponse(string message)
    {
        SangoLogger.Error("A strange message Received.");
    }
}
