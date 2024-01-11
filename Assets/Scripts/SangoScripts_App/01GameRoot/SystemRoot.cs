public class SystemRoot
{
    public static SystemRoot Instance { get; private set; }

    public LoginSystem LoginSystem { get; private set; }
    public OperationKeyCoreSystem OperationKeyCoreSystem { get; private set; }
    public OperationKeyMoveSystem OperationKeyMoveSystem { get; private set; }

    public SystemRoot()
    {
        Instance = this;
        LoginSystem = new();
        OperationKeyCoreSystem = new();
        OperationKeyMoveSystem = new();
    }
}
