public class SceneService : BaseService<SceneService>
{
    private EventCache _eventCache;
    private SceneViewConfig _currentSceneViewConfig;

    public override void OnInit()
    {
        base.OnInit();
        _currentSceneViewConfig = SangoSystemConfig.SceneViewConfig;
        AddEvent();
    }

    private void AddEvent()
    {
        _eventCache = new EventCache();
        _eventCache.AddEventListener<SceneSystemEventMessage.ChangeToHomeScene>(OnHandleEventMessage);
        _eventCache.AddEventListener<SceneSystemEventMessage.ChangeToBattleScene>(OnHandleEventMessage);
    }

    private void OnHandleEventMessage(IEventMessageBase message)
    {
        SceneMainInstance.Instance.OnHandleEventMessage(message);
    }
}

public enum SceneViewResolution
{
    _1KH_1920x1080,
    _1KV_1080x1920,
    _4KH_3840x2160,
    _4kV_2160x3840
}

public class SceneViewConfig
{
    public SceneViewResolution sceneViewResolution;
}
