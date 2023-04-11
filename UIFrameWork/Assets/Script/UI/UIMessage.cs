namespace GBS.UI
{
    public class MessageType
    {
        public enum Type
        {
            ClickTab,
            Drag
        }
    }

    public interface IUIMessage
    {
        MessageType.Type Type { get; }
    }

    public class UIMessage_EnterGameScene : IUIMessage
    {
        public MessageType.Type Type { private set; get; }
        public UIMessage_EnterGameScene(MessageType.Type type)
        {
            Type = type;
        }
    }
}