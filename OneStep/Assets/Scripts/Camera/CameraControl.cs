using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;


public class CameraControl : MonoBehaviour {

    public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
    public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
    public float m_MinSize = 6.5f;                  // The smallest orthographic size the camera can be.


    [SerializeField]
    private Camera m_backgroundCamera;                        // Used for referencing the camera.
    [SerializeField]
    private Camera m_middleCamera;
    [SerializeField]
    private Camera m_foregroundCamera;
    [SerializeField]
    private Camera m_effectCamera;

    private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
    private Vector3 m_DesiredPosition;              // The position the camera is moving towards.


    public const int BASE_WIDTH = 1920;
    public const int BASE_HEIGHT = 1080;

    public enum CameraModeType
    {
        ORTHOGRAPHIC,
        PERSPECTIVE
    }

    struct ShockData
    {
        public bool isShock;
        public float amplitude;
        public float rate;
        public Vector3 allOffset;
        public float time;
        public float timer;
    }

    struct RotateData
    {
        public bool isRotating;
        public Vector3 finalRotation;
        public float maxDegrees;
        public float time;
        public float timer;
    }

    struct FOVData
    {
        public bool isChangingFOV;
        public float originalFOV;
        public float finalFOV;
        public float time;
        public float timer;
    }

    private ShockData m_shockData;
    private RotateData m_rotateData;
    private FOVData m_fovData;
    
    private Animator m_anim;
    public Animator Anim { get { return m_anim; } set { m_anim = value; } }
    private bool m_isAnimatorMayaMode = false;

    [SerializeField]
    private CameraModeType m_cameraMode;
    public CameraModeType CameraMode
    {
        get { return m_cameraMode; }
        set { m_cameraMode = value; }
    }

    [SerializeField]
    private Vector3 m_startPosition;
    public Vector3 StartPosition
    {
        get { return m_startPosition; }
        set
        {
            m_startPosition = value;
        }
    }

    [SerializeField]
    private Vector3 m_startRotation;
    public Vector3 StartRotation
    {
        get { return m_startRotation; }
        set
        {
            m_startRotation = value;
        }
    }

    private Vector3 m_cameraLookPosToCameraPosVector;

    private float m_depth;

    private Vector3 m_cameraLookPos;
    public Vector3 CameraLookPos
    {
        get { return m_cameraLookPos; }
        set { m_cameraLookPos = value; }
    }

    private Vector3 m_cameraFinalPos;
    public Vector3 CameraFinalPos
    {
        get { return m_cameraFinalPos; }
        set { m_cameraFinalPos = value; }
    }

    // Move speed: min & ratio
    [SerializeField]
    private float m_minMoveSpeed = 1;
    public float MinMoveSpeed
    {
        get { return m_minMoveSpeed; }
        set { m_minMoveSpeed = value; }
    }

    [SerializeField]
    private float m_moveSpeedRatio = 1;
    public float MoveSpeedRatio
    {
        get { return m_moveSpeedRatio; }
        set { m_moveSpeedRatio = value; }
    }

    // Smooth move: distance = velocity * smoothOffset
    [SerializeField]
    [Range(0, 100)]
    private float m_smoothOffset = 0f;
    public float SmoothOffset
    {
        get { return m_smoothOffset; }
        set { m_smoothOffset = value; }
    }

    //滑動速度，與m_moveOffset作用不同
    private Vector3 m_velocity = Vector3.zero;

    // Focus speed: min & Ratio
    [SerializeField]
    private float m_minFocusSpeed = 1;
    public float MinFocusSpeed
    {
        get { return m_minFocusSpeed; }
        set { m_minFocusSpeed = value; }
    }

    [SerializeField]
    private float m_focusSpeedRatio = 1f;
    public float FocusSpeedRatio
    {
        get { return m_focusSpeedRatio; }
        set { m_focusSpeedRatio = value; }
    }

    private bool m_isChangeCameraFocusDistance = false;
    // View Size: min & max
    [SerializeField]
    private float m_minViewSize = 10;
    public float MinViewSize
    {
        get { return m_minViewSize; }
        set { m_minViewSize = value; }
    }

    [SerializeField]
    private float m_maxViewSize = 50;
    public float MaxViewSize
    {
        get { return m_maxViewSize; }
        set { m_maxViewSize = value; }
    }

    [SerializeField]
    private float m_resetSpeed = 1;
    public float ResetSpeed
    {
        get { return m_resetSpeed; }
        set { m_resetSpeed = value; }
    }

    [SerializeField]
    private float m_moveToPositionSpeed = 60;
    public float MoveToPositionSpeed
    {
        get { return m_moveToPositionSpeed; }
        set { m_moveToPositionSpeed = value; }
    }

    [SerializeField]
    private float m_lookToPositionSpeed = 1;
    public float LookToPositionSpeed
    {
        get { return m_lookToPositionSpeed; }
        set { m_lookToPositionSpeed = value; }
    }

    [SerializeField]
    private bool m_isRestrictMoveRange = false;
    public bool IsRestrictMoveRange
    {
        get { return m_isRestrictMoveRange; }
        set { m_isRestrictMoveRange = value; }
    }

    //aX + b = Z    
    //[SerializeField]
    private Vector3 m_currentRestrictRangeOffset = Vector3.zero;
    [SerializeField]
    private Vector3 m_minRestrictRangeOffset = Vector3.zero;
    public Vector3 MinRestrictRangeOffset
    {
        set { m_minRestrictRangeOffset = value; }
    }
    [SerializeField]
    private Vector3 m_maxRestrictRangeOffset = Vector3.zero;
    public Vector3 MaxRestrictRangeOffset
    {
        set { m_maxRestrictRangeOffset = value; }
    }
    [SerializeField]
    [Range(0f, 10f)]
    private float m_restrictRangeSlope = 0;
    public float RestrictRangeSlope
    {
        set { m_restrictRangeSlope = value; }
    }
    private float m_m1;// 斜率 = m_restrictRangeSlope
    private float m_m2;// -1 / 斜率
    //[SerializeField]
    private float m_currentRestricLineHorizontalOffest;
    //[SerializeField]
    private float m_currentRestricLineVerticalOffest;
    [SerializeField]
    [Range(0f, 3000f)]
    private float m_minRestricLineHorizontalOffest = 5;
    public float MinRestrictLineHorizontalOffset
    {
        set { m_minRestricLineHorizontalOffest = value; }
    }
    [SerializeField]
    [Range(0f, 3000f)]
    private float m_minRestrictLineVerticalOffest = 5;
    public float MinRestrictLineVerticalOffset
    {
        set { m_minRestrictLineVerticalOffest = value; }
    }
    [SerializeField]
    [Range(0f, 3000f)]
    private float m_maxRestrictLineHorizontalOffest = 5;
    public float MaxRestrictLineHorizontalOffset
    {
        set { m_maxRestrictLineHorizontalOffest = value; }
    }
    [SerializeField]
    [Range(0f, 3000f)]
    private float m_maxRestrictLineVerticalOffest = 6;
    public float MaxRestrictLineVerticalOffset
    {
        set { m_maxRestrictLineVerticalOffest = value; }
    }

