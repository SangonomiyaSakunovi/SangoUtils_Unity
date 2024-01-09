using SangoUtils_Common.Messages;

public class AOIIOCPEvent : BaseNetEvent
{
    public override void OnEventData(string message)
    {
        SangoLogger.Done("AOI EventMessage: " + message);
        AOIEventMessage eventMessage = DeJsonString<AOIEventMessage>(message);
        if (eventMessage != null)
        {
            SceneMainInstance.Instance.OnAOIOperationEvent(eventMessage);
        }
    }
}
