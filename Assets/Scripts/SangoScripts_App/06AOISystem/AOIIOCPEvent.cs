using SangoUtils_Common.Messages;
using SangoUtils_Logger;

public class AOIIOCPEvent : BaseNetEvent
{
    public override void OnEventData(string message)
    {
        SangoLogger.Done("AOI EventMessage: " + message);
        AOIEventMessage eventMessage = DeJsonString<AOIEventMessage>(message);
        if (eventMessage != null)
        {
            SangoLogger.Warning("Why we received a message from AOISystem?");
        }
    }
}
