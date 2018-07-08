using System;

//http://unity3d.ru/distribution/viewtopic.php?f=13&t=24933

public static class EventDispatcher<EventType>
{
    static public event Action<EventType> OnEvent;
    static public void Broadcast(EventType data)
    {
        if (OnEvent != null)
            OnEvent(data);
    }
}

static public class EventDispatcherExtension
{
    static public void Broadcast<T>(this T data)
    {
        EventDispatcher<T>.Broadcast(data);
    }
}

//public class EventDispatcherTester
//{
//    public struct OnTestInvokeEvent
//    {
//        public string Test;
//    }

//    public EventDispatcherTester()
//    {
//        EventDispatcher<OnTestInvokeEvent>.OnEvent += OnSomeHappened;
//    }

//    void OnSomeHappened(OnTestInvokeEvent obj)
//    {
//    }

//    public void OnDie()
//    {
//        new OnTestInvokeEvent() { Test = "Succes" }.Broadcast();
//    }
//}