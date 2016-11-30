using UnityEngine;
using System.Collections;

public class GameLaunch : MonoBehaviour {


    const string m_gameDebugPath = "Game/DebugSys";

    [SerializeField]
    private bool m_bIsShowHomeEditor = false;

    public bool ShowHomeEditor
    {
        get { return m_bIsShowHomeEditor; }
        set { m_bIsShowHomeEditor = value; }
    }

    private static GameLaunch m_instance = null;

    public static GameLaunch Instance
    {
        get { return m_instance; }
    }

    void Awake()
    {
        if(m_instance == null )
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
    // Use this for initialization
    void Start () 
    {
        // 创建摄像机控制器
        CameraManager.getInstance().createCamera(CameraManager.CameraSort.PERSPECTIVE_CITYMAP);
        
        // 创建NormalMap
        NormalMapManager.getInstance().CreateNormalMap();

        // 创建家园地图编辑器
        HomeMapManager.getInstance().createHomeEditor();

        //CameraManager.getInstance().setCameraRotation(new Vector3(90.0f, 180.0f, 0.0f));

	}


    private void InitCamera()
    {
        CameraManager.getInstance().createCamera(CameraManager.CameraSort.PERSPECTIVE_CITYMAP);
    }

    private void InitHomeEditor()
    {
        HomeMapManager.getInstance().createHomeEditor();
    }

	// Update is called once per frame
	void Update () {
	
	}



    void OnGUI()
    {
        if(!m_bIsShowHomeEditor)
        {
            GUI.Box(new Rect(0.0f, 0.0f, Screen.width, 40.0f), "");
            if (GUI.Button(new Rect(5, 10, 90, 20), "HomeEditot"))
            {
                m_bIsShowHomeEditor = !m_bIsShowHomeEditor;
                HomeMapManager.getInstance().setHomeEditActive(m_bIsShowHomeEditor);
            }
        }
        
    }
}
