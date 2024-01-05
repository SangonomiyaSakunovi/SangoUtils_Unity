using SangoUtils_Common.Messages;

public class AOINetRequest : BaseNetRequest
{
    private AOIReqMessage _message = new();

    public void AddAOIActiveMoveEntity(AOIActiveMoveEntity activeMoveEntity)
    {
        _message.AOIActiveMoveEntitys.Add(activeMoveEntity);
    }

    public void SendAOIReqMessage()
    {
        DefaultOperationRequest();
        _message.AOIActiveMoveEntitys.Clear();
    }

    protected override void DefaultOperationRequest()
    {
        string jsonString = SetJsonString(_message);
        NetService.Instance.SendOperationRequest(NetOperationCode, jsonString);
    }

    public override void OnOperationResponse(string message)
    {
        SangoLogger.Error("Why the AOI Request can receive a response???");
    }
}
