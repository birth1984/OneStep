using UnityEngine;
using System.Collections;

public class BuildingNode : BaseNode
{
    //private TimeListenerHandler _timeHandle;

    public BuildingNode(ushort heroIndexId, int indexId, GameObject obj = null)
    {
        m_hitPoint = new Vector3(0, 1.5f, 0);
        
        if (obj == null)
            displayNode = MoveBaseMaker.Instance.GetHeroNode();
        else
            displayNode = obj;
        //_timeHandle = new TimeListenerHandler(buildingDestroyHandle);
    }

    public override void NodeDead()
    {
        Vector3 pos = displayNode.transform.localPosition;
        GameObject destroyAnim = MoveBaseMaker.Instance.GetBuildingDestroy();
        destroyAnim.transform.localRotation = Quaternion.Euler(0, 180, 0);
        displayNode.SetActive(false);
        destroyAnim.transform.SetParent((MapManager.Instance.CurrentMap as NormalMap).transform);
        destroyAnim.transform.localPosition = new Vector3(pos.x, pos.y, pos.z);

        pos = displayNode.transform.localPosition;
        GameObject bomm = MoveBaseMaker.Instance.GetBomm();
        bomm.transform.SetParent((MapManager.Instance.CurrentMap as NormalMap).transform);
        bomm.transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
    }

    private void buildingDestroyHandle(float dt)
    {
        
    }
}
