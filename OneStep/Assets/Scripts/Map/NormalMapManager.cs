using UnityEngine;
using System.Collections;

public class NormalMapManager : BaseSingleton<NormalMapManager> {

    private GameObject m_NormalMap = null;

    NormalMapManager()
    {

    }

    ~NormalMapManager()
    {

    }

    public void CreateNormalMap()
    {
        if(m_NormalMap==null)
        {
            m_NormalMap = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Map/NormalMap"), Vector3.zero, Quaternion.identity) as GameObject;
            m_NormalMap.name = "NormalMap";
            //m_NormalMap.SetActive(true);
        }
    }
}
