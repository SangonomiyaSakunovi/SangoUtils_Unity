using SangoNetProtol;
using SangoUtils_Common.Infos;
using SangoUtils_Common.Messages;
using System.Collections.Generic;
using UnityEngine;

public class AOISystem : BaseSystem<AOISystem>
{
    private AOINetRequest _aoiNetRequest;
    private AOINetEvent _aoiNetEvent;
    private Dictionary<string, GameObject> _entityID_ObjectDict = new();

    public override void OnInit()
    {
        base.OnInit();
        _instance = this;
        _aoiNetRequest = NetService.Instance.GetNetRequest<AOINetRequest>(NetOperationCode.Aoi);
        _aoiNetEvent = NetService.Instance.GetNetEvent<AOINetEvent>(NetOperationCode.Aoi);
    }

    public void AddAOIActiveMoveEntity(string entityId, Transform transform)
    {
        Vector3Info position = new(transform.position.x, transform.position.y, transform.position.z);
        QuaternionInfo rotation = new(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        Vector3Info scale = new(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        TransformInfo transformInfo = new(position, rotation, scale);

        AOIActiveMoveEntity activeMoveEntity = new AOIActiveMoveEntity(entityId, transformInfo);
        _aoiNetRequest.AddAOIActiveMoveEntity(activeMoveEntity);
    }

    public void SendAOIReqMessage()
    {
        _aoiNetRequest.SendAOIReqMessage();
    }

    //处理退出 DestoryObject

    //处理进入

    //处理移动
}
