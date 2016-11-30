using UnityEngine;
using System.Collections;
using System;

public class CameraManager : BaseSingleton<CameraManager>
{
    public enum CameraSort
    {
        ORTHOGRAPHIC_WORLDMAP,
        ORTHOGRAPHIC_CITYMAP,
        ORTHOGRAPHIC_CITYMAP_LIMIT,
        PERSPECTIVE_CITYMAP,
        PERSPECTIVE_CITYMAP_LIMIT,
        CAMPAIGN,
    }

    public struct AnimatorState
    {
        public int idle ;
        public int animation ;
        public int fighting ;
        public int moving ;
        public int crash ;
        public int UIVictory_crash ;
    }

    private AnimatorState m_animatorState;

    public AnimatorState AnimatorStateHashID
    {
        get { return m_animatorState; }
    }


    private GameObject m_cameraRoot;
    public GameObject CameraRoot
    {
        get { return m_cameraRoot; }
    }


    private CameraSort m_cameraType;

    public CameraSort CameraType
    {
        get { return m_cameraType; }
    }

    private CameraControl m_controllor;

    private Camera m_backgroundCamera;

    private Camera m_middleCamera;

    private Camera m_foregroundCamera;

    private Camera m_effectCamera;

    
    /* Constructor */
    CameraManager()
    {
        initAnimatorStateHashID();
    }

    /* Destructor */
    ~CameraManager()
    {

    }

    private void initAnimatorStateHashID()
    {
        m_animatorState.idle            = Animator.StringToHash("Idle");
        m_animatorState.animation       = Animator.StringToHash("Animation");
        m_animatorState.fighting        = Animator.StringToHash("Fighting");     
        m_animatorState.moving          = Animator.StringToHash("Moving");
        m_animatorState.UIVictory_crash = Animator.StringToHash("UIVictory_crash");
    }

    // Initialize Camera Object & Controller
    public void createCamera(CameraSort type)
    {
        if (m_cameraRoot == null)
        {
            if (!Camera.main)
            {
                m_cameraRoot = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/CameraControllor"), Vector3.zero, Quaternion.identity) as GameObject;
                //m_cameraRoot = UnityEngine.Object.Instantiate(AssetBundleLoader.getInstance().loadPrefabSync("Prefab/CameraControllor"), Vector3.zero, Quaternion.identity) as GameObject;
                m_cameraRoot.name = "CameraControllor";
            }
            else
            {
                if (Camera.main.transform.parent.gameObject.name != "CameraControllor")
                    Debug.LogError("Main Camera 已被CameraController佔用!");
                m_cameraRoot = Camera.main.transform.parent.gameObject;
            }
        }

        initCameraSetting(type);
    }

