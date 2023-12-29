public class SceneSystemEventMessage
{
    public class ChangeToHomeScene : IEventMessageBase
    {
        public static void SendEventMessage()
        {
            var msg = new ChangeToHomeScene();
            EventService.Instance.SendEventMessage(msg);
        }
    }

    public class ChangeToBattleScene : IEventMessageBase
    {
        public static void SendEventMessage()
        {
            var msg = new ChangeToBattleScene();
            EventService.Instance.SendEventMessage(msg);
        }
    }
}
