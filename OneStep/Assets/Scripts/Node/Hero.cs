using UnityEngine;
using System.Collections;

public class Hero : RoleNode
{
    public Hero(ushort heroIndexId, int indexId,GameObject obj = null)
    {
        this.indexId = heroIndexId;
        buildingIndexId = indexId;
        if (obj == null)
            displayNode = MoveBaseMaker.Instance.GetHeroNode();
        else
            displayNode = obj;
        initNode();
    }
}
