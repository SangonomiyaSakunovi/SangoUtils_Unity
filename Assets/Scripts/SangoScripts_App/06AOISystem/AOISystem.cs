using SangoNetProtol;
using SangoUtils_Common.Infos;
using SangoUtils_Common.Messages;
using System.Collections.Generic;
using UnityEngine;

public class AOISystem : BaseSystem<AOISystem>
{
    private AOIIOCPRequest _aoiIOCPRequest;
    private AOIIOCPEvent _aoiIOCPEvent;

    private Dictionary<string, GameObject> _entityID_ObjectDict = new();

    public AOISystem()
    {
        _aoiIOCPRequest = IOCPService.Instance.GetNetRequest<AOIIOCPRequest>(NetOperationCode.Aoi);
        _aoiIOCPEvent = IOCPService.Instance.GetNetEvent<AOIIOCPEvent>(NetOperationCode.Aoi);
    }

    public void AddAOIActiveMoveEntity(string entityId, Transform transform)
    {
        Vector3Info position = new(transform.position.x, transform.position.y, transform.position.z);
        QuaternionInfo rotation = new(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        Vector3Info scale = new(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        TransformInfo transformInfo = new(position, rotation, scale);

        AOIActiveMoveEntity activeMoveEntity = new AOIActiveMoveEntity(entityId, transformInfo);
        _aoiIOCPRequest.AddAOIActiveMoveEntity(activeMoveEntity);
    }

    public void SendAOIReqMessage()
    {
        _aoiIOCPRequest.SendAOIReqMessage();
    }

    //处理退出 DestoryObject

    //处理进入

    //处理移动
}
