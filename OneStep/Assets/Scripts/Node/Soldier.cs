using UnityEngine;
using System.Collections;

public class Soldier : RoleNode
{
    public Soldier(ushort baseId,int indexId)
    {
        this.indexId = indexId;
        ID = baseId;
        displayNode = MoveBaseMaker.Instance.GetHeroNode();
        initNode();
    }
}
