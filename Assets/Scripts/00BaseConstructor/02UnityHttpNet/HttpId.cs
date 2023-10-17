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

        Type attType = typeof(HttpApiKey);
        for (int i = 0; i < fields.Length; i++)
        {
            if (fields[i].IsDefined(attType, false))
            {
                int id = (int)fields[i].GetValue(null);
                object attribute = fields[i].GetCustomAttributes(attType, false)[0];
                string api = (attribute as HttpApiKey).httpApi;
                idDic[id] = api;
            }
        }
    }

    private static Dictionary<int, string> idDic = new Dictionary<int, string>();

    public static string GetHttpApi(int httpId)
    {
        idDic.TryGetValue(httpId, out var api);
        return api;
    }
}


public class HttpApiKey : Attribute
{
    public HttpApiKey(string _httpApi)
    {
        httpApi = _httpApi;
    }
    public string httpApi;
}