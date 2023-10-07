using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _01System : BaseSystem
{
    public static _01System Instance = null;
    private List<_01ClassInfo> _01ClassInfos = null;
    private _01Window _01window = null;

    private void Start()
    {
        InitSystem();
    }

    public override void InitSystem()
    {
        Instance = this;
        _01window = GetComponent<_01Window>();
        _01ClassInfos = new List<_01ClassInfo>();
        SetTestInfo();
        _01window.InstantiatePrefab();
    }

    private void SetTestInfo()
    {
        int[] ids = { 1, 2, 3 };
        string[] heads = { "one", "two", "three" };
        string[] contents = { "111111111111111111111111111111111111111111111111111111111111111111", "222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222", "333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333" };
        for (int i = 0; i < ids.Length; i++)
        {
            _01ClassInfo info = new _01ClassInfo(ids[i], heads[i], contents[i]);
            _01ClassInfos.Add(info);
        }
    }

    public List<_01ClassInfo> GetClassInfo()
    {
        return _01ClassInfos;
    }
}

public class _01ClassInfo
{
    public int Id { get; private set; }
    public string Head { get; private set; }
    public string Content { get; private set; }

    public _01ClassInfo(int id, string head, string content)
    {
        Id = id;
        Head = head;
        Content = content;
    }
}
