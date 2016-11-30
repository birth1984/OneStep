using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using IGG.CCTwo.Data;

public class NormalMap : BaseMap {
    public LayerMask mouseHitLayer;
    public Camera mCamera = null;

    public List<GameObject> m_selfHeroList;
    public List<GameObject> m_enemyHeroList;
    public List<GameObject> m_enemyBuildingList;

    private Transform m_transform;
    private PlayerController m_playerController;
    private List<Hero> m_heroList;
    private Hero m_selfHero;
    private Hero m_enemyHero;
    private bool _bClickGui = false;
    private int m_indexId;

    private Vector3 _preMousePos;
    private Vector3 _preMouseDownPos;

    void Awake()
    {
        if (mCamera == null)
            mCamera = CameraManager.getInstance().GetBgCamera();
    }
    
    // Use this for initialization
    override public void Start()
    {
        m_heroList = new List<Hero>();
        m_transform = this.transform;
        m_indexId = 0;
        for (int i = 0;i < m_selfHeroList.Count;i++)
        {
            Hero selfHero = new Hero(1, 1, m_selfHeroList[i]);
            selfHero.SetParent(m_transform);
            selfHero.SetPosition(m_selfHeroList[i].transform.localPosition);
            m_heroList.Add(selfHero);
            selfHero.Camp = 0;
            selfHero.indexId = i;

            CBaseObject pObject = new CBaseObject(selfHero.indexId);
            pObject.Camp = 0;
            pObject.indexId = selfHero.indexId;
            pObject.SetPosition(m_selfHeroList[i].transform.localPosition);
            pObject.SetNode(selfHero);
            BattleSceneProcessor.Instance.m_attackerHeroList.Add(pObject);
        }

        ushort baseId = 0;
        float attackRange = 0;
        float attackSpeed = 1.5f;
        ushort moveSpeed = 10;
        short hp = 0;
        for (int i = 0; i < m_enemyHeroList.Count; i++)
        {
            if (m_enemyHeroList[i].tag.CompareTo("Soldier_0") == 0)
            {
                baseId = 5001;
                attackRange = 10;
                attackSpeed = 2.0f;
                moveSpeed = 10;
                hp = 0;
            }
            else if (m_enemyHeroList[i].tag.CompareTo("Soldier_1") == 0)
            {
                baseId = 5002;
                attackRange = 2;
                attackSpeed = 2.5f;
                moveSpeed = 5;
                hp = -500;
            }
            else if (m_enemyHeroList[i].tag.CompareTo("Soldier_2") == 0)
            {
                baseId = 5003;
                attackRange = 1.5f;
                attackSpeed = 1.5f;
                moveSpeed = 7;
                hp = -300;
            }
            Hero enemyHero = new Hero(baseId, 1, m_enemyHeroList[i]);
            enemyHero.ID = baseId;
            enemyHero.SetParent(m_transform);
            enemyHero.SetPosition(m_enemyHeroList[i].transform.localPosition);
            m_heroList.Add(enemyHero);
            enemyHero.Camp = 1;
            enemyHero.indexId = 1000 + i;
            enemyHero.AttackRange = attackRange;
            enemyHero.moveData.MoveSpeed = moveSpeed;
            enemyHero.SetCurrentHP(hp);
            enemyHero.RegressPosition = m_enemyHeroList[i].transform.localPosition;

            CBaseObject pObject = new CBaseObject(enemyHero.indexId);
            pObject.Camp = 1;
            pObject.indexId = enemyHero.indexId;
            pObject.SetPosition(m_enemyHeroList[i].transform.localPosition);
            pObject.SetNode(enemyHero);
            pObject.AttackRange = attackRange;
            pObject.AttackSpeed = attackSpeed;
            pObject.SetCurrentHP(hp);
            BattleSceneProcessor.Instance.m_defenderHeroList.Add(pObject);
        }

        for (int i = 0; i < m_enemyBuildingList.Count; i++)
        {
            BuildingNode pBuild = new BuildingNode(1,1,m_enemyBuildingList[i]);
            pBuild.SetPosition(m_enemyBuildingList[i].transform.localPosition);
            pBuild.Camp = 1;
            pBuild.indexId = 2000 + i;

            CBaseObject pObject = new CBaseObject(1);
            pObject.indexId = pBuild.indexId;
            pObject.Camp = 1;
            pObject.SetPosition(m_enemyBuildingList[i].transform.localPosition);
            pObject.SetNode(pBuild);
            BattleSceneProcessor.Instance.m_defenderBuildingList.Add(pObject);
        }

        GameData.Instance.actConfig.GetActionSequence(1);
        base.Start();
        MapManager.Instance.CurrentMap = this;

        BattleSceneProcessor.Instance.BattleStart();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseDownHandle();
        }
        if (Input.GetMouseButton(0))
        {
            DragHandle();
        }
        if (Input.GetMouseButtonUp(0))
        {
            MouseUpHandle();
        }

