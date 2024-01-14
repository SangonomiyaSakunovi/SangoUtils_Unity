using SangoUtils_Logger;
using SangoUtils_NetOperation;

public class PingWebSocketEvent : BaseNetEvent
{
    public override void OnEventData(string message)
    {
        SangoLogger.Log("A ping message from the Server: " + message);
    }
}
