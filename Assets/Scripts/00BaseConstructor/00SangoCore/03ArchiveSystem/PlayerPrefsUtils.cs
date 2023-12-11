using UnityEngine;

public static class PlayerPrefsUtils
{
    public static bool AddPersistData(string key, string value)
    {
        bool res = false;
        PlayerPrefs.SetString(key, value);
        if (PlayerPrefs.HasKey(key))
        {
            if (PlayerPrefs.GetString(key) == value)
            {
                res = true;
            }
        }
        return res;
    }

    public static string GetPersistData(string key)
    {
        string res = "";
        if (PlayerPrefs.HasKey(key))
        {
            res = PlayerPrefs.GetString(key);
        }
        return res;
    }

    public static void RemovePersistData(string key)
    {        
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}
