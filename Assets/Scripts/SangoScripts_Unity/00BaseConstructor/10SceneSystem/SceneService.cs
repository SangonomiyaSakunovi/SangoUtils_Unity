using SangoUtils_Bases_UnityEngine;
using SangoUtils_Bases_Universal;
using SangoUtils_Event;
using System;

public class SceneService : BaseService<SceneService>
{
    private EventCache _eventCache;
    private SceneViewConfig _currentSceneViewConfig;

    private Action<IEventMessageBase> _onHandleEventMessage;

    public override void OnInit()
    {
        base.OnInit();
        AddEvent();
    }

    public void SetConfig(SceneViewConfig sceneViewConfig)
    {
        _currentSceneViewConfig = sceneViewConfig;
    }

    public void SetHandleEventMessageCallBack(Action<IEventMessageBase> onHandleEventMessage)
    {
        _onHandleEventMessage = onHandleEventMessage;
    }

    private void AddEvent()
    {
        _eventCache = new EventCache();
        _eventCache.AddEventListener<SceneSystemEventMessage.ChangeToHomeScene>(OnHandleEventMessage);
        _eventCache.AddEventListener<SceneSystemEventMessage.ChangeToBattleScene>(OnHandleEventMessage);
    }

    public void AddEvent(int eventHash)
    {
        _eventCache.AddEventListener(eventHash, OnHandleEventMessage);
    }

    private void OnHandleEventMessage(IEventMessageBase message)
    {
        _onHandleEventMessage?.Invoke(message);
    }
}

public enum SceneViewResolution
{
    _1KH_1920x1080,
    _1KV_1080x1920,
    _4KH_3840x2160,
    _4kV_2160x3840
}

public class SceneViewConfig : BaseConfig
{
    public SceneViewResolution SceneViewResolution { get; set; }
}
