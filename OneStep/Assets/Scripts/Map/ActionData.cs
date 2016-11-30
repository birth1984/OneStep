using UnityEngine;
using System.Collections;

public enum E_ActionType
{
    AT_Normal,
    AT_Skill,
}

public class ActionData
{
    public BaseNode ObjNode;
    public BaseNode SrcNode;
    public int HurtValue;
    public int RobResouce;
    public Vector3 ObjPoint;
    public Vector3 AttackDir;
    public bool RemoveSrcNode;
    public byte AdvancedAction;
    public E_ActionType Type;

    public int NeedCallback;
    public int HurtType;
    public ActionData NextData;

    public ActionData()
    {
        HurtType = 1;
        Type = E_ActionType.AT_Normal;
        RemoveSrcNode = false;
        NeedCallback = -1;
        NextData = null;
        AdvancedAction = 0;
    }
};