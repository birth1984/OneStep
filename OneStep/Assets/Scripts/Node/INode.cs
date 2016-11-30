using UnityEngine;
using System.Collections;
using IGG.CCTwo.Data;

public delegate void ActionEndDelegate(INode pNode);

public class INode
{
    public GameObject displayNode;

    protected Vector3 _pixelPos;
    protected ActionEndDelegate m_actionEndCallback;

    private PlayingActionInfo m_playingActionInfo;

    public INode()
    {

    }

    public PlayingActionInfo GetPlayingActionInfo()
    {
        return m_playingActionInfo;
    }

    public virtual void SetPlayingActionInfo(PlayingActionInfo playInfo)
    {
        m_playingActionInfo = playInfo;
    }

    public Vector3 GetPosition()
    {
        return _pixelPos;
    }

    public virtual void SetPosition(Vector3 pos)
    {
    }

    public void SetActionEndCallback(ActionEndDelegate callback)
    {
        m_actionEndCallback = callback;
    }
}
