public class PersistDataService : BaseService<PersistDataService>
{
    private PersistDataType _persistDataType = PersistDataType.PlayerPrefs;

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    public bool AddPersistData(string key, string value)
    {
        bool res = false;
        switch (_persistDataType)
        {
            case PersistDataType.PlayerPrefs:
                res = PlayerPrefsUtils.AddPersistData(key, value);
                break;
        }
        return res;
    }

    public string GetPersistData(string key)
    {
        string res = "";
        switch (_persistDataType)
        {
            case PersistDataType.PlayerPrefs:
                res = PlayerPrefsUtils.GetPersistData(key);
                break;
        }
        return res;
    }

    public bool RemovePersistData(string key)
    {
        bool res = false;
        switch (_persistDataType)
        {
            case PersistDataType.PlayerPrefs:
                PlayerPrefsUtils.RemovePersistData(key);
                res = true;
                break;
        }
        return res;
    }
}

public enum PersistDataType
{
    PlayerPrefs,
    Registry
}
