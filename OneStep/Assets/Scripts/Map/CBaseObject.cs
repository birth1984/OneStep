using UnityEngine;
using System.Collections;

public class CBaseObject
{
    public float CastSpeed = 30.0f;
    public float CastAngle = 15.0f;
    public float AttackRange = 10.0f;
    public float DenfendRange = 10.0f;
    public int AttackValue = 100;
    public float AttackSpeed = 1.0f;
    public float LastAttackTime = 0f;

    public int indexId;//服务端下发的id
    public int ID;//配置表id

    protected Vector3 _pixelPos;
    protected Vector3 m_dir;

    private CBaseObject m_targetObject;

    private int m_maxHP = 300;
    private int m_maxMP = 100;
    private int m_currHP = 300;
    private int m_currMP = 0;
    private byte m_camp = 0;

    private int m_currShowHP = 300;

    private INode m_pNode;
    private byte m_walkflag;

    public CBaseObject(int id)
    {
        indexId = id;
    }

    public Vector3 GetPosition()
    {
        return _pixelPos;
    }

    public void SetPosition(Vector3 pos)
    {
        _pixelPos = pos;
    }

    public CBaseObject TargetObject
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
                //m_dir = m_targetObject.displayNode.transform.localPosition - displayNode.transform.localPosition;
            }
        }
    }

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

    public INode GetNode()
    {
        return m_pNode;
    }
    public void SetNode(INode pNode)
    {
        m_pNode = pNode;
    }

    public void Walk()
    {
        
    }

    public void Stop()
    {
        m_walkflag = 3;
    }

    public void ResumeMove()
    {
        m_walkflag = 1;
    }

    public bool IsMoving()
    {
        return m_walkflag == 1 || m_walkflag == 2;
    }

    public bool IsJump()
    {
        return m_walkflag == 2;
    }
}
