using UnityEngine;
using System.Collections;
using IGG.CCTwo.Data;

public class ExplosionNode : INode
{
    private BaseNode m_srcNode;
    private BaseNode m_objNode;

    public ExplosionNode(ushort nodeId)
    {
        displayNode = MoveBaseMaker.Instance.GetBulletNode(nodeId);
        //ParticleController pc = displayNode.transform.GetComponent<ParticleController>();
        //pc.SetParticleCallback(new ParticlePlayEndDelegate(particlePlayEnd));
    }

    public override void SetPlayingActionInfo(PlayingActionInfo playInfo)
    {
        base.SetPlayingActionInfo(playInfo);
        if (playInfo == null) return;
        m_srcNode = playInfo.srcNode as BaseNode;
        m_objNode = playInfo.actionData.ObjNode;
        displayNode.transform.SetParent((MapManager.Instance.CurrentMap as NormalMap).GetMapObject().transform);
        displayNode.transform.localPosition = /*m_srcNode.displayNode.transform.localPosition;*/m_objNode.displayNode.transform.TransformPoint(m_objNode.hitPoint);

        TimeListenerManager.Instance.AddTimeListener(new TimeListenerHandler(particlePlayEnd), 0.05f, 1);
    }

    private void particlePlayEnd(float dt)
    {
        if(m_actionEndCallback != null)
            m_actionEndCallback(this);
    }
}
