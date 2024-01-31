using SangoUtils_Common.Messages;
using SangoUtils_Logger;
using SangoUtils_NetOperation;

public class AOIIOCPEvent : BaseNetEvent
{
    public override void OnEventData(string message)
    {
        SangoLogger.Done("AOI EventMessage: " + message);
        AOIEventMessage eventMessage = FromJson<AOIEventMessage>(message);
        if (eventMessage != null)
        {
            SangoLogger.Warning("Why we received a message from AOISystem?");
        }
    }
}