    //紀錄手指觸碰位置
    private Vector3 m_lastTouchPos0 = new Vector3();
    private Vector3 m_currentTouchPos0 = new Vector3();
    private Vector3 m_lastTouchPos1 = new Vector3();
    private Vector3 m_currentTouchPos1 = new Vector3();

    //是否正回歸起始點
    private bool m_isResetCamera = false;

    private bool m_isMovingToPosition = false;

    //是否正移動到目標點
    private bool m_isMoveingToLookPosition = false;

    // Flag for move by touch & avoid to move when two touch become one touch
    private bool m_canMoveByTouch = false;

    // Flag for press mouse middle button to move
    private bool m_canMoveByMouse = false;

    // Flag for smooth move after touch up
    private bool m_canCalculateSmooth = false;

    private bool m_isTouchUIOnBegan = false;

    // Distance of two touch points
    private float m_touchLength;

    // 移動偏移量，與m_velocity作用不同
    private Vector3 m_moveOffset = Vector3.zero;


    private Action<Vector3> m_cameraOnMoveAction;
    private Action<float> m_cameraOnZoomChangeAction;
    private Action m_cameraOnMoveEndAction;
    private Action<Rect, float> m_changeCameraSizeCallback;

    #region 計算用Position
    private Vector3 m_p0;
    private Vector3 m_p1;
    private Vector3 m_p2;
    private Vector3 m_p3;
    private Vector3 m_p4;
    #endregion

    public bool IsCameraManagerCreate { get; set; }

    // 为了提高性能 cache transform 降低呼叫gameobject.transform次数
    private Transform myTransform;
    public Transform MyTransform
    {
        get { return myTransform; }
    }
   
    private void Awake()
    {
        //m_Camera = GetComponentInChildren<Camera>();

        ///// 摄像机初始化 后面由摄像机manager统一管理初始化
        ////IsRestrictMoveRange = true;
        //StartPosition = new Vector3(114.4f, 16.6f, 64.8f);      //102.8f,22.7f,53.2f
        //StartRotation = new Vector3(19.8f, -135.0f, 0.0f);          //45.0f, -135.0f, 0.0f

        //CameraMode = CameraModeType.PERSPECTIVE;
        //SmoothOffset = 3f;
        //ResetSpeed = 0.0f;
        //MinMoveSpeed = 20;
        //MoveSpeedRatio = 1;
        //MinFocusSpeed = 10;
        //FocusSpeedRatio = 1;
        //MoveToPositionSpeed = 60;
        //MinViewSize = 25.0f;
        //MaxViewSize = 45.0f;
        //IsRestrictMoveRange = true;
        //MinRestrictRangeOffset = new Vector3(90, 2, 50f);
        //MaxRestrictRangeOffset = new Vector3(87, 2, 47.5f);

        //MinRestrictLineVerticalOffset = 23f;
        //MaxRestrictLineHorizontalOffset = 14f;
        //MaxRestrictLineVerticalOffset = 18.5f;

        //m_Camera.clearFlags = CameraClearFlags.Depth;
        //m_Camera.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.02f);
        ////遮罩
        ////m_Camera.cullingMask = 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_defaultLayerName) | 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_IgnoreRayCastLayerName) | 1 << LayerMask.NameToLayer(GlobalTypeDefine.sm_foregroundLayerName);//311
        //m_Camera.depth = 1;
        //m_Camera.fieldOfView = 55.0f;
        //m_Camera.nearClipPlane = 0.01f;
        //m_Camera.farClipPlane = 1000.0f;
        //m_Camera.transparencySortMode = TransparencySortMode.Orthographic;
    }

    // Use this for initialization
    private void Start()
    {
        
        //m_moveOffset = Vector3.zero;

        init();
        m_fovData.originalFOV = m_backgroundCamera.fieldOfView;  
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
        // 允許多點觸碰
        //Input.unltiTouchEnabled = true ;
#else
        //// 允許多點觸碰
        //Input.multiTouchEnabled = true;
#endif
    }

