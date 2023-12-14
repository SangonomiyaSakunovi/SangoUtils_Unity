using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SangoGameRoot : BaseRoot<SangoGameRoot>
{
    public SceneMainInstance SceneMainInstance;

    private void Awake()
    {
        OnInit();
        SceneMainInstance.OnInit();
        DontDestroyOnLoad(this);
    }

    public override void OnInit()
    {
        base.OnInit();
        EventService.Instance.OnInit();

        SceneService.Instance.OnInit();
    }
}
