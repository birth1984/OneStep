using UnityEngine;
using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;

public class GridControllor : MonoBehaviour
{

    // Use this for initialization

    //private Vector3 m_startPos = new Vector3(91f, 0.1f, 37f);
    //private Vector3 m_zeroPos = new Vector3(16.5f, 0.1f, -37.5f);
    private GameObject m_grids;
    private GameObject m_plane;

    private Vector3 m_clickPos;
    private Vector3 m_movePos;

    private float m_selectW;
    private float m_selectH;

    private static GridControllor m_instance = null;

    public static GridControllor Instance
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

    void Start()
    {
        UnityEngine.Object gridRes = Resources.Load("Prefabs/Grid/uteLayer");

        m_grids = (GameObject)UnityEngine.Object.Instantiate(gridRes);

        m_grids.layer = 0;

        m_grids.name = "grids";

        m_grids.transform.SetParent(transform);

        m_grids.transform.position = HomeMapManager.getInstance().StartPos; // m_startPos;

        m_grids.transform.localScale = new Vector3(150, 0.1f, 150);

        m_grids.layer = 9;

        Renderer renderer = m_grids.GetComponent<Renderer>();

        renderer.material.mainTextureScale = new Vector2(150f, 150f);

        renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 81 / 255.0f);

        m_plane = createPlane(new Vector3(1.0f, 0.1f, 1.0f), new Color(0.4f, 1.0f, 0.4f));
        m_plane.layer = 2;
        m_plane.name = "test";
        m_plane.transform.position = new Vector3(91f - 75f + 0.5f, 2f, 37f - 75f + 0.5f); ;
        m_plane.transform.SetParent(transform);
        m_plane.layer = 9;