    // Update is called once per frame
    public void init()
    {
        myTransform = transform;
        myTransform.position = m_startPosition;
        myTransform.rotation = Quaternion.Euler(m_startRotation);
        m_cameraFinalPos = myTransform.position;

        m_anim = GetComponent<Animator>();

        if (!IsCameraManagerCreate)
        {
            m_backgroundCamera.orthographicSize = m_maxViewSize;
            m_middleCamera.orthographicSize = m_maxViewSize;
            m_foregroundCamera.orthographicSize = m_maxViewSize;
            m_effectCamera.orthographicSize = m_maxViewSize;
        }
        // 计算摄像机打在地图上的点
        calculateCameraViewVector_Depth_LookPos(m_startPosition, m_startRotation);
        // 计算边界
        calculateCurrentRestricLineOffset();

        switch (m_cameraMode)
        {
            case CameraModeType.ORTHOGRAPHIC:
                m_backgroundCamera.orthographic = true;
                m_middleCamera.orthographic = true;
                m_foregroundCamera.orthographic = true;
                m_effectCamera.orthographic = true;
                break;
            case CameraModeType.PERSPECTIVE:
                m_backgroundCamera.orthographic = false;
                m_middleCamera.orthographic = false;
                m_foregroundCamera.orthographic = false;
                m_effectCamera.orthographic = false;
                break;
        }
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            useAnimator(CameraManager.getInstance().AnimatorStateHashID.animation, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            useAnimator(CameraManager.getInstance().AnimatorStateHashID.fighting, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            useAnimator(CameraManager.getInstance().AnimatorStateHashID.moving, false);
            //CameraManager.getInstance().useAnimatorMayaMode(CameraManager.getInstance().AnimatorStateHashID.crash, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            useAnimator(CameraManager.getInstance().AnimatorStateHashID.crash, false);
            //CameraManager.getInstance().useAnimatorMayaMode(CameraManager.getInstance().AnimatorStateHashID.crash, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            useAnimator(CameraManager.getInstance().AnimatorStateHashID.UIVictory_crash, false);
            //CameraManager.getInstance().useAnimatorMayaMode(CameraManager.getInstance().AnimatorStateHashID.crash, false);
        }       
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            resetFOV();
            rotateCamera(new Vector3(30, 0, 0), 3);
        }

#endif

        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    shockCamera(1, 50, 2);
        //}
        //震动摄像机
        shockingCamera();
        //旋转
        rotatingCamera();
        // 改变Field of View
        changingFOV();

        //正移動到目標點時不能動作
        if (m_isMovingToPosition)
        {
            if (myTransform.position != m_cameraFinalPos)
            {
                lockInMoveRange(m_cameraFinalPos);
                myTransform.position = Vector3.MoveTowards(myTransform.position, m_cameraFinalPos, m_moveToPositionSpeed * Time.deltaTime);
            }
            else
            {
                m_isMovingToPosition = false;
                if (m_cameraOnMoveEndAction != null)
                {
                    m_cameraOnMoveEndAction.Invoke();
                }

            }

            return;
        }

        //正移動到目標點時不能動作
        if (m_isMoveingToLookPosition)
        {
            if (myTransform.position != m_cameraFinalPos)
            {
                lockInMoveRange(m_cameraFinalPos);
                myTransform.position = Vector3.MoveTowards(myTransform.position, m_cameraFinalPos, m_lookToPositionSpeed * Time.deltaTime);

                if (m_cameraOnMoveAction != null)
                {
                    m_cameraOnMoveAction.Invoke(getCameraViewWorldPosition());
                }
            }
            else
            {
                m_isMoveingToLookPosition = false;
            }

            return;
        }

        //正回到初始位置時不能動作
        if (m_isResetCamera)
        {
            if (myTransform.position != m_cameraFinalPos)
            {
                lockInMoveRange(m_cameraFinalPos);
                myTransform.position = Vector3.MoveTowards(myTransform.position, m_cameraFinalPos, m_resetSpeed * Time.deltaTime);
            }

            if (myTransform.rotation != Quaternion.Euler(m_startRotation))
            {
                myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation, Quaternion.Euler(m_startRotation), m_resetSpeed * Time.deltaTime);
            }

            if (myTransform.position != m_cameraFinalPos || myTransform.rotation != Quaternion.Euler(m_startRotation))
            {
                if (m_cameraOnMoveAction != null)
                {
                    m_cameraOnMoveAction.Invoke(getCameraViewWorldPosition());
                }

                return;
            }
            else
            {
                m_isResetCamera = false;
            }
        }

        //播放運鏡時不能動作
        if (!m_anim.GetBool("isIdle"))
        {
            if (m_isAnimatorMayaMode)
            {
                setFOV(myTransform.localScale.z);
            }

            if (m_cameraOnMoveAction != null)
            {
                m_cameraOnMoveAction.Invoke(getCameraViewWorldPosition());
            }
            return;
        }
        else if (m_isAnimatorMayaMode)
        {
            m_anim.applyRootMotion = true;
            myTransform.rotation = m_backgroundCamera.transform.rotation;
            setChildCameraLocalRotation(Vector3.zero);
            myTransform.localScale = Vector3.one;
            m_isAnimatorMayaMode = false;
        }
        else
        {
            m_anim.applyRootMotion = true;
        }

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
        MobileInput() ;
#else
        DeskTopInput();
#endif
    }

    private void FixedUpdate()
    {
        
    }

    


    private void MobileInput()
    {

    }
