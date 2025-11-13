using System;
using System.Collections.Generic;

public delegate void EventHandler(EventDispatcher.EventType type);
public delegate void EventHandler<in T>(EventDispatcher.EventType type, T data);

public class EventDispatcher
{
    public static readonly EventDispatcher instance = new EventDispatcher();

    public enum EventType
    {
        OnConnectedToMaster,
        OnJoinedLobby,
        OnDisconnected,
        OnCreatedRoom,
        OnRoomListUpdate,
        OnJoinedRoom,
        OnLeftRoom,
        OnPlayerEnteredRoom,
        OnPlayerLeftRoom,
    }
    
    private EventDispatcher()
    {
        
    }

    private readonly Dictionary<short, List<Delegate>> _eventHandlerDic = new Dictionary<short, List<Delegate>>();

    public void AddEventHandler(EventType type, EventHandler handler)
    {
        AddEventHandlerInternal(type, handler);
    }

    public void RemoveEventHandler(EventType type, EventHandler handler)
    {
        RemoveEventHandlerInternal(type, handler);
    }

    public void RemoveEventHandler(EventType type)
    {
        if (_eventHandlerDic.ContainsKey((short)type))
        {
            _eventHandlerDic.Remove((short)type);
        }
    }


    public void RemoveAllEventHandlers()
    {
        _eventHandlerDic.Clear();
    }

    public void AddEventHandler<T>(EventType type, EventHandler<T> handler)
    {
        AddEventHandlerInternal(type, handler);
    }

    public void RemoveEventHandler<T>(EventType type, EventHandler<T> handler)
    {
        RemoveEventHandlerInternal(type, handler);
    }

    public void SendEvent(EventType type)
    {
        var eventHandlers = GetEventHandlers(type);
        if (eventHandlers != null)
        {
            foreach (var handler in eventHandlers)
            {
                if (handler is EventHandler typedHandler)
                {
                    typedHandler(type);
                }
            }
        }
    }

    public void SendEvent<T>(EventType type, T msg)
    {
        var eventHandlers = GetEventHandlers(type);
        if (eventHandlers != null)
        {
            foreach (var handler in eventHandlers)
            {
                if (handler is EventHandler<T> typedHandler)
                {
                    typedHandler(type, msg);
                }
            }
        }
    }

    public int GetEventHandlerCount(EventType type)
    {
        var eventHandlers = GetEventHandlers(type);
        if (eventHandlers != null)
        {
            return eventHandlers.Count;
        }

        return 0;
    }


    public void Clear()
    {
        _eventHandlerDic.Clear();
    }

    private void AddEventHandlerInternal(EventType type, Delegate handler)
    {
        if (_eventHandlerDic.TryGetValue((short)type, out var eventHandlers))
        {
            if (!eventHandlers.Contains(handler))
            {
                eventHandlers.Add(handler);
            }
        }
        else
        {
            eventHandlers = new List<Delegate> { handler };
            _eventHandlerDic.Add((short)type, eventHandlers);
        }
    }

    private void RemoveEventHandlerInternal(EventType type, Delegate handler)
    {
        if (_eventHandlerDic.TryGetValue((short)type, out var eventHandlers))
        {
            eventHandlers.Remove(handler);
            if (eventHandlers.Count == 0)
            {
                _eventHandlerDic.Remove((short)type);
            }
        }
    }

    private List<Delegate> GetEventHandlers(EventType type)
    {
        if (_eventHandlerDic.TryGetValue((short)type, out var eventHandlers))
        {
            return eventHandlers;
        }

        return null;
    }
}