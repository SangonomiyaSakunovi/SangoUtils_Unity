using SangoNetProtol;
using System.Collections.Generic;
using UnityEngine;

public class AOISystem : BaseSystem<AOISystem>
{
    private AOINetEvent _aoiNetEvent;
    private Dictionary<string, GameObject> _entityID_ObjectDict = new();

    public override void OnInit()
    {
        base.OnInit();
        _instance = this;
        _aoiNetEvent = NetService.Instance.GetNetEvent<AOINetEvent>(NetOperationCode.Aoi);
    }

    //�����˳� DestoryObject

    //�������

    //�����ƶ�
}
