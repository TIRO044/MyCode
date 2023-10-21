
public class CallBackDelegate
{
    delegate void ReadonlyCallBack(in DefaultCallBack);
}

public struct DefaultCallBack
{
    public object obj;
}

public interface IEvent
{
}

public interface IEventContainer
{
    
}

public class EventContainer
{
    public Dictionary<int, IEvent> _eventDic = new Dictionary<int, IEvent>();

    public void Register()
    {
        
    }

}
