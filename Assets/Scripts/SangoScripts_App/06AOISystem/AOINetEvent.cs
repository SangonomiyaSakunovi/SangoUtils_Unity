using SangoUtils_Common.Messages;

public class AOINetEvent : BaseNetEvent
{
    public override void OnOperationEvent(string message)
    {
        SangoLogger.Done("AOI EventMessage: " + message);
        AOIEventMessage eventMessage = DeJsonString<AOIEventMessage>(message);
        if (eventMessage != null)
        {
            SceneMainInstance.Instance.OnAOIOperationEvent(eventMessage);
        }
    }
}
