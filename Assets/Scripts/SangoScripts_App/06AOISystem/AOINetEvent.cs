public class AOINetEvent : BaseNetEvent
{
    public override void OnOperationEvent(string message)
    {
        SangoLogger.Done("AOI EventMessage: " + message);
    }
}