#if UNITY_EDITOR
    private void DeskTopInput()
    {
        m_moveOffset = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            m_moveOffset.x += Mathf.Sin(myTransform.rotation.eulerAngles.y * Mathf.PI / 180);
            m_moveOffset.y += 0;
            m_moveOffset.z += Mathf.Cos(myTransform.rotation.eulerAngles.y * Mathf.PI / 180);
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            m_moveOffset.x -= Mathf.Sin(myTransform.rotation.eulerAngles.y * Mathf.PI / 180);
            m_moveOffset.y += 0;
            m_moveOffset.z -= Mathf.Cos(myTransform.rotation.eulerAngles.y * Mathf.PI / 180);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            m_moveOffset.x -= Mathf.Sin((90 - myTransform.rotation.eulerAngles.y) * Mathf.PI / 180);
            m_moveOffset.y += 0;
            m_moveOffset.z += Mathf.Cos((90 - myTransform.rotation.eulerAngles.y) * Mathf.PI / 180);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            m_moveOffset.x = Mathf.Sin((90 - myTransform.rotation.eulerAngles.y) * Mathf.PI / 180);
            m_moveOffset.y = 0;
            m_moveOffset.z -= Mathf.Cos((90 - myTransform.rotation.eulerAngles.y) * Mathf.PI / 180);
        }

        if (m_moveOffset != Vector3.zero)
        {
            m_cameraFinalPos = myTransform.position + m_moveOffset * currentMoveSpeed() * Time.deltaTime;
            m_cameraLookPos += m_moveOffset * currentMoveSpeed() * Time.deltaTime;
            lockInMoveRange(m_cameraFinalPos);

            myTransform.position = m_cameraFinalPos;

            if (m_cameraOnMoveAction != null)
            {
                m_cameraOnMoveAction.Invoke(getCameraViewWorldPosition());
            }
        }

        // 滑鼠滾輪
        float m_scrollMouse = Input.GetAxis("Mouse ScrollWheel");
        if (/*Input.GetKeyDown(KeyCode.Alpha8) ||*/ Input.GetKey(KeyCode.KeypadPlus) || m_scrollMouse > 0)
        {
            changeCameraFocusDistance(true);
        }

        if (/*Input.GetKeyDown(KeyCode.Alpha9)||*/Input.GetKey(KeyCode.KeypadMinus) || m_scrollMouse < 0)
        {
            changeCameraFocusDistance(false);
        }

        if (Input.GetMouseButton(2) && !m_isTouchUIOnBegan)
        {
            Vector3 touchPos = Input.mousePosition;

            if (Input.GetMouseButtonDown(2))
            {
                if (isTouchingUI())
                {
                    m_isTouchUIOnBegan = true;
                    return;
                }
                m_lastTouchPos0 = getScreenPosToPlanePos(touchPos);
            }
            else
            {
                m_currentTouchPos0 = getScreenPosToPlanePos(touchPos);

                m_moveOffset = (m_lastTouchPos0 - m_currentTouchPos0);

                m_cameraFinalPos = myTransform.position + m_moveOffset;
                m_cameraLookPos = cameraPosToPlanePos(m_cameraFinalPos);
            }

            lockInMoveRange(m_cameraFinalPos);

            myTransform.position = m_cameraFinalPos;

            if (m_cameraOnMoveAction != null)
            {
                m_cameraOnMoveAction.Invoke(getCameraViewWorldPosition());
            }
        }

        if (Input.GetMouseButtonUp(2))
        {
            m_isTouchUIOnBegan = false;

            m_velocity = m_lastTouchPos0 - m_currentTouchPos0;

            m_lastTouchPos0 = new Vector3(0, m_lastTouchPos0.y, 0);

            m_currentTouchPos0 = new Vector3(0, m_currentTouchPos0.y, 0);

            m_canMoveByMouse = false;


            if (m_velocity.sqrMagnitude > 0.5f)
            {
                m_cameraFinalPos += m_velocity * m_smoothOffset;
                m_cameraLookPos = cameraPosToPlanePos(m_cameraFinalPos);
                m_canCalculateSmooth = true;
            }
        }

        //滑動
        if (m_canCalculateSmooth)
        {
            lockInMoveRange(m_cameraFinalPos);
            myTransform.position = Vector3.MoveTowards(myTransform.position, m_cameraFinalPos, currentMoveSpeed() * Time.deltaTime);

            if (myTransform.position == m_cameraFinalPos)
            {
                m_canCalculateSmooth = false;
            }

            if (m_cameraOnMoveAction != null)
            {
                m_cameraOnMoveAction.Invoke(getCameraViewWorldPosition());
            }
        }

        //touchInput();

        //if (Input.GetKey(KeyCode.Q))
        //{
        //    myTransform.Rotate(Vector3.forward * m_rotateSpeed * Time.deltaTime);
        //}

        //if (Input.GetKey(KeyCode.E))
        //{
        //    myTransform.Rotate(Vector3.back * m_rotateSpeed * Time.deltaTime);
        //}
    }
