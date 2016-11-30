using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum E_NodeDir
{
    ND_DOWN,
    ND_LEFT_DOWN,
    ND_LEFT,
    ND_LEFT_UP,
    ND_UP,
    ND_RIGHT_UP,
    ND_RIGHT,
    ND_RIGHT_DOWN
};

/*enum E_ActionType
{
    AT_Stand = 0,
    AT_Walk,
    AT_Jump,
    AT_Attack,
    AT_Dead,
};*/

public class ActionPlayer : MonoBehaviour
{
    static ActionPlayer m_instance = null;
    const float aAngle = 22.5f / 180.0f * Mathf.PI;

    private Dictionary<INode, ActionData> ActionList;
    private Dictionary<INode, ActionData> ActionListTemp;

    void Awake()
    {
        m_instance = this;
        ActionList = new Dictionary<INode, ActionData>();
        ActionListTemp = new Dictionary<INode, ActionData>();
    }

    public static ActionPlayer Instance
    {
        get
        {
            return m_instance;
        }
    }

    public void PlayStatus(BaseNode pNode, E_NodeStatus StatusId, bool Forever = true)
    {
        pNode.SetRoleStatus(StatusId);
        //pNode.SetDir(Dir);
    }

    public void OnRoleMoveEnd(RoleNode pNode)
    {
        if (pNode.PathList.Count > 1)
        {
            int NewX = pNode.PathList[0];
            int NewZ = pNode.PathList[1];
            pNode.PathList.RemoveRange(0, 2);
            MoveTo(pNode, new Vector3(NewX, 0, NewZ));
        }
        else
        {
            PlayStatus(pNode, E_NodeStatus.Idle);
        }
    }

    public void AddMovePoint(RoleNode pNode, Vector3 pos, float speedUp = 1.0f)
    {
        if (speedUp != 0)
            pNode.moveData.SpeedUp = speedUp;

        if (pNode.GetRoleStatus() == E_NodeStatus.Idle && pNode.moveData.MoveDelay <= 0.0f)
        {
            MoveTo(pNode, pos);
        }
        else
        {
            if(pNode.IsMoving())
            {
                pNode.PathList.Add((ushort)pos.x);
                pNode.PathList.Add((ushort)pos.z);
            }
        }
    }

    public void MoveTo(RoleNode MoveNode, Vector3 MoveTo)
    {
        Vector3 curPos = MoveNode.GetPosition();
        MoveTo.x = Mathf.Abs(MoveTo.x);
        MoveTo.z = Mathf.Abs(MoveTo.z);
        if (curPos.x != MoveTo.x || curPos.z != MoveTo.z)
        {
            Vector3 targetDir = MoveTo - curPos;//目标点的朝向 Vector3 tempDir=Vector3.Cross(transform.forward,targetDir.normalized);
            //float dotValue = Vector3.Dot(MoveNode.displayNode.transform.forward, targetDir.normalized);
            //float angle = Mathf.Acos(dotValue) * Mathf.Rad2Deg;//计算夹角
            //MoveNode.displayNode.transform.RotateAround(MoveNode.displayNode.transform.position, Vector3.up, angle);//旋转角色
            MoveNode.SetDir(targetDir);
            float su = MoveNode.moveData.SpeedUp > 4.0f ? 4.0f : MoveNode.moveData.SpeedUp;

            float duration = MoveNode.MoveTo(MoveTo.x, MoveTo.z, 1.0f);
            if (duration <= 0)
                MoveNode.MoveEndCallback();
        }
    }

    void Update()
    {
        if(ActionList.Count > 0)
        {
            var itor = ActionList.GetEnumerator();
            while (itor.MoveNext())
            {
                ActionData aData = itor.Current.Value;
                if (aData.SrcNode.IsEmissionWatting)
                {
                    ActionProcess(aData);
                    ActionListTemp.Add(itor.Current.Key, itor.Current.Value);
                    aData.SrcNode.IsEmissionWatting = false;
                }
            }            var itorTemp = ActionListTemp.GetEnumerator();            while (itorTemp.MoveNext())
            {
                ActionList.Remove(itorTemp.Current.Key);
            }            ActionListTemp.Clear();        }
    }