        if (m_heroList == null)
            return;
        foreach (Hero pHero in m_heroList)
        {
            moveNode(pHero);
        }

        TimeListenerManager.Instance.Update();
        BattleSceneProcessor.Instance.BattleRun();
    }

    public GameObject GetMapObject()
    {
        return m_transform.gameObject;
    }

    private void moveNode(RoleNode pNode)
    {
        //屏幕外的要隐藏.
        //pNode.SetVisible((IsInScene(pNode)));
        MoveData aMoveData = pNode.moveData;
        if (aMoveData.MoveDelay > 0.0f)
        {
            aMoveData.MoveDelay -= Time.deltaTime;
            if (aMoveData.MoveDelay <= 0.0f)
            {
                pNode.MoveEndCallback();
            }
        }
        else if (pNode.IsMoving())
        {
            float dx = aMoveData.delaX * Time.deltaTime;
            float dz = aMoveData.delaY * Time.deltaTime;
            if (Mathf.Abs(dx) >= Mathf.Abs(aMoveData.dx))
            {
                dx = aMoveData.dx;
                aMoveData.dx = 0.0f;
            }
            else
            {
                aMoveData.dx -= dx;
            }

            if (Mathf.Abs(dz) >= Mathf.Abs(aMoveData.dz))
            {
                dz = aMoveData.dz;
                aMoveData.dz = 0.0f;
            }
            else
            {
                aMoveData.dz -= dz;
            }
            Vector3 curPos = pNode.GetPosition();
            curPos.x += dx;
            curPos.z += dz;
            pNode.SetPosition(curPos);
           
            if (aMoveData.dx == 0.0f && aMoveData.dz == 0.0f)
            {
                pNode.MoveEndCallback();
            }
        }
    }

    void OnGUI()
    {
        
    }
    //test
    private Vector3 m_endPos,m_startPos;
    void OnDrawGizmos()
    {
//         if(m_endPos.x != 0 && m_startPos.x != 0)
//             Gizmos.DrawLine(Input.mousePosition, m_endPos);
    }

    public void MouseDownHandle()
    {
        if (GUIUtility.hotControl != 0)
        {
            _bClickGui = true;
            return;
        }
        if (GameLaunch.Instance && GameLaunch.Instance.ShowHomeEditor)
            return;
        /*Vector3 clickPos = converPosition2Local(Input.mousePosition);
        _preMousePos = Input.mousePosition;
        _preMouseDownPos = _preMousePos;
        //Debug Info
        Vector3 cellPos = MapManager.Instance.GetCellPosition(clickPos);
        clickPos = MapManager.Instance.GetPixelPosition(cellPos);
        Debug.Log("Cell Position:(" + (int)cellPos.x + ", " + (int)cellPos.y + ") ---------- Pixel Position:" + clickPos);*/

        m_startPos = Input.mousePosition;
        Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100, mouseHitLayer);
        if (hit.transform != null && hit.transform.name.CompareTo("NormalMap") == 0)
        {
            Hero selfHero = new Hero(1, 1);
            m_heroList.Add(selfHero);
            selfHero.Camp = 0;
            selfHero.indexId = m_indexId++;
            Vector3 pos = m_endPos = hit.point;
            AddNode(selfHero, pos);

            CBaseObject pObject = new CBaseObject(selfHero.indexId);
            pObject.Camp = 0;
            pObject.indexId = selfHero.indexId;
            pObject.SetPosition(pos);
            pObject.SetNode(selfHero);
            BattleSceneProcessor.Instance.m_attackerHeroList.Add(pObject);
        }


        /*if (!MapManager.Instance.IsSelfMap())
            return;
        BuildingNode temp = IsSelectedBuilding((int)cellPos.x, (int)cellPos.y) as BuildingNode;

        if (_selectNode != null && temp != null && temp == _selectNode)
        {
            if (_selectNode.buildInfo.Type == BuildingType.BT_Function && _selectNode.buildInfo.SubType == BuildingSort.BS_CityCenter / *||
                _selectNode.buildInfo.Type == BuildingType.BT_COLLECTION* /)
                return;
            MapManager.Instance.IsDragingNode = true;

            if (_selectNode.GridState != Building.GRID_STATE_RED)
            {
                _selectNode.SetGridState(Building.GRID_STATE_GREEN);
                _selectNode.HideSelectEffect();
                GameUIRoot.Instance.GetMainFrame().ControlBuildPanelUI.Close();
            }
        }*/
    }

    public void MouseUpHandle()
    {
        if (_bClickGui)
        {
            _bClickGui = false;
            return;
        }
        m_startPos = m_endPos = Vector3.zero;
        MapManager.Instance.IsDragingNode = false;
    }

    public void DragHandle()
    {
        if (_bClickGui)
            return;
        if (!MapManager.Instance.IsDragingNode)
            return;
    }

    public void AddNode(INode pNode,ushort posX,ushort posY)
    {

    }

    public void AddNode(INode pNode, Vector3 pos)
    {
        pNode.displayNode.transform.SetParent(m_transform);
        pNode.SetPosition(pos);
    }

    private Vector3 converPosition2Local(Vector3 pos)
    {
        Vector3 outPos = mCamera.ScreenToWorldPoint(pos);
        outPos = transform.InverseTransformPoint(outPos);
        return outPos;
    }

    /**
		 * 是否选中建筑物 
		 */
    /*public BuildingNode IsSelectedBuilding(int logicX, int logicY)
    {
        if (_bCreatingFlag)
        {
            if (_tempNode == null)
                return null;
            int maxX = _tempNode.CellX + _tempNode.baseWidth;
            int minX = _tempNode.CellX;
            int maxY = _tempNode.CellY + _tempNode.baseHeight;
            int minY = _tempNode.CellY;
            if (logicX >= minX && logicX < maxX && logicY >= minY && logicY < maxY)
            {
                return _tempNode;
            }
        }
        else
        {
            if (_buildingGroup != null && _buildingGroup.IsSelect(logicX, logicY))
                return _buildingGroup;
            if (_bIsMovingBuilding)
            {
                int maxX = _selectNode.CellX + _selectNode.baseWidth;
                int minX = _selectNode.CellX;
                int maxY = _selectNode.CellY + _selectNode.baseHeight;
                int minY = _selectNode.CellY;
                if (logicX >= minX && logicX < maxX && logicY >= minY && logicY < maxY)
                {
                    return _selectNode;
                }
                else
                    return null;
            }
            return MapManager.Instance.GetBuildingInCell(logicX, logicY, true);
        }
        return null;
    }*/
#if UNITY_EDITOR 
    private int m_currentNodeId;
    private int m_currentCamp;
    public void UpdateNodeId(int id,int camp)
    {
        m_currentNodeId = id;
        m_currentCamp = camp;
    }

    public void ClearNode()
    {

    }

    private void AddNode()
    {
        if (m_currentNodeId < 1)
            return;
        
        if(m_currentNodeId > 100)//Hero
        {
            
        }
        else//Soldier
        {

        }

        if(m_currentCamp == 0)//Enemy
        {

        }
        else//Self
        {

        }

        
    }
#endif 
}

