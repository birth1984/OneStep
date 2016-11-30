using UnityEngine;
using System.Collections;
using IGG.CCTwo.Data;

public class ActionNode : INode
{
    private BaseNode m_srcNode;
    private BaseNode m_objNode;

    public ActionNode(ushort nodeId)
    {
        displayNode = MoveBaseMaker.Instance.GetBulletNode(nodeId);
        //DoAction();
    }

    public override void SetPlayingActionInfo(PlayingActionInfo playInfo)
    {
        base.SetPlayingActionInfo(playInfo);
        if (playInfo == null) return;
        m_srcNode = playInfo.srcNode as BaseNode;
        m_objNode = playInfo.actionData.ObjNode;
        //TimeListenerManager.Instance.AddTimeListener(new TimeListenerHandler(particlePlayEnd), 0.05f, 1);
    }

    public void DoAction()
    {
        ParticleController pc = displayNode.transform.GetComponent<ParticleController>();
        pc.SetParticleCallback(new ParticlePlayEndDelegate(particlePlayEnd));
    }

    private void particlePlayEnd()
    {
        if (m_actionEndCallback != null)
            m_actionEndCallback(this);
    }
}