        //删除文件
        //DeleteFile(Application.persistentDataPath, "FileName.txt");
        //创建文件，共写入3次数据
        // 	    CreateFile(Application.persistentDataPath,"FileName.txt","宣雨松MOMO");
        // 	    CreateFile(Application.persistentDataPath,"FileName.txt","宣雨松MOMO");
        // 	    CreateFile(Application.persistentDataPath ,"FileName.txt","宣雨松MOMO");
        //         //文本中每行的内容
        // 	    ArrayList infoall;
        // 	    //得到文本中每一行的内容
        // 	    infoall = LoadFile(Application.persistentDataPath,"FileName.txt");
    }




    public GameObject createPlane(Vector3 size, Color color)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        plane.GetComponent<MeshCollider>().enabled = false;

        plane.GetComponent<MeshRenderer>().material.color = color;

        plane.transform.localScale = new Vector3(0.1f * size.x, 0.1f, 0.1f * size.z);

        return plane;
    }

    // Update is called once per frame
    void Update()
    {
       // HomeMapEditor
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        // show mouse position & grid infomation
        

        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            if (hit.collider && hit.collider.gameObject.name.Equals("grids"))
            {
                Vector3 originPos = hit.collider.gameObject.transform.position - 0.5f * hit.collider.gameObject.transform.localScale;

                Vector3 dist = new Vector3(Mathf.Floor(hit.point.x - originPos.x) + 0.5f, hit.point.y - originPos.y + 0.15f, Mathf.Floor(hit.point.z - originPos.z) + 0.5f);

                Vector3 pos = originPos + dist;
                // 用于临时计算
                Vector3 tempPos = new Vector3();

                if (Input.GetMouseButtonDown(0))
                {
                    m_clickPos = pos;
                    //m_plane.transform.position = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 2f, Mathf.Floor(hit.point.z) + 0.5f);
                    Vector3 targetGrid = new Vector3(1f, 0.11f, 1f);
                    Debug.Log("MouseButtonDown");
                }
                if (Input.GetMouseButton(0))
                {                 
                    dist = pos - m_clickPos;
                    float w = Mathf.Abs(dist.x) + 1.0f;
                    float h = Mathf.Abs(dist.z) + 1.0f;
                    Debug.Log("MouseButton" + w + " " + h );
                    m_plane.transform.localScale = new Vector3(0.1f * w, 0.1f, 0.1f * h);

                    tempPos = m_clickPos + 0.5f * dist;
                    tempPos.y = 0.05f; // originPos.y;
                    m_plane.transform.position = tempPos;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    tempPos = m_plane.transform.position - HomeMapManager.getInstance().ZeroPos ;// m_zeroPos;
                    Debug.Log("MouseButtonUp" + tempPos + "_" + m_plane.transform.localScale);

                    m_selectW = Mathf.Floor(m_plane.transform.localScale.x * 10);
                    m_selectH = Mathf.Floor(m_plane.transform.localScale.z * 10);

                    // 测试edit region
                    HomeMapEditor.Instance.RegionX = (m_clickPos.x - HomeMapManager.getInstance().ZeroPos.x).ToString();
                    HomeMapEditor.Instance.RegionY =(m_clickPos.z - HomeMapManager.getInstance().ZeroPos.z).ToString();
                    HomeMapEditor.Instance.RegionWidth = m_selectW.ToString();
                    HomeMapEditor.Instance.RegionHeight = m_selectH.ToString();

                    Vector2 p = new Vector2();

                    if (m_selectW % 2 == 0)
                    {
                        p.x = Mathf.Floor(tempPos.x) + Mathf.Floor(m_selectW / 2);
                    }
                    else
                    {
                        p.x = tempPos.x + Mathf.Floor(m_selectW / 2);
                    }

                    if (m_selectH % 2 == 0)
                    {
                        p.y = Mathf.Floor(tempPos.z) + Mathf.Floor(m_selectH / 2);
                    }
                    else
                    {
                        p.y = tempPos.z + Mathf.Floor(m_selectH / 2);
                    }

                    //Vector2[] datas = new Vector2[22500] ;
                    //Byte[] mapdata = new Byte[150*150];
                    //List<Vector2> datas = new List<Vector2>();
                    Vector2 point = new Vector2();
                    int index;
                    for (int i = 0; i < m_selectW; i++)
                    {
                        for (int j = 0; j < m_selectH; j++)
                        {
                            point.x = p.x - i;
                            point.y = p.y - j;
                            MapDataManager.getInstance().GridDatas.Add(point);
                            index = (int)(p.y * 150 + p.x);
                            MapDataManager.getInstance().MapCharData[index] = '1';
                        }
                    }
                    //Debug.Log("Count:" + datas.Count + datas.ToString());
                    //char[] tempchar = Encoding.ASCII.GetChars(mapdata);
                    //string tempstring = Encoding.UTF8.GetString(mapdata);
                    //string da = tempchar.ToString() ;
                    //MapDataManager.getInstance().CreateFile(Application.persistentDataPath, "FileName.txt", "" + tempstring);
                    //da = null;
                    //m_plane.transform.position ;
                    //m_plane.transform.localScale;
                }
            }
        }
    }

    /**
     * 设置Region位置大小
     */
    public void SetGridPosition(float x , float z , float width , float height)
    {
        Vector3 tempPos = new Vector3();
        Vector3 targetGrid = new Vector3(1f, 0.11f, 1f);    
       
        float w = Mathf.Abs(width) + 1.0f;
        float h = Mathf.Abs(height) + 1.0f;
        m_plane.transform.localScale = new Vector3(0.1f * width, 0.1f, 0.1f * height);

        tempPos.x = HomeMapManager.getInstance().ZeroPos.x + x + 0.5f * (width) - 0.5f ;
        tempPos.y = 0.05f; 
        tempPos.z = HomeMapManager.getInstance().ZeroPos.z + z + +0.5f * (height) - 0.5f;

        m_plane.transform.position = tempPos;
    }

    void OnGUI()
    {
        return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        Physics.Raycast(ray, out hit, 1000.0f) ;

        if (hit.collider && hit.collider.gameObject.name.Equals("grids"))
        {
            Vector3 originPos = hit.collider.gameObject.transform.position - 0.5f * hit.collider.gameObject.transform.localScale;

            Vector3 dist = new Vector3(Mathf.Floor(hit.point.x - originPos.x) + 0.5f, hit.point.y - originPos.y + 0.15f, Mathf.Floor(hit.point.z - originPos.z) + 0.5f);

            Vector3 pos = originPos + dist;
            // 用于临时计算
            Vector3 tempPos = new Vector3();

            GUI.Label(new Rect(10, 50, 100, 20), "X:" + (pos.x - HomeMapManager.getInstance().ZeroPos.x)); ;
            GUI.Label(new Rect(10, 80, 100, 20), "Y:" + (pos.z - HomeMapManager.getInstance().ZeroPos.z)); ;
            //GUI.Label(new Rect(10, 110, 100, 20), "W:" + m_selectW);
            //GUI.Label(new Rect(10, 140, 100, 20), "H:" + m_selectH);
            //tempPos = HomeMapManager.getInstance().gridToPosition(1f, 1f);
            tempPos = HomeMapManager.getInstance().pixelPositonToGamePosition(10, 10);
            GUI.Label(new Rect(10, 170, 100, 20), "Pos:" + tempPos.x);
            GUI.Label(new Rect(10, 200, 100, 20), "Pos:" + tempPos.z);
        }

//         }
// 
//             if (Input.GetMouseButtonDown(0))
//             {
//                 m_clickPos = pos;
//                 //m_plane.transform.position = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, 2f, Mathf.Floor(hit.point.z) + 0.5f);
//             }
//             if (Input.GetMouseButton(0))
//             {
//                 Debug.Log("MouseButton");
//                 dist = pos - m_clickPos;
//                 float w = Mathf.Abs(dist.x) + 1.0f;
//                 float h = Mathf.Abs(dist.z) + 1.0f;
//                 m_plane.transform.localScale = new Vector3(0.1f * w, 0.1f, 0.1f * h);
// 
//                 tempPos = m_clickPos + 0.5f * dist;
//                 tempPos.y = originPos.y;
//                 m_plane.transform.position = tempPos;
// 
//             }
//             if (Input.GetMouseButtonUp(0))
//             {
//                 tempPos = m_plane.transform.position - m_zeroPos;
//                 Debug.Log("MouseButtonUp" + tempPos + "_" + m_plane.transform.localScale);
// 
//                 float w = Mathf.Floor(m_plane.transform.localScale.x * 10);
//                 float h = Mathf.Floor(m_plane.transform.localScale.z * 10);
//                 Vector2 p = new Vector2();
// 
//                 if (w % 2 == 0)
//                 {
//                     p.x = Mathf.Floor(tempPos.x) + Mathf.Floor(w / 2);
//                 }
//                 else
//                 {
//                     p.x = tempPos.x + Mathf.Floor(w / 2);
//                 }
// 
//                 if (h % 2 == 0)
//                 {
//                     p.y = Mathf.Floor(tempPos.z) + Mathf.Floor(h / 2);
//                 }
//                 else
//                 {
//                     p.y = tempPos.z + Mathf.Floor(h / 2);
//                 }

        
    }
}