#endif


    

    private float currentMoveSpeed()
    {
        if (m_backgroundCamera.orthographic)
        {
            if (m_backgroundCamera.orthographicSize > m_minViewSize)
            {
                return m_minMoveSpeed + m_moveSpeedRatio * ((m_backgroundCamera.orthographicSize - m_minViewSize)/* / (m_maxViewSize - m_minViewSize)*/);
            }
            else
            {
                return m_minMoveSpeed;
            }
        }
        else
        {
            if (m_depth > m_minViewSize)
            {
                return m_minMoveSpeed + m_moveSpeedRatio * ((m_depth - m_minViewSize) /*/ (m_maxViewSize - m_minViewSize)*/);
            }
            else
            {
                return m_minMoveSpeed;
            }
        }
    }


    public bool isTouchingUI()
    {
        if (EventSystem.current == null)
            return false;

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount != 1)
                return false;

            if (Input.GetTouch(0).phase != TouchPhase.Began)
                return false;

            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }
        else
        {
            if (Input.GetMouseButtonDown(2) == false)
                return false;

            return EventSystem.current.IsPointerOverGameObject();
        }
    }


    private void changeCameraFocusDistance(bool isPlus)
    {
        //Cyclone @Reviewer: 簡化寫法
        if (m_cameraMode == CameraModeType.ORTHOGRAPHIC)
        {
            float size = m_backgroundCamera.orthographicSize;

            float realForcsSpeed = m_minFocusSpeed + m_focusSpeedRatio * ((m_backgroundCamera.orthographicSize - m_minViewSize) / (m_maxViewSize - m_minViewSize));

            if (isPlus)
            {
                if (m_backgroundCamera.orthographicSize < m_maxViewSize)
                {
                    size = Mathf.Lerp(m_backgroundCamera.orthographicSize, m_backgroundCamera.orthographicSize + 1, realForcsSpeed /** m_focusSpeedScrollMouseAdjust*/ * Time.deltaTime);
                }
                else
                {
                    size = m_maxViewSize;
                }
            }
            else
            {
                if (m_backgroundCamera.orthographicSize > m_minViewSize)
                {
                    size = Mathf.Lerp(m_backgroundCamera.orthographicSize, m_backgroundCamera.orthographicSize - 1, realForcsSpeed /** m_focusSpeedScrollMouseAdjust*/ * Time.deltaTime);
                }
                else
                {
                    size = m_minViewSize;
                }
            }

            calculateCurrentRestricLineOffset();

            m_backgroundCamera.orthographicSize = size;
            m_middleCamera.orthographicSize = size;
            m_foregroundCamera.orthographicSize = size;
            m_effectCamera.orthographicSize = size;

            if (m_cameraOnZoomChangeAction != null)
                m_cameraOnZoomChangeAction.Invoke(m_backgroundCamera.orthographicSize / m_maxViewSize);
        }
        else
        {
            if (isPlus)
            {
                m_depth += (m_minFocusSpeed + m_focusSpeedRatio * (m_depth /*/ (m_maxViewSize - m_minViewSize*/)) * Time.deltaTime;
            }
            else
            {
                m_depth -= (m_minFocusSpeed + m_focusSpeedRatio * (m_depth /*/ (m_maxViewSize - m_minViewSize*/)) * Time.deltaTime;
            }

            if (m_depth > m_maxViewSize)
            {
                m_depth = m_maxViewSize;
            }
            else if (m_depth < m_minViewSize)
            {
                m_depth = m_minViewSize;
            }

            calculateCurrentRestricLineOffset();

            m_cameraFinalPos = planePosToCameraPos(m_cameraLookPos);
            lockInMoveRange(m_cameraFinalPos);
            myTransform.position = m_cameraFinalPos;

            if (m_cameraOnZoomChangeAction != null)
                m_cameraOnZoomChangeAction.Invoke(m_depth / m_maxViewSize);

            if (m_cameraOnMoveAction != null)
                m_cameraOnMoveAction.Invoke(getCameraViewWorldPosition());
        }
    }

    

    private Vector3 getCameraViewWorldPosition()
    {
        return myTransform.position - m_cameraLookPosToCameraPosVector * m_depth;
    }

    // Animator
    public void useAnimator(int hashID, bool isRelative = true)
    {
        m_anim.SetBool("isIdle", false);

        m_anim.applyRootMotion = isRelative;
        m_anim.Play(hashID, 0, 0);
    }

    public void useAnimatorMayaMode(int hashID, bool isRelative = true)
    {
        m_isAnimatorMayaMode = true;

        useAnimator(hashID, isRelative);

        // maya animation tricky : camera position.y = 180
        setChildCameraLocalRotation(new Vector3(0, 180, 0));
    }

    public void setAerialView()
    {
        myTransform.position = new Vector3(108.4f , 100f , 62.8f);
    }
    // Reset camera position
    public void resetCameraPosition()
    {
        myTransform.position = m_startPosition;
        myTransform.rotation = Quaternion.Euler(m_startRotation);
        m_cameraFinalPos = myTransform.position;

        calculateCameraViewVector_Depth_LookPos(m_startPosition, m_startRotation);
    }

    public void resetCameraPositionLerp()
    {
        m_isResetCamera = true;
        m_cameraLookPos = cameraPosToPlanePos(m_startPosition);
        m_cameraFinalPos = m_startPosition;
        calculateCameraViewVector_Depth_LookPos(m_startPosition, m_startRotation);
    }

    // Cyclone@Reviewer: 兩個函式可以合併
    //public void resetCameraPosition(bool isLerp)
    //{
    //    m_isResetCamera = isLerp;

    //    if (!isLerp)
    //    {
    //        myTransform.position = m_startPosition;
    //        myTransform.rotation = Quaternion.Euler(m_startRotation);
    //    }

    //    m_cameraFinalPos = m_startPosition;

    //    calculateCameraViewVector_Depth_LookPos();
    //}

    public void setStartPosition(Vector3 lookPos)
    {
        m_cameraLookPos = lookPos;
        m_startPosition = planePosToCameraPos(lookPos);
    }

    public void moveToPosition(Vector3 pos)
    {
        m_cameraFinalPos = pos;
        myTransform.position = pos;
        m_cameraLookPos = cameraPosToPlanePos(pos);

        //立即結束直接回傳Callback
        if (m_cameraOnMoveEndAction != null)
        {
            m_cameraOnMoveEndAction.Invoke();
        }
    }

    public void moveToPositionLerp(Vector3 pos)
    {
        m_isMovingToPosition = true;
        m_cameraFinalPos = pos;
        m_cameraLookPos = cameraPosToPlanePos(pos);
    }

    // Look at position
    public void lookAtPosition(Vector3 lookPos)
    {
        m_cameraLookPos = lookPos;
        m_cameraFinalPos = planePosToCameraPos(lookPos);
        myTransform.position = m_cameraFinalPos;
    }

    public void lookAtPositionLerp(Vector3 lookPos)
    {
        m_isMoveingToLookPosition = true;
        m_cameraLookPos = lookPos;
        m_cameraFinalPos = planePosToCameraPos(lookPos);
    }

    // Cyclone@Reviewer: 兩個函式可以合併
    //public void lookAtPosition(bool isLerp, Vector3 lookPos)
    //{
    //    m_isMoveingToTarget = isLerp;
    //
    //    m_cameraLookPos = lookPos;
    //    m_cameraFinalPos = planePosToCameraPos(lookPos);
    //
    //    if (isLerp)
    //    {
    //        myTransform.position = m_cameraFinalPos;
    //    }
    //}

#region Register/DeRegister
    //ChangeCameraSizeCallback
    public void registerChangeCameraSizeCallback(Action<Rect, float> changeCameraSizeCallback)
    {
        m_changeCameraSizeCallback += changeCameraSizeCallback;
    }
    public void deleteChangeCameraSizeCallback(Action<Rect, float> changeCameraSizeCallback)
    {
        m_changeCameraSizeCallback -= changeCameraSizeCallback;
    }

    // CameraOnMoveEndAction
    public void registerCameraOnMoveEndAction(Action onMoveEndAction)
    {
        m_cameraOnMoveEndAction += onMoveEndAction;
    }
    public void deleteCameraOnMoveEndAction(Action onMoveEndAction)
    {
        m_cameraOnMoveEndAction -= onMoveEndAction;
    }

    // CameraOnMoveAction
    public void registerCameraMoveAction(Action<Vector3> onMoveAction)
    {
        m_cameraOnMoveAction += onMoveAction;
    }
    public void deleteCameraMoveAction(Action<Vector3> onMoveAction)
    {
        m_cameraOnMoveAction -= onMoveAction;
    }

    //CameraOnZoomChangeAction
    public void registerCameraZoomChangeAction(Action<float> onZoomChangeAction)
    {
        m_cameraOnZoomChangeAction += onZoomChangeAction;
    }
    public void deleteCameraZoomChangeAction(Action<float> onZoomChangeAction)
    {
        m_cameraOnZoomChangeAction -= onZoomChangeAction;
    }
