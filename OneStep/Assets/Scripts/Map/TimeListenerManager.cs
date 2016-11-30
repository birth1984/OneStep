using UnityEngine;
using System.Collections.Generic;
using System;

public delegate void TimeListenerHandler(float dt);

public class TimeListenerManager
{
    private static TimeListenerManager _instance;

    private List<TimeListenerHandler> _frameListenerList;
    private List<TimeListener> _timeListenerList;
    private List<EventArgs> _paramList;
    private float _prevTime;

    private TimeListenerManager()
    {
        _frameListenerList = new List<TimeListenerHandler>();
        _timeListenerList = new List<TimeListener>();
    }

    public static TimeListenerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TimeListenerManager();
            }
            return _instance;
        }
    }

    public bool HasListener(TimeListenerHandler handler)
    {
        foreach (var listener in _frameListenerList)
        {
            if (handler == listener)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasTimeListener(TimeListenerHandler handler)
    {
        foreach (var timeListener in _timeListenerList)
        {
            if (handler == timeListener.listener)
            {
                return true;
            }
        }
        return false;
    }

    public void AddListener(TimeListenerHandler handler)
    {
        if (_frameListenerList.Count == 0 && _timeListenerList.Count == 0)
            _prevTime = Time.time;

        _frameListenerList.Add(handler);
    }

    public void RemoveListener(TimeListenerHandler handler)
    {
        foreach(var listener in _frameListenerList)
        {
            if (handler == listener)
            {
                _frameListenerList.Remove(handler);
                return;
            }
        }
    }

    public void AddTimeListener(TimeListenerHandler handler, float delay,int handleCount)
    {
        if (_frameListenerList.Count == 0 && _timeListenerList.Count == 0)
            _prevTime = Time.time;

        _timeListenerList.Add(new TimeListener(delay, handler, Time.time, handleCount));
    }

    public void RemoveTimeListener(TimeListenerHandler handler)
    {
        foreach (var timeListener in _timeListenerList)
        {
            if (handler == timeListener.listener)
            {
                _timeListenerList.Remove(timeListener);
                return;
            }
        }
    }

    public void Update()
    {
        if (_frameListenerList.Count < 1 && _timeListenerList.Count < 1)
            return;

        float interval = Time.time - _prevTime;
        _prevTime = Time.time;
        int i = _frameListenerList.Count;
        //TODO:在循环执行过程中，如果删除本次循环前面的回调，会导致本次循环多执行一次
        while (i-- > 0)
        {
            _frameListenerList[i](interval);
        }

        int j = _timeListenerList.Count;
        while (j-- > 0)
        {
            bool needRemove = _timeListenerList[j].Update(Time.time);
            if (needRemove)
                RemoveTimeListener(_timeListenerList[j].listener);
        }
    }
}

public class TimeListener
{
    private float _delay;
	private TimeListenerHandler _listener;
    //上一次的时间
    private float _prevTime;
    //执行次数,-1表示一致执行
    private int handleNumber = -1;

    public TimeListener(float delay, TimeListenerHandler listener, float curTime, int handleNumber = -1)
    {
		_delay = delay;
		_listener = listener;
		_prevTime = curTime;

        this.handleNumber = handleNumber;
    }

    public TimeListenerHandler listener
    {
        get
        {
            return _listener;
        }
    }

    /**
     * 更新时间, 如果时间一到就调用函数.
     * @param curTime			当前时间
     * @return 是否需要移除
     */
    public bool Update(float curTime)
    {
        float interval = curTime - _prevTime;
        if (interval >= _delay)
        {
			_prevTime = curTime;
			_listener(interval);
			if(handleNumber >= 1)
                handleNumber -= 1;
		}
		if(handleNumber == 0)//移除监听
		    return true;
		else
            return false;
	}
}