    private void ActionProcess(ActionData aData)
    {
        while (aData != null)
        {
            ActionData NextData = aData.NextData;
            if (aData.Type == E_ActionType.AT_Skill)
            {

                int SkillId = aData.HurtValue; 
                PlaySkillAction(aData.SrcNode, SkillId, aData);
            }
            else
            {
                ActionListController.Instance.PlayNodeAction(aData.SrcNode, aData);
            }
            aData = NextData;
        }
    }

    private void DoRemainingAction(INode aNode)
    {
        if(ActionList.ContainsKey(aNode))
        {
            ActionData aData = ActionList[aNode];
            ActionProcess(aData);
            ActionList.Remove(aNode);
        }
        /*map<INode*, ActionData*>::iterator itor = ActionList.find(aNode);
        if (itor == ActionList.end()) return;
        ActionData* aData = itor->second;
        if (_CurItor == itor)
            _CurItor++;
        ActionList.erase(itor);
        ActionProcess(aNode, aData);*/

    }

    public void Attack(INode pSrcNode, INode pObjNode, Vector3 objPoint, int damageValue)
    {
        ActionData aData = new ActionData();
        aData.SrcNode = pSrcNode as BaseNode;
        aData.ObjNode = pObjNode as BaseNode;
        aData.AttackDir = (pSrcNode as BaseNode).GetDir();
        aData.HurtType = damageValue;
        aData.ObjPoint = pObjNode.GetPosition();

        if(aData.Type == E_ActionType.AT_Skill)
            PlayStatus(aData.SrcNode, E_NodeStatus.skill);
        else
            PlayStatus(aData.SrcNode, E_NodeStatus.attack);

        aData.AttackDir = (pObjNode as BaseNode).displayNode.transform.localPosition - (pSrcNode as BaseNode).displayNode.transform.localPosition;
        (pSrcNode as BaseNode).SetDir(aData.AttackDir);

        AddToActionList(pSrcNode as BaseNode, aData);
    }

    public void AddToActionList(BaseNode pNode, ActionData aData)
    {
        if(ActionList.ContainsKey(pNode))
        {
            ActionData aFirstData = ActionList[pNode];
            while (aFirstData.NextData != null)
            {
                aFirstData = aFirstData.NextData;
            }
            aFirstData.NextData = aData;
            return;
        }
        ActionList.Add(pNode, aData);
    }

    public void NodeDead(BaseNode pNode,bool isDoingAction = true)
    {
        if(isDoingAction)
        {
            DoRemainingAction(pNode);
            //复活或加血后，不死
            if (pNode.GetCurrentHP() > 0)
                return;
        }
        pNode.NodeDead();
        Debug.Log("----------------------> DeadNode Id = " + pNode.indexId);
    }

    public void PlaySkillAction(BaseNode pNode, int skillId, ActionData aData)
    {

    }

//     public void PlayNodeAction(BaseNode pNode, ActionData aData)
//     {
//         ProjectileNode node = new ProjectileNode(pNode, aData);
//         StartCoroutine(node.Shoot());
//    
    public void ActionEndCallback(INode pNode, ActionData aData)
    {
        if (aData.ObjNode is RoleNode)
        {
            (aData.ObjNode as RoleNode).SetRoleStatus(E_NodeStatus.hit);
        }
        ModifyHP(aData.ObjNode, aData.SrcNode.AttackValue);
    }

    public void ModifyHP(BaseNode pNode, int hurtValue)
    {
        //Update HP UI And Role Action
        pNode.SetCurrentHP(hurtValue);
        if (pNode.GetCurrentHP() <= 0)
            NodeDead(pNode);
    }
}
