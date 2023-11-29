using System;
using System.Collections.Generic;

public class HttpId
{
    [HttpApiKey("Register")]
    public const int registerId = 10001;
    [HttpApiKey("Login")]
    public const int loginId = 10002;

    static HttpId()
    {
        System.Reflection.FieldInfo[] fields = typeof(HttpId).GetFields();

        Type attributeType = typeof(HttpApiKey);
        for (int i = 0; i < fields.Length; i++)
        {
            if (fields[i].IsDefined(attributeType, false))
            {
                int id = (int)fields[i].GetValue(null);
                object attribute = fields[i].GetCustomAttributes(attributeType, false)[0];
                string api = (attribute as HttpApiKey).httpApi;
                idDict[id] = api;
            }
        }
    }

    private static Dictionary<int, string> idDict = new Dictionary<int, string>();

    public static string GetHttpApi(int httpId)
    {
        idDict.TryGetValue(httpId, out var api);
        return api;
    }
}


public class HttpApiKey : Attribute
{
    public string httpApi;

    public HttpApiKey(string httpApi)
    {
        this.httpApi = httpApi;
    }
}