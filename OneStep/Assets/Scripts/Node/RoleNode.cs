using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum E_NodeStatus
{
    Idle,
    walk,
    attack,
    skill,
    jump,
    hit,
    die,
    cheer,
}

public class MoveData
{
    public float SpeedUp;//单体加速之用.
    public float MoveSpeed;//单体加速之用.
    public float MoveDelay;
    public float dx;
    public float dz;
    public float delaX;
    public float delaY;
    public float Jumph;
    public float JumpHeight;
    public float JumpDuration;
    public float JumpElap;
    public int MoveToX;
    public int MoveToY;

    public MoveData()
    {

    }

    public void Clear()
    {
        MoveToX = -9999;
        MoveToY = -9999;
        JumpHeight = 0;
        Jumph = 0;
        dx = 0;
        dz = 0;
        MoveDelay = 0;
    }
};

public class RoleNode : BaseNode
{

    public int buildingIndexId;//所属建筑id

    public MoveData moveData;

    public byte level;
    public List<ushort> PathList;
    
    private bool _bIsMoving = false;
    
    protected PlayerController m_playerController;

    public RoleNode()
    {
        moveData = new MoveData();
        PathList = new List<ushort>();
        moveData.MoveSpeed = 5;
        m_attackPoint = new Vector3(0, 1.2f, 1);
        m_hitPoint = new Vector3(0, 0.9f, 0);
    }

    protected void initNode()
    {
        m_playerController = displayNode.GetComponent<PlayerController>();
        m_playerController.SetNodeData(this);
    }

    public bool IsMoving()
    {
        return _bIsMoving;
    }

    public float MoveTo(float ToX, float ToZ, float speedUp)
    {
        if (_bIsMoving)
        {
            if (ToX == moveData.MoveToX && ToZ == moveData.MoveToY)
                return 0;
        }

        float dx = ToX - _pixelPos.x;
        float dz = ToZ - _pixelPos.z;
        if (dx == 0 && dz == 0)
            return 0;
        moveData.Clear();
        moveData.MoveToX = (int)ToX;
        moveData.MoveToY = (int)ToZ;

        moveData.dx = dx;
        moveData.dz = dz;
        float PathLen = Mathf.Sqrt(dx * dx + dz * dz);
        float duration = PathLen / ((float)(moveData.MoveSpeed) * speedUp);
        moveData.delaX = dx / duration;
        moveData.delaY = dz / duration;

        _bIsMoving = true;
        //m_animator.SetFloat(m_runHash, 1);
        m_playerController.SetRoleStatus(E_NodeStatus.walk);
        return duration;
    }

    public virtual void MoveEndCallback()
    {
        moveData.Clear();
        _bIsMoving = false;
        m_playerController.SetRoleStatus(E_NodeStatus.Idle);
        //m_animator.SetFloat(m_runHash, 0);

        ActionPlayer.Instance.OnRoleMoveEnd(this);
    }

    public void ClearPath()
    {
        PathList.Clear();
    }

    public void Stop()
    {
        moveData.Clear();
        _bIsMoving = false;
        m_playerController.SetRoleStatus(E_NodeStatus.Idle);

        ClearPath();
    }

    public void SetVisible(bool b)
    {
        if (displayNode != null)
            displayNode.GetComponent<Renderer>().enabled = b;
    }

    public void SetParent(Transform parent)
    {
        displayNode.transform.SetParent(parent);
        displayNode.transform.localScale = new Vector3(1, 1, 1);//Vector3.one;//new Vector3(100, 100, 100);
        m_playerController.SetRoleStatus(E_NodeStatus.Idle);
    }

    public override void SetRoleStatus(E_NodeStatus state)
    {
        m_playerController.SetRoleStatus(state);
        if(state == E_NodeStatus.attack && (ID == 5002 || ID == 5003))
        {
            INode pNode = MoveBaseMaker.Instance.GetNode(1003);
            pNode.displayNode.transform.SetParent(displayNode.transform);
            pNode.displayNode.transform.localPosition = Vector3.zero;
            pNode.displayNode.transform.forward = GetDir();
        }
    }

    public override E_NodeStatus GetRoleStatus()
    {
        return m_playerController.GetCurStatus();
    }

    public override void NodeDead()
    {
        Stop();
        if (ID == 5002 || ID == 5003)
            displayNode.transform.gameObject.SetActive(false);
        else
            m_playerController.SetRoleStatus(E_NodeStatus.die);
    }
}
