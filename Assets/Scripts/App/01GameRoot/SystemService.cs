using SangoUtils_Bases_UnityEngine;
using SangoUtils_Unity_App.Operation;

public class SystemService : BaseService<SystemService>
{   
    public LoginSystem LoginSystem { get; private set; }
    public OperationKeyCoreSystem OperationKeyCoreSystem { get; private set; }
    public OperationKeyMoveSystem OperationKeyMoveSystem { get; private set; }

    public override void OnInit()
    {
        LoginSystem = new();
        OperationKeyCoreSystem = new();
        OperationKeyMoveSystem = new();
    }

    protected override void OnUpdate()
    {
        
    }

    public override void OnDispose()
    {
        
    }
}