#endregion

    // Calculate Func
    public Vector3 planePosToCameraPos(Vector3 planePos)
    {
        return planePos + m_cameraLookPosToCameraPosVector * m_depth;
    }

    public Vector3 cameraPosToPlanePos(Vector3 cameraPos)
    {
        return cameraPos - m_cameraLookPosToCameraPosVector * m_depth;
    }

    private void calculateCameraViewVector_Depth_LookPos(Vector3 pos, Vector3 rotation)
    {
        m_p1 = pos;
        // p0摄像机在平面上的位置
        m_p0 = new Vector3(m_p1.x, 0, m_p1.z);
        // p2为射线的投影长度
        m_p2 = new Vector3(m_p1.x, 0, m_p1.z + m_p1.y * Mathf.Tan((90 - rotation.x) * Mathf.PI / 180));
        // 向量长度
        float p0p2 = (m_p0 - m_p2).magnitude;
        // 摄像机的射点在平面上的位置
        m_p3 = new Vector3(m_p1.x + p0p2 * Mathf.Sin(rotation.y * Mathf.PI / 180), 0, m_p1.z + p0p2 * Mathf.Cos(rotation.y * Mathf.PI / 180));

        m_cameraLookPosToCameraPosVector = (m_p1 - m_p3).normalized;
        // 射线长度
        m_depth = (m_p1 - m_p3).magnitude;
        m_cameraLookPos = m_p3;
    }

    private Vector3 getScreenPosToPlanePos(Vector3 screenTouchPos)
    {
        m_p0 = m_backgroundCamera.ScreenToWorldPoint(screenTouchPos);
        screenTouchPos.z = m_backgroundCamera.nearClipPlane;
        m_p1 = m_backgroundCamera.ScreenToWorldPoint(screenTouchPos);

        Vector3 p0p1 = m_p1 - m_p0;
        float n;
        if (p0p1.y != 0)
            n = -1 * m_p0.y / p0p1.y;
        else
            n = 0;
        m_p2 = p0p1 * n + m_p0;

        return m_p2;
    }

    private void calculateCurrentRestricLineOffset()
    {
        if (!m_isRestrictMoveRange)
            return;

        float realDepth = 0;
        if (m_cameraMode == CameraModeType.ORTHOGRAPHIC)
            realDepth = m_backgroundCamera.orthographicSize;
        else if (m_cameraMode == CameraModeType.PERSPECTIVE)
            realDepth = m_depth;

        float percent = (realDepth - m_minViewSize) / (m_maxViewSize - m_minViewSize);
        m_currentRestricLineHorizontalOffest = m_minRestricLineHorizontalOffest + (m_maxRestrictLineHorizontalOffest - m_minRestricLineHorizontalOffest) * percent;
        m_currentRestricLineVerticalOffest = m_minRestrictLineVerticalOffest + (m_maxRestrictLineVerticalOffest - m_minRestrictLineVerticalOffest) * percent;

        m_currentRestrictRangeOffset = m_minRestrictRangeOffset + (m_maxRestrictRangeOffset - m_minRestrictRangeOffset) * percent;
    }

    //得到兩線交點
    private Vector3 getIntersection(float m1, float lineHorizontalParameter, float m2, float lineVerticalParameter)
    {
        if (m_restrictRangeSlope != 0)
        {
            m1 = m_restrictRangeSlope;
            m2 = -1 / m1;
            float x = (lineVerticalParameter - lineHorizontalParameter) / (m1 - m2);
            float z = m1 * x + lineHorizontalParameter;
            return new Vector3(x, 0, z);
        }
        else
        {
            return new Vector3(lineHorizontalParameter, 0, lineVerticalParameter);
        }
    }

    // m1 * X + H = Z
    // m1 * X + (-H) = Z
    // m2 * X + V = Z
    // m2 * X + (-V) = Z
    private void lockInMoveRange(Vector3 cameraFinalPos)
    {
        if (!m_isRestrictMoveRange)
            return;

        Vector3 cameraLookPos = cameraPosToPlanePos(cameraFinalPos) - m_currentRestrictRangeOffset;
        if (m_restrictRangeSlope != 0)
        {
            m_m1 = m_restrictRangeSlope;
            m_m2 = -1 / m_m1;

            //超出Horizontal雙線範圍則修正回線上            
            if ((m_m1 * cameraLookPos.x - cameraLookPos.z + m_currentRestricLineHorizontalOffest) < 0)
            {
                float currentV = cameraLookPos.z - m_m2 * cameraLookPos.x;
                cameraLookPos = getIntersection(m_m1, m_currentRestricLineHorizontalOffest, m_m2, currentV);

            }
            else if ((m_m1 * cameraLookPos.x - cameraLookPos.z + (-1 * m_currentRestricLineHorizontalOffest)) > 0)
            {
                float currentV = cameraLookPos.z - m_m2 * cameraLookPos.x;
                cameraLookPos = getIntersection(m_m1, -1 * m_currentRestricLineHorizontalOffest, m_m2, currentV);
            }

            //超出Vertical雙線範圍則修正回線上
            if ((m_m2 * cameraLookPos.x - cameraLookPos.z + m_currentRestricLineVerticalOffest) < 0)
            {
                float currentH = cameraLookPos.z - m_m1 * cameraLookPos.x;
                cameraLookPos = getIntersection(m_m1, currentH, m_m2, m_currentRestricLineVerticalOffest);
            }
            else if ((m_m2 * cameraLookPos.x - cameraLookPos.z + (-1 * m_currentRestricLineVerticalOffest)) > 0)
            {
                float currentH = cameraLookPos.z - m_m1 * cameraLookPos.x;
                cameraLookPos = getIntersection(m_m1, currentH, m_m2, -1 * m_currentRestricLineVerticalOffest);
            }
        }
        else
        {
            if (cameraLookPos.x > m_currentRestricLineHorizontalOffest)
                cameraLookPos.x = m_currentRestricLineHorizontalOffest;
            else if (cameraLookPos.x < -1 * m_currentRestricLineHorizontalOffest)
                cameraLookPos.x = -1 * m_currentRestricLineHorizontalOffest;

            if (cameraLookPos.z > m_currentRestricLineVerticalOffest)
                cameraLookPos.z = m_currentRestricLineVerticalOffest;
            else if (cameraLookPos.z < -1 * m_currentRestricLineVerticalOffest)
                cameraLookPos.z = -1 * m_currentRestricLineVerticalOffest;
        }

        m_cameraLookPos = cameraLookPos + m_currentRestrictRangeOffset;
        m_cameraFinalPos = planePosToCameraPos(m_cameraLookPos);
    }

    public void resetScreenSize()
    {
        Rect rect = new Rect();
        rect.width = 1.0f;
        rect.height = 1.0f;
        rect.x = 0;
        rect.y = 0;

        m_backgroundCamera.rect = rect;
        m_middleCamera.rect = rect;
        m_foregroundCamera.rect = rect;
        m_effectCamera.rect = rect;
        //m_effectCamera.rect = rect;

        if (m_changeCameraSizeCallback != null)
            m_changeCameraSizeCallback.Invoke(rect, 1f);
    }

    public void changeScreenSize()
    {
        //螢幕解析度調整(黑邊)
        float windowAspect = (float)Screen.width / (float)Screen.height;

        float m_targetRatio = (float)BASE_WIDTH / (float)BASE_HEIGHT;

        Rect rect = new Rect();
        //UI canvas match width and height should only have relation with screen horizontal or vertical
        float uiCanvasRatio = 1f;
        float scaleHeight = Screen.width * ((float)BASE_HEIGHT / (float)BASE_WIDTH) / Screen.height;

        //寬不夠，縮高
        if (m_targetRatio > windowAspect)
        {
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            uiCanvasRatio = 1f;
        }
        else //高不夠，縮寬
        {
            float scaleWidth = 1 / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            uiCanvasRatio = 1f;
        }

        m_backgroundCamera.rect = rect;
        m_middleCamera.rect = rect;
        m_foregroundCamera.rect = rect;
        m_effectCamera.rect = rect;

        if (m_changeCameraSizeCallback != null)
            m_changeCameraSizeCallback.Invoke(rect, uiCanvasRatio);

        //float size = 0;
        //if (m_cameraMode == CameraModeType.ORTHOGRAPHIC)
        //{
        //    size = m_fgCamera.orthographicSize / m_maxViewSize;
        //}
        //else
        //{
        //    size = m_depth / m_maxViewSize;                
        //}

        //m_cameraOnZoomChangeAction.Invoke(size);
    }

    private void shockCamera(float amplitude, float rate, float time)
    {
        m_shockData.isShock = true;
        m_shockData.amplitude = amplitude;
        m_shockData.rate = rate;
        m_shockData.time = time;
        m_shockData.timer = 0;
        m_shockData.allOffset = Vector3.zero;
    }

    private void shockingCamera()
    {
        if (m_shockData.isShock)
        {
            Vector3 offset;
            if (m_shockData.timer < m_shockData.time)
            {
                offset = new Vector3(m_shockData.amplitude * Mathf.Cos(m_shockData.rate * m_shockData.timer), 0, 0);
                myTransform.position += offset;
                //UIGameObjectManager.getInstance().UICamera.myTransform.position += offset;
                m_shockData.allOffset += offset;

                m_shockData.timer += Time.deltaTime;
            }
            else
            {
                m_shockData.isShock = false;
                myTransform.position -= m_shockData.allOffset;
                //UIGameObjectManager.getInstance().UICamera.myTransform.position -= m_shockData.allOffset;
            }
        }
    }

    public void rotateCamera(Vector3 finalRotation, float time = 0)
    {
        m_rotateData.isRotating = true;
        m_rotateData.finalRotation = finalRotation;
        m_rotateData.maxDegrees = Vector3.Angle(myTransform.rotation.eulerAngles, finalRotation);
        m_rotateData.time = time;
        m_rotateData.timer = 0;
    }

    private void rotatingCamera()
    {
        if (m_rotateData.isRotating)
        {
            if (m_rotateData.timer < m_rotateData.time)
            {
                myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation, Quaternion.Euler(m_rotateData.finalRotation), m_rotateData.maxDegrees * Time.deltaTime);
                m_rotateData.timer += Time.deltaTime;
            }
            else
            {
                if (m_rotateData.time == 0)
                    myTransform.rotation = Quaternion.Euler(m_rotateData.finalRotation);

                calculateCameraViewVector_Depth_LookPos(m_cameraFinalPos, m_rotateData.finalRotation);
                m_rotateData.isRotating = false;
            }
        }
    }

    public void changeFOV(float finalFOV, float time = 0)
    {
        m_fovData.isChangingFOV = true;
        m_fovData.finalFOV = finalFOV;
        m_fovData.time = time;
        m_fovData.timer = 0;
    }
    private void changingFOV()
    {
        if (m_fovData.isChangingFOV)
        {
            if (m_fovData.timer < m_fovData.time)
            {
                //所有相機FOV都一樣，以fgCamera做代表
                float maxDelta = ((m_backgroundCamera.fieldOfView - m_fovData.finalFOV) / m_fovData.time) * Time.deltaTime;
                m_backgroundCamera.fieldOfView = Mathf.MoveTowards(m_backgroundCamera.fieldOfView, m_fovData.finalFOV, maxDelta);
                m_middleCamera.fieldOfView = Mathf.MoveTowards(m_backgroundCamera.fieldOfView, m_fovData.finalFOV, maxDelta);
                m_foregroundCamera.fieldOfView = Mathf.MoveTowards(m_backgroundCamera.fieldOfView, m_fovData.finalFOV, maxDelta);
                m_effectCamera.fieldOfView = Mathf.MoveTowards(m_backgroundCamera.fieldOfView, m_fovData.finalFOV, maxDelta);


                m_backgroundCamera.fieldOfView = Mathf.MoveTowards(m_backgroundCamera.fieldOfView, m_fovData.finalFOV, ((m_backgroundCamera.fieldOfView - m_fovData.finalFOV) / m_fovData.time) * Time.deltaTime);
                m_foregroundCamera.fieldOfView = Mathf.MoveTowards(m_foregroundCamera.fieldOfView, m_fovData.finalFOV, ((m_foregroundCamera.fieldOfView - m_fovData.finalFOV) / m_fovData.time) * Time.deltaTime);
                m_middleCamera.fieldOfView = Mathf.MoveTowards(m_middleCamera.fieldOfView, m_fovData.finalFOV, ((m_middleCamera.fieldOfView - m_fovData.finalFOV) / m_fovData.time) * Time.deltaTime);
                m_effectCamera.fieldOfView = Mathf.MoveTowards(m_effectCamera.fieldOfView, m_fovData.finalFOV, ((m_effectCamera.fieldOfView - m_fovData.finalFOV) / m_fovData.time) * Time.deltaTime);
                m_fovData.timer += Time.deltaTime;
            }
            else
            {
                //時間為0瞬間轉換
                if (m_fovData.time == 0)
                {
                    setFOV(m_fovData.finalFOV);
                }

                m_fovData.isChangingFOV = false;
            }
        }
    }

    public void resetFOV(float time = 0)
    {
        changeFOV(m_fovData.originalFOV, time);
    }

    public void setFOV(float FOV)
    {
        m_backgroundCamera.fieldOfView = FOV;
        m_middleCamera.fieldOfView = FOV;
        m_foregroundCamera.fieldOfView = FOV;
        m_effectCamera.fieldOfView = FOV;
    }

    public void setChildCameraLocalRotation(Vector3 rotation)
    {
        m_backgroundCamera.transform.localRotation = Quaternion.Euler(rotation);
        m_middleCamera.transform.localRotation = Quaternion.Euler(rotation);
        m_foregroundCamera.transform.localRotation = Quaternion.Euler(rotation);
        m_effectCamera.transform.localRotation = Quaternion.Euler(rotation);
    }

