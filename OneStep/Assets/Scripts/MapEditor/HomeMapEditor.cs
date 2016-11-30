using UnityEngine;
using System.Collections;

public class HomeMapEditor : MonoBehaviour {

    public bool m_isShowSetting = false ;

    public bool m_isAerialView = false ;

    private bool m_isEditRegion = false;

    public bool IsEditRegion
    {
        get { return m_isEditRegion; }
        set { m_isEditRegion = value; }
    }

    public bool m_isCloseed = false;

    // region temp string 
    private string m_regionX = "";

    public string RegionX
    {
        get { return m_regionX; }
        set { m_regionX = value; }
    }
    private string m_regionY = "";

    public string RegionY
    {
        get { return m_regionY; }
        set { m_regionY = value; }
    }
    private string m_regionWidth = "";

    public string RegionWidth
    {
        get { return m_regionWidth; }
        set { m_regionWidth = value; }
    }
    private string m_regionHeight = "";

    public string RegionHeight
    {
        get { return m_regionHeight; }
        set { m_regionHeight = value; }
    }
	// Use this for initialization


    private static HomeMapEditor m_instance = null;

    public static HomeMapEditor Instance
    {
        get { return m_instance; }
    }


    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
#if DEBUG_MODE
            //GameObject debugSys = GameObject.Instantiate(Resources.Load(m_gameDebugPath)) as GameObject;
		    //debugSys.transform.parent = this.gameObject.transform;
#endif
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
	void Start () 
    {
        InitMapEditor() ;        
	}
	
	// Update is called once per frame
	void Update () 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	}

    /// <summary>
    /// Initialize Map Editor 
    /// </summary>
    private void InitMapEditor()
    {
        //Initialize Camera
        InitCamera();
    }

    private void InitCamera()
    {
        CameraManager.getInstance().createCamera(CameraManager.CameraSort.PERSPECTIVE_CITYMAP);
    }

    void OnGUI()
    {
        
        GUI.Box(new Rect(0.0f, 0.0f, Screen.width, 40.0f), "");

        if (GUI.Button(new Rect(0, 10, 80, 20), "AerialView"))
        {
            //m_isShowSetting = !m_isShowSetting;
            m_isAerialView = !m_isAerialView;
            if (m_isAerialView)
            {
                CameraManager.getInstance().setCameraRotation(new Vector3(90.0f, 180.0f, 0.0f));
                //CameraManager.getInstance().setCameraAerialView(new Vector3(90.0f, 180.0f, 0.0f));
                //CameraManager.getInstance().setCameraAerialView(m_isAerialView);  88 76 33   
            }
            else
            {
                CameraManager.getInstance().setCameraRotation(new Vector3(19.8f, -135.0f, 0.0f));
                //CameraManager.getInstance().setCameraAerialView(m_isAerialView);
            }
        }
        if (GUI.Button(new Rect(90, 10, 80, 20), "EditRegion"))
        {
            m_isEditRegion = !m_isEditRegion;
        }
        if (GUI.Button(new Rect(180, 10, 80, 20), "SaveData"))
        {
            MapDataManager.getInstance().SaveMap();
        }
        if (GUI.Button(new Rect(270, 10, 20, 20), "X"))
        {
            m_isCloseed = true ;
            HomeMapManager.getInstance().setHomeEditActive(false);
            GameLaunch.Instance.ShowHomeEditor = false;
        }


        if (m_isShowSetting)
        {
            /*m_isShowFreedomRegion = */
            GUI.Toggle(new Rect(0, 40, 150, 20), /*m_isShowFreedomRegion*/false, " Freedom Region");

            /*m_isShowFixRegion = */
            GUI.Toggle(new Rect(0, 65, 150, 20), /*m_isShowFixRegion*/false, " Fix Region");

            /*m_isShowObstacle = */
            GUI.Toggle(new Rect(0, 90, 150, 20), /*m_isShowObstacle*/false, " Obstacle");

            /*m_isShowBattleRegion = */
            GUI.Toggle(new Rect(0, 115, 150, 20), /*m_isShowBattleRegion*/false, " Battle Region");

            //m_isAerialView = GUI.Toggle(new Rect(0, 140, 150, 20), m_isAerialView, " AerialView");
        }

        

        if(m_isEditRegion)
        {
            GUI.Box(new Rect(90.0f, 50.0f, 120, 140.0f), "");
            GUI.Label(new Rect(100, 55, 50, 20), "X:" );
            m_regionX = GUI.TextField(new Rect(150, 55, 50, 20), m_regionX );
            GUI.Label(new Rect(100, 80, 50, 20), "Y:");
            m_regionY = GUI.TextField(new Rect(150, 80, 50, 20), m_regionY );
            GUI.Label(new Rect(100, 105, 50, 20), "Width:");
            m_regionWidth = GUI.TextField(new Rect(150, 105, 50, 20), m_regionWidth );
            GUI.Label(new Rect(100, 130, 50, 20), "Height:");
            m_regionHeight = GUI.TextField(new Rect(150, 130, 50, 20), m_regionHeight );

            if (GUI.Button(new Rect(95, 160, 60, 20), "Refresh"))
            {
                GridControllor.Instance.SetGridPosition(float.Parse(m_regionX), float.Parse(m_regionY), float.Parse(m_regionWidth), float.Parse(m_regionHeight));
            }
            if (GUI.Button(new Rect(160, 160, 40, 20), "Add"))
            {
                //HomeMapManager.getInstance().CreateRegion("1", m_regionX, m_regionY, m_regionWidth, m_regionHeight);
                //MapDataManager.getInstance().InsertRegionXML("1", "HomeRegion", m_regionX, m_regionY, m_regionWidth, m_regionHeight);
                //MapDataManager.getInstance().UpdateRegionXML("1", "HomeRegion-", m_regionX, m_regionY, m_regionWidth, m_regionHeight);
                MapDataManager.getInstance().SaveJsonData();
            }
        }

        
    }
}
