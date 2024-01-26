using SangoNetProtol;
using SangoUtils_Unity_Scripts.Net;
using SangoUtils_Common.Messages;
using SangoUtils_FixedNum;
using System.Collections.Generic;
using UnityEngine;
using SangoUtils_Bases_Universal;

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

    public void AddAOIActiveMoveEntity(string entityId, FixedVector3 logicPosition)
    {
        Vector3Message position = new(logicPosition.X.ScaledValue, logicPosition.Y.ScaledValue, logicPosition.Z.ScaledValue);
        //QuaternionMessage rotation = new(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        //Vector3Message scale = new(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //TransformMessage transformInfo = new(position, rotation, scale);
        TransformMessage transformInfo = new();

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
