using UnityEngine;
using System.Collections;
using IGG.CCTwo.Data;

public class ProjectileNode : INode
{
    private float m_distanceToTarget;
    private Vector3 m_startPt;
    private Vector3 m_endPt;
    private BaseNode m_srcNode;
    private BaseNode m_objNode;
    private ActionData m_aData;
    private bool m_bMove = false;

    private TimeListenerHandler m_timeHandle;

    public ProjectileNode(ushort nodeId)
    {
        displayNode = MoveBaseMaker.Instance.GetBulletNode(nodeId);
    }

    public override void SetPlayingActionInfo(PlayingActionInfo playInfo)
    {
        base.SetPlayingActionInfo(playInfo);
        if (playInfo == null) return;

        m_aData = playInfo.actionData;
        m_srcNode = playInfo.srcNode as BaseNode;
        m_objNode = playInfo.actionData.ObjNode;
        displayNode.transform.SetParent((MapManager.Instance.CurrentMap as NormalMap).GetMapObject().transform);
        displayNode.transform.localPosition = m_startPt = m_srcNode.displayNode.transform.TransformPoint(m_srcNode.attackPoint);
        m_endPt = m_objNode.displayNode.transform.position + m_objNode.hitPoint;
        m_distanceToTarget = Vector3.Distance(m_startPt, m_endPt);

        m_timeHandle = new TimeListenerHandler(Shoot);

        TimeListenerManager.Instance.AddListener(m_timeHandle);
    }

    public void Shoot(float dt)
    {
        //朝向目标(z轴)
        displayNode.transform.LookAt(m_endPt);
        //当前距离目标点
        float currentDist = Vector3.Distance(displayNode.transform.position, m_endPt);
        //根据距离衰减角度
        float angle = Mathf.Min(1, currentDist / m_distanceToTarget) * m_srcNode.CastAngle;
        //旋转对应的角度（每帧绕X轴旋转）  
        displayNode.transform.rotation = displayNode.transform.rotation * Quaternion.Euler(-angle/*Mathf.Clamp(-angle, -42, 42)*/, 0, 0);


        //平移(朝向Z轴移动)
        displayNode.transform.Translate(Vector3.forward * Mathf.Min(m_srcNode.CastSpeed * Time.deltaTime, currentDist));

        if (currentDist < 0.5f)
        {
            m_bMove = false;
            castMoveEnd();
        }
    }

    private void castMoveEnd()
    {
        TimeListenerManager.Instance.RemoveListener(m_timeHandle);
        GameObject.Destroy(displayNode.gameObject);
        if (m_actionEndCallback != null)
            m_actionEndCallback(this);
    }
}
