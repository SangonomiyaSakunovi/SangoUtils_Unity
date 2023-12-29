public class DefaultNetEvent : BaseNetEvent
{
    public override void OnOperationEvent(string message)
    {
        SangoLogger.Error("A strange message Received.");
    }
}