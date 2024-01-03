using System.Collections.Generic;

public class FSMStaterBase
{
    protected Dictionary<string, object> _blackboard = new();

    public void SetBlackboardValue(string key, object value)
    {
        if (_blackboard.ContainsKey(key))
        {
            _blackboard[key] = value;
        }
        else
        {
            _blackboard.Add(key, value);
        }
    }

    public object GetBlackboardValue(string key)
    {
        if (_blackboard.TryGetValue(key, out object value))
        {
            return value;
        }
        return null;
    }
}
