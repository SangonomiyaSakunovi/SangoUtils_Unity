using System;
using System.Collections.Generic;

public class UdpEventListenPortId
{
    [UdpEventPortApiKey("TypeInPort")]
    public const int typeInPort = 52022;

    public UdpEventListenPortId()
    {
        System.Reflection.FieldInfo[] fields = typeof(UdpEventListenPortId).GetFields();

        Type attributeType = typeof(UdpEventPortApiKey);
        for (int i = 0; i < fields.Length; i++)
        {
            if (fields[i].IsDefined(attributeType, false))
            {
                int id = (int)fields[i].GetValue(null);
                object attribute = fields[i].GetCustomAttributes(attributeType, false)[0];
                string api = (attribute as UdpEventPortApiKey).udpEventApi;
                idDict[id] = api;
            }
        }
    }

    private static Dictionary<int, string> idDict = new Dictionary<int, string>();

    public static string GetUdpEventApi(int udpEventId)
    {
        idDict.TryGetValue(udpEventId, out var api);
        return api;
    }
}

public class UdpEventPortApiKey : Attribute
{
    public string udpEventApi;

    public UdpEventPortApiKey(string udpEventApi)
    {
        this.udpEventApi = udpEventApi;
    }
}
