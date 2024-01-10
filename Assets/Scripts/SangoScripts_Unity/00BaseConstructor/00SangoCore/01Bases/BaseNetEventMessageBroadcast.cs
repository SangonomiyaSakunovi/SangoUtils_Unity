public abstract class BaseNetEventMessageBroadcast
{
    public abstract void OnMessageReceived(string message);

    public abstract void OnBinaryReceived(byte[] buffer);
}
