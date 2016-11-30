using System.Collections.Generic;
using UnityEngine;

public class MoveBaseMaker
{
    private GameObject _originalGreen;
    private GameObject _originalRed;
    private GameObject _originalWhite;
    // Use this for initialization
    private static MoveBaseMaker _instance = null;

    private Dictionary<int, GameObject> _greenBaseArray;
    private Dictionary<int, GameObject> _redBaseArray;

    private float _gridWidth;
    private float _gridHeight;
    private float _totalHeight;

    private GameObject _buildingBase;
    private GameObject _progressBar;
    private GameObject _roadNode;
    private GameObject _wallNode;
    private GameObject _roadCornerNode;
    private GameObject _bulletNode;
    private GameObject _soldierNode;
    private GameObject _workerNode;
    private GameObject _heroNode;
    private GameObject _roleShadowNode;
    private GameObject _mapResources;
    private GameObject _imageBtn;
    private GameObject _bullet;
    private GameObject _buildingResTxt;
    private GameObject _fillArea;
    private Dictionary<int, GameObject> _createAnimatorArray;
    private GameObject _createAnimator;

    private GameObject _buildingDestroyEffect;
    private Dictionary<int, GameObject> _bulletArray;

    private MoveBaseMaker()
    {
        _bulletArray = new Dictionary<int, GameObject>();
    }

    public static MoveBaseMaker Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MoveBaseMaker();
            }
            return _instance;
        }
    }

    public GameObject GetBuildingBase()
    {
        if (_buildingBase == null)
            _buildingBase = (GameObject)Resources.Load<GameObject>("Prefabs/Nodes/base/NE_base_1");
        return GameObject.Instantiate(_buildingBase, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetBuildingDestroy()
    {
        if (_buildingDestroyEffect == null)
            _buildingDestroyEffect = (GameObject)Resources.Load<GameObject>("Prefabs/poshui");
        return GameObject.Instantiate(_buildingDestroyEffect, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetBomm()
    {
        //if (_buildingDestroyEffect == null)
        GameObject buildingDestroyEffect = (GameObject)Resources.Load<GameObject>("Prefabs/Bomm");
        return GameObject.Instantiate(buildingDestroyEffect, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetProgress()
    {
        if(_progressBar == null)
            _progressBar = (GameObject)Resources.Load<GameObject>("Prefabs/Map/ProgressBar");
        return GameObject.Instantiate(_progressBar, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetRoadNode()
    {
        if (_roadNode == null)
            _roadNode = (GameObject)Resources.Load<GameObject>(/*"Prefabs/Map/Road"*/"Prefabs/Nodes/building/SoilRoad");
        return GameObject.Instantiate(_roadNode, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetWallNode()
    {
        if (_wallNode == null)
            _wallNode = (GameObject)Resources.Load<GameObject>("Prefabs/Nodes/building/WallNode");
        return GameObject.Instantiate(_wallNode, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetRoadCornerNode()
    {
        if (_roadCornerNode == null)
            _roadCornerNode = (GameObject)Resources.Load<GameObject>(/*"Prefabs/Map/Road"*/"Prefabs/Nodes/building/SoilRoad");
        return GameObject.Instantiate(_roadCornerNode, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetBulletNode(ushort nodeId)
    {
        if(_bulletArray.ContainsKey(nodeId))
            return GameObject.Instantiate(_bulletArray[nodeId], Vector3.zero, Quaternion.identity) as GameObject;
        else
        {
            GameObject pNode = (GameObject)Resources.Load<GameObject>("Prefabs/Nodes/" + nodeId.ToString());
            _bulletArray.Add(nodeId, pNode);
            return GameObject.Instantiate(pNode, Vector3.zero, Quaternion.identity) as GameObject;
        }
        
    }

    public GameObject GetSoldierNode()
    {
        if (_soldierNode == null)
            _soldierNode = (GameObject)Resources.Load<GameObject>(/*"Prefabs/Animation/soldier_1"*/"Prefabs/Nodes/role/Weibing_0");
        return GameObject.Instantiate(_soldierNode, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetWorkerNode()
    {
        if (_workerNode == null)
            _workerNode = (GameObject)Resources.Load<GameObject>(/*"Prefabs/Animation/nongmin_1"*/"Prefabs/Nodes/role/Woodcutter_0");
        return GameObject.Instantiate(_workerNode, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetHeroNode()
    {
        if (_heroNode == null)
            _heroNode = (GameObject)Resources.Load<GameObject>(/*"Prefabs/Animation/hero_1"*/"Prefabs/Hero_ancher_0");
        return GameObject.Instantiate(_heroNode, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetRoleShadow()
    {
        if (_roleShadowNode == null)
            _roleShadowNode = (GameObject)Resources.Load<GameObject>(/*"Prefabs/Animation/hero_1"*/"Prefabs/Nodes/role/Roleshadow");
        return GameObject.Instantiate(_roleShadowNode, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetArrowTip(int type)
    {
        if(_mapResources == null)
            _mapResources = (GameObject)Resources.Load<GameObject>("Prefabs/Map/MapResources");
        GameObject go = _mapResources.transform.FindChild("ArrowTip" + type.ToString()).gameObject;
        return go;
    }

    public GameObject GetImageBtn()
    {
        if (_imageBtn == null)
            _imageBtn = (GameObject)Resources.Load<GameObject>("Prefabs/Map/BasicButton");
        return GameObject.Instantiate(_imageBtn, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetInVailedFlag()
    {
        if (_mapResources == null)
            _mapResources = (GameObject)Resources.Load<GameObject>("Prefabs/Map/MapResources");
        GameObject go = _mapResources.transform.FindChild("InvalidFlag").gameObject;
        return GameObject.Instantiate(go, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetBullet()
    {
        if (_bullet == null)
            _bullet = (GameObject)Resources.Load<GameObject>("Prefabs/Map/Bullet1");
        return GameObject.Instantiate(_bullet, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetTestBuildingResText()
    {
        if (_buildingResTxt == null)
            _buildingResTxt = (GameObject)Resources.Load<GameObject>("Prefabs/Map/TestBuildingResText");
        return GameObject.Instantiate(_buildingResTxt, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetFillArea()
    {
        if (_fillArea == null)
            _fillArea = (GameObject)Resources.Load<GameObject>("Prefabs/Map/FillArea");
        return GameObject.Instantiate(_fillArea, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetCreateAnimator(int size)
    {
        if (_createAnimatorArray == null)
            _createAnimatorArray = new Dictionary<int, GameObject>();

        if (!_createAnimatorArray.ContainsKey(size))
        {
            _createAnimator = (GameObject)Resources.Load<GameObject>("Prefabs/Nodes/role/Building_" + size.ToString());
            _createAnimatorArray.Add(size, _createAnimator);
        }
        else
            _createAnimator = _createAnimatorArray[size];

        return GameObject.Instantiate(_createAnimator, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject GetEllipseView()
    {
        if (_mapResources == null)
            _mapResources = (GameObject)Resources.Load<GameObject>("Prefabs/Map/MapResources");
        GameObject go = _mapResources.transform.FindChild("EllipseView").gameObject;
        return go;
    }

    public INode GetNode(ushort nodeId)
    {
        INode pNode = null;
        if (nodeId == 1001)
            pNode = new ProjectileNode(nodeId);
        else if (nodeId == 1002)
            pNode = new ExplosionNode(nodeId);
        else if (nodeId == 1003)
            pNode = new ActionNode(nodeId);
        return pNode;
    }
}