#if UNITY_EDITOR
    Vector3 a1, a2, a3, a4, viewPort;
    [SerializeField]
    private bool m_isShowCameraViewRange = false;
    private void OnDrawGizmos()
    {
        if (m_isShowCameraViewRange)
        {
            RaycastHit hit;
            viewPort = Vector3.zero;
            Ray ray = m_backgroundCamera.ViewportPointToRay(viewPort);
            if (Physics.Raycast(ray, out hit))
            {
                a1 = hit.point;
            }
            viewPort.Set(1, 0, 0);
            ray = m_backgroundCamera.ViewportPointToRay(viewPort);
            if (Physics.Raycast(ray, out hit))
            {
                a2 = hit.point;
            }
            viewPort.Set(1, 1, 0);
            ray = m_backgroundCamera.ViewportPointToRay(viewPort);
            if (Physics.Raycast(ray, out hit))
            {
                a3 = hit.point;
            }
            viewPort.Set(0, 1, 0);
            ray = m_backgroundCamera.ViewportPointToRay(viewPort);
            if (Physics.Raycast(ray, out hit))
            {
                a4 = hit.point;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(a1, a2);
            Gizmos.DrawLine(a2, a3);
            Gizmos.DrawLine(a3, a4);
            Gizmos.DrawLine(a4, a1);
        }


        calculateCurrentRestricLineOffset();

        Gizmos.color = Color.yellow;
        a1 = m_minRestrictRangeOffset + getIntersection(m_m1, m_minRestricLineHorizontalOffest, m_m2, m_minRestrictLineVerticalOffest);
        a2 = m_minRestrictRangeOffset + getIntersection(m_m1, m_minRestricLineHorizontalOffest, m_m2, -1 * m_minRestrictLineVerticalOffest);
        a3 = m_minRestrictRangeOffset + getIntersection(m_m1, -1 * m_minRestricLineHorizontalOffest, m_m2, -1 * m_minRestrictLineVerticalOffest);
        a4 = m_minRestrictRangeOffset + getIntersection(m_m1, -1 * m_minRestricLineHorizontalOffest, m_m2, m_minRestrictLineVerticalOffest);
        Gizmos.DrawLine(a1, a2);
        Gizmos.DrawLine(a2, a3);
        Gizmos.DrawLine(a3, a4);
        Gizmos.DrawLine(a4, a1);

        Gizmos.color = Color.cyan;
        a1 = m_maxRestrictRangeOffset + getIntersection(m_m1, m_maxRestrictLineHorizontalOffest, m_m2, m_maxRestrictLineVerticalOffest);
        a2 = m_maxRestrictRangeOffset + getIntersection(m_m1, m_maxRestrictLineHorizontalOffest, m_m2, -1 * m_maxRestrictLineVerticalOffest);
        a3 = m_maxRestrictRangeOffset + getIntersection(m_m1, -1 * m_maxRestrictLineHorizontalOffest, m_m2, -1 * m_maxRestrictLineVerticalOffest);
        a4 = m_maxRestrictRangeOffset + getIntersection(m_m1, -1 * m_maxRestrictLineHorizontalOffest, m_m2, m_maxRestrictLineVerticalOffest);
        Gizmos.DrawLine(a1, a2);
        Gizmos.DrawLine(a2, a3);
        Gizmos.DrawLine(a3, a4);
        Gizmos.DrawLine(a4, a1);

        Gizmos.color = Color.blue;
        a1 = m_currentRestrictRangeOffset + getIntersection(m_m1, m_currentRestricLineHorizontalOffest, m_m2, m_currentRestricLineVerticalOffest);
        a2 = m_currentRestrictRangeOffset + getIntersection(m_m1, m_currentRestricLineHorizontalOffest, m_m2, -1 * m_currentRestricLineVerticalOffest);
        a3 = m_currentRestrictRangeOffset + getIntersection(m_m1, -1 * m_currentRestricLineHorizontalOffest, m_m2, -1 * m_currentRestricLineVerticalOffest);
        a4 = m_currentRestrictRangeOffset + getIntersection(m_m1, -1 * m_currentRestricLineHorizontalOffest, m_m2, m_currentRestricLineVerticalOffest);
        Gizmos.DrawLine(a1, a2);
        Gizmos.DrawLine(a2, a3);
        Gizmos.DrawLine(a3, a4);
        Gizmos.DrawLine(a4, a1);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_cameraLookPos, m_cameraFinalPos);
    }
#endif


    public void ChangeCameraPosision(bool b)
    {
        return;
        if(b)
        {
            myTransform.position = new Vector3(114f, 41f, 64f);
        }
        else
        {
            myTransform.position = new Vector3(114.4f, 16.6f, 64.8f);
        }
        

//         m_cameraFinalPos = myTransform.position + m_moveOffset * currentMoveSpeed() * Time.deltaTime;
//         m_cameraLookPos += m_moveOffset * currentMoveSpeed() * Time.deltaTime;
//         lockInMoveRange(m_cameraFinalPos);
    }

}
