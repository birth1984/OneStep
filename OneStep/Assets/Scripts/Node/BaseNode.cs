using UnityEngine;
using System.Collections;

public class BaseNode : INode
{
    public float CastSpeed = 30.0f;
    public float CastAngle = 15.0f;
    public float AttackRange = 10.0f;
    public float DenfendRange = 15.0f;
    public int AttackValue = 100;
    public float AttackSpeed = 2.0f;
    public float LastAttackTime = 0f;
    public Vector3 RegressPosition;

    public int indexId;//服务端下发的id
    public int ID;//配置表id

    protected Vector3 _cellPos;
    protected Vector3 m_dir;
    
    protected Vector3 m_attackPoint;
    protected Vector3 m_hitPoint;
    //private BaseNode m_targetObject;

    private int m_maxHP = 300;
    private int m_maxMP = 100;
    private int m_currHP = 300;
    private int m_currMP = 0;
    private byte m_camp = 0;

    private int m_currShowHP = 300;

    public BaseNode()
    {
        IsEmissionWatting = false;
        LastAttackTime = -AttackSpeed;
    }

    public Vector3 GetCellPosition()
    {
        return _cellPos;
    }

    public override void SetPosition(Vector3 pos)
    {
        _pixelPos = displayNode.transform.localPosition = pos;
        _cellPos = MapManager.Instance.GetCellPosition(_pixelPos);
    }

    public virtual void SetDir(Vector3 dir)
    {
        m_dir = dir;
        displayNode.transform.forward = dir;
    }
    public Vector3 GetDir()
    {
        return m_dir;
    }

    public Vector3 attackPoint
    {
        get
        {
            return m_attackPoint;
        }
    }

    public Vector3 hitPoint
    {
        get
        {
            return m_hitPoint;
        }
    }

    /*public BaseNode TargetObject
    {
        get
        {
            return m_targetObject;
        }
        set
        {
            m_targetObject = value;
            if (m_targetObject != null)
            {
                Vector3 targetDir = m_targetObject.displayNode.transform.localPosition - displayNode.transform.localPosition;
                SetDir(targetDir);
            }
        }
    }*/

    public byte Camp
    {
        get { return m_camp; }
        set { m_camp = value; }
    }

    public void SetCurrentHP(int damageValue)
    {
        m_currHP -= damageValue;
        if (m_currHP < 0) m_currHP = 0;
    }
    public int GetCurrentHP()
    {
        return m_currHP;
    }

    public int GetCurrentMP()
    {
        return m_currMP;
    }
    public void SetCurrentMP(int damageValue)
    {
        m_currMP += damageValue;
    }

    public bool IsEmissionWatting { get; set; }

    public virtual void SetRoleStatus(E_NodeStatus state)
    {
    }
    public virtual E_NodeStatus GetRoleStatus()
    {
        return E_NodeStatus.Idle;
    }

    public virtual void NodeDead()
    {
    }

    public void destroy()
    {
        GameObject.Destroy(displayNode);
    }
}