    private void initCameraSetting(CameraSort type)
    {
        //EfficacyDebuger.logConsole("initCameraSetting");

        m_cameraType = type;

        m_backgroundCamera  = m_cameraRoot.transform.GetChild(0).GetComponent<Camera>();

        m_middleCamera = m_cameraRoot.transform.GetChild(1).GetComponent<Camera>();

        m_foregroundCamera = m_cameraRoot.transform.GetChild(2).GetComponent<Camera>();

        m_effectCamera = m_cameraRoot.transform.GetChild(3).GetComponent<Camera>();

        //if (m_fgCamera == null || m_bgCamera == null || m_middleCamera == null || m_effectCamera == null)
        //    EfficacyDebuger.logError("m_fgCamera and m_bgCamera Null!");

        m_controllor = m_cameraRoot.GetComponent<CameraControl>();

        //initRegister();

        switch (m_cameraType)
        {
            //case CameraSort.ORTHOGRAPHIC_WORLDMAP:
            //    m_fgCamera.clearFlags = CameraClearFlags.Depth;
            //    m_fgCamera.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.02f);
            //    m_fgCamera.cullingMask = 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_foregroundLayerName) | 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_TransportUnitLayerName);
            //    m_fgCamera.depth = 2;
            //    m_fgCamera.fieldOfView = 30f;
            //    m_fgCamera.nearClipPlane = -100.0f;
            //    m_fgCamera.farClipPlane = 1000.0f;

            //    m_controllor.CameraMode = CameraControl.CameraModeType.ORTHOGRAPHIC;
            //    m_fgCamera.gameObject.SetActive(true);
            //    //m_bgCamera.gameObject.SetActive(true);
            //    //m_middleCamera.gameObject.SetActive(false);

            //    m_controllor.StartPosition = new Vector3(275, 66, 185);
            //    m_controllor.StartRotation = new Vector3(45f, 225f, 0);
            //    m_controllor.MinMoveSpeed = 30;
            //    m_controllor.MoveSpeedRatio = 1;
            //    m_controllor.SmoothOffset = 5;
            //    m_controllor.MinFocusSpeed = 3;
            //    m_controllor.FocusSpeedRatio = 5;
            //    m_controllor.MinViewSize = 2.5f;
            //    m_controllor.MaxViewSize = 6f;
            //    m_controllor.ResetSpeed = 0.01f;
            //    m_controllor.LookToPositionSpeed = 1f;
            //    m_controllor.IsRestrictMoveRange = true;
            //    m_controllor.MinRestrictRangeOffset = Vector3.zero;
            //    m_controllor.MaxRestrictRangeOffset = Vector3.zero;
            //    m_controllor.MinRestrictLineHorizontalOffset = 10;
            //    m_controllor.MinRestrictLineVerticalOffset = 10;
            //    m_controllor.MaxRestrictLineHorizontalOffset = 10;
            //    m_controllor.MaxRestrictLineVerticalOffset = 10;
            //    break;
            //case CameraSort.ORTHOGRAPHIC_CITYMAP:
            //case CameraSort.ORTHOGRAPHIC_CITYMAP_LIMIT:
            //    m_controllor.IsCameraManagerCreate = true;
            //    m_controllor.IsRestrictMoveRange = false;
            //    m_controllor.StartPosition = new Vector3(-20.0f, 6.0f, 50.0f);
            //    m_controllor.StartRotation = new Vector3(45.0f, 225.0f, 0.0f);

            //    m_controllor.CameraMode = CameraControl.CameraModeType.ORTHOGRAPHIC;
            //    m_controllor.SmoothOffset = 0.0f;
            //    m_controllor.ResetSpeed = 0.0f;
            //    m_controllor.MinMoveSpeed = 50;
            //    m_controllor.MoveSpeedRatio = 1;
            //    m_controllor.MinFocusSpeed = 20;
            //    m_controllor.FocusSpeedRatio = 1;

            //    m_controllor.MoveToPositionSpeed = 60;
            //    m_controllor.MinViewSize = 25.0f;
            //    m_controllor.MaxViewSize = 45.0f;
            //    m_controllor.IsRestrictMoveRange = true;
            //    m_controllor.MinRestrictRangeOffset = new Vector3(90, 2, 50f);
            //    m_controllor.MaxRestrictRangeOffset = new Vector3(87, 2, 47.5f);

            //    m_fgCamera.clearFlags = CameraClearFlags.Depth;
            //    m_fgCamera.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.02f);
            //    //m_fgCamera.cullingMask = 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_defaultLayerName) | 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_IgnoreRayCastLayerName) | 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_foregroundLayerName);//311
            //    m_fgCamera.depth = 1;
            //    m_fgCamera.orthographic = true;
            //    m_fgCamera.orthographicSize = 25.0f;
            //    m_fgCamera.nearClipPlane = -1000f;
            //    m_fgCamera.farClipPlane = 1000.0f;

            //    break;

            case CameraSort.PERSPECTIVE_CITYMAP:
            case CameraSort.PERSPECTIVE_CITYMAP_LIMIT:

                m_controllor.StartPosition = new Vector3(108.4f, 21.6f, 62.8f);
                m_controllor.StartRotation = new Vector3(28.8f, 225.0f, 0.0f);

                m_controllor.CameraMode = CameraControl.CameraModeType.PERSPECTIVE;
                m_controllor.SmoothOffset = 3f;
                m_controllor.ResetSpeed = 0.0f;
                m_controllor.MinMoveSpeed = 20;
                m_controllor.MoveSpeedRatio = 1;
                m_controllor.MinFocusSpeed = 10;
                m_controllor.FocusSpeedRatio = 1;
                m_controllor.MoveToPositionSpeed = 60;
                m_controllor.MinViewSize = 25.0f;
                m_controllor.MaxViewSize = 45.0f;
                m_controllor.IsRestrictMoveRange = true;

                m_controllor.MinRestrictRangeOffset = new Vector3(91.5f, 2f, 37f);
                m_controllor.MaxRestrictRangeOffset = new Vector3(91.5f, 2f, 37f);

                m_controllor.MinRestrictLineVerticalOffset = 75f;
                m_controllor.MinRestrictLineHorizontalOffset = 75f;
                m_controllor.MaxRestrictLineHorizontalOffset = 80f;
                m_controllor.MaxRestrictLineVerticalOffset = 80f;

                

                m_backgroundCamera.clearFlags = CameraClearFlags.Skybox;
                m_backgroundCamera.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.02f);
                m_backgroundCamera.cullingMask = 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_defaultLayerName) | 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_IgnoreRayCastLayerName) | 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_foregroundLayerName);
                m_backgroundCamera.depth = -1;
                m_backgroundCamera.fieldOfView = 55.0f;
                m_backgroundCamera.nearClipPlane = 0.01f;
                m_backgroundCamera.farClipPlane = 1000.0f;
                m_backgroundCamera.transparencySortMode = TransparencySortMode.Orthographic;

                m_middleCamera.clearFlags = CameraClearFlags.Depth;
                m_middleCamera.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.02f);
                m_middleCamera.cullingMask = 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_middleLayerName) ;
                m_middleCamera.depth = 0;
                m_middleCamera.fieldOfView = 55.0f;
                m_middleCamera.nearClipPlane = 0.01f;
                m_middleCamera.farClipPlane = 1000.0f;
                m_middleCamera.transparencySortMode = TransparencySortMode.Orthographic;

                m_foregroundCamera.clearFlags = CameraClearFlags.Depth; 
                m_foregroundCamera.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.02f);
                m_foregroundCamera.cullingMask = 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_backgroundLayerName);//311
                m_foregroundCamera.depth = 1;
                m_foregroundCamera.fieldOfView = 55.0f;
                m_foregroundCamera.nearClipPlane = 0.01f;
                m_foregroundCamera.farClipPlane = 1000.0f;
                m_foregroundCamera.transparencySortMode = TransparencySortMode.Orthographic;

                m_effectCamera.clearFlags = CameraClearFlags.Depth;
                m_effectCamera.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.02f);
                m_effectCamera.cullingMask = 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_effectLayerName) ;
                m_effectCamera.depth = 2;
                m_effectCamera.fieldOfView = 55.0f;
                m_effectCamera.nearClipPlane = 0.01f;
                m_effectCamera.farClipPlane = 1000.0f;
                m_effectCamera.transparencySortMode = TransparencySortMode.Orthographic;
                break;

            //case CameraSort.CAMPAIGN:
            //    m_controllor.IsRestrictMoveRange = false;
            //    m_controllor.StartPosition = new Vector3(10.6074f, 25.8109f, 81.4117f);
            //    m_controllor.StartRotation = new Vector3(35.0f, 200.0f, 0.0f);

            //    m_controllor.CameraMode = CameraControl.CameraModeType.PERSPECTIVE;
            //    m_controllor.SmoothOffset = 3f;
            //    m_controllor.ResetSpeed = 0.0f;
            //    m_controllor.MinMoveSpeed = 20;
            //    m_controllor.MoveSpeedRatio = 1;
            //    m_controllor.MinFocusSpeed = 1;
            //    m_controllor.MoveToPositionSpeed = 60;
            //    m_controllor.FocusSpeedRatio = 1;
            //    m_controllor.MinViewSize = 25.0f;
            //    m_controllor.MaxViewSize = 45.0f;
            //    m_controllor.IsRestrictMoveRange = true;
            //    m_controllor.MinRestrictRangeOffset = new Vector3(-13, 0, 34.8f);
            //    m_controllor.MaxRestrictRangeOffset = new Vector3(-16, 0, 28.5f);
            //    m_controllor.MinRestrictLineHorizontalOffset = 18.7f;
            //    m_controllor.MinRestrictLineVerticalOffset = 23f;
            //    m_controllor.MaxRestrictLineHorizontalOffset = 14f;
            //    m_controllor.MaxRestrictLineVerticalOffset = 18.5f;

            //    m_fgCamera.clearFlags = CameraClearFlags.Depth;
            //    m_fgCamera.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.02f);
            //    m_fgCamera.cullingMask = 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_defaultLayerName) | 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_foregroundLayerName);//311
            //    m_fgCamera.depth = 1;
            //    m_fgCamera.fieldOfView = 55.0f;
            //    m_fgCamera.nearClipPlane = 0.01f;
            //    m_fgCamera.farClipPlane = 1000.0f;


            //    m_controllor.changeScreenSize();
            //    break;
        }

        if (m_cameraType == CameraSort.ORTHOGRAPHIC_CITYMAP || m_cameraType == CameraSort.PERSPECTIVE_CITYMAP)
        {
            m_controllor.MinViewSize = 1;
            m_controllor.MaxViewSize = 1000;
        }
        else if (m_cameraType == CameraSort.ORTHOGRAPHIC_CITYMAP_LIMIT)
        {
            m_controllor.MinViewSize = 10.0f;
            m_controllor.MaxViewSize = 18.0f;
        }
        else if (m_cameraType == CameraSort.PERSPECTIVE_CITYMAP_LIMIT)
        {
            m_controllor.MinViewSize = 25.0f;
            m_controllor.MaxViewSize = 45.0f;
        }
    }

    public void setCameraFinalPos()
    {
        m_controllor.CameraFinalPos = m_controllor.transform.position;       
    }

    public void setCameraLookPos()
    {
        m_controllor.CameraLookPos = m_controllor.cameraPosToPlanePos(m_controllor.transform.position);
    }

    public void setCameraRotation(Vector3 rotation)
    {
        if( m_controllor.StartRotation != rotation )
        {
            m_controllor.StartRotation = rotation;

            m_controllor.resetCameraPosition();
        }
    }

    public void setCameraAerialView(Vector3 rotation)
    {
        if (m_controllor.StartRotation != rotation)
        {
            m_controllor.StartRotation = rotation;

            m_controllor.setAerialView();
        }
            
        
    }

    public Camera GetBgCamera()
    {
        return m_backgroundCamera;
    }

}
