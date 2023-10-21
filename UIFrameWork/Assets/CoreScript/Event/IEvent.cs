
using System.Collections.Generic;

public class CallBackDelegate
{
    public delegate void ReadonlyCallBack(in DefaultCallBack);

    public delegate void CallBack();
}

public struct DefaultCallBack
{
    public object obj;
}

public class TempCallBack
{
    private List<CallBackDelegate.ReadonlyCallBack> _callBackConatiner = new List<CallBackDelegate.ReadonlyCallBack>();

    public void AddCallBack()
    {
        
    }
}

public interface IEventContainer
{
    
}

public class EventContainer
{
    public Dictionary<int, TempCallBack> _eventDic = new Dictionary<int, TempCallBack>();

    private volatile int _count;
    
    
    public void RegisterCallBack()
    {
        
    }
}
