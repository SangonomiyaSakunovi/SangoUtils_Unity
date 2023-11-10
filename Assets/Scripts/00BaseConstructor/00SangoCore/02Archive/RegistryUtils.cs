using Microsoft.Win32;
using System.Linq;

public static class RegistryUtils
{
    public static RegistryKey GetOrAddRootRegistryPathKey(string registryFileName, bool writable = true)
    {
        RegistryKey key_SoftwareName = null;
        RegistryKey key_SOFTWARE_SystemTag = Registry.LocalMachine.OpenSubKey("SOFTWARE", writable);
        string[] subKeyNames = key_SOFTWARE_SystemTag.GetSubKeyNames();
        if (subKeyNames.Contains(registryFileName))
        {
            key_SoftwareName = key_SOFTWARE_SystemTag.OpenSubKey(registryFileName);
        }
        else
        {
            key_SoftwareName = key_SOFTWARE_SystemTag.CreateSubKey(registryFileName);
        }
        return key_SoftwareName;
    }

    public static RegistryKey GetOrAddSubPathKey(RegistryKey parentKey, string subKeyName, bool writable = true)
    {
        RegistryKey subKey = null;
        string[] subKeyNames = parentKey.GetSubKeyNames();
        if (subKeyNames.Contains(subKeyName))
        {
            subKey = parentKey.OpenSubKey(subKeyName);
        }
        else
        {
            subKey = parentKey.CreateSubKey(subKeyName, writable);
        }
        return subKey;
    }

    public static bool DeleteSubPathKey(RegistryKey parentKey, string subKeyName)
    {
        bool res = false;
        string[] subKeyNames = parentKey.GetSubKeyNames();
        if (subKeyNames.Contains(subKeyName))
        {
            parentKey.DeleteSubKey(subKeyName);
            res = true;
        }
        return res;
    }

    public static object ReadValue(RegistryKey registryKey, string value)
    {
        return registryKey.GetValue(value);
    }

    public static void SetValue(RegistryKey registryKey, string key, object value)
    {
        registryKey.SetValue(key, value);
    }

    public static void DeletValue(RegistryKey registryKey, string key)
    {
        registryKey.DeleteValue(key);
    }

    public static bool HasRegistryValueKeyExist(RegistryKey registryKey, string key)
    {
        bool res = false;
        string[] valueNames = registryKey.GetValueNames();
        if (valueNames.Contains(key))
        {
            res = true;
        }
        return res;
    }
}
