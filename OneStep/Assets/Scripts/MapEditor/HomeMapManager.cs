using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct RegionData
{
    public int Id;  
    public float[] Position;
    public int[] Size;
    public int[] Color ;
    public int[] Data;

    public RegionData(int id, int x, int y, int z, int width, int height, int a, int r, int g, int b)
    {
        Id = id;
        Position = new float[3] ;
        Position[0] = x;
        Position[1] = y;
        Position[2] = z;
        Size = new int[2] ;
        Size[0] = width;
        Size[1] = height;
        Color = new int[4] ;
        Color[0] = a;
        Color[1] = r;
        Color[2] = g;
        Color[3] = b;
        Data = new int[width*height] ;
    }
    public void setPosition(float x, float y, float z)
    {
        Position[0] = x;
        Position[1] = y;
        Position[2] = z;
    }

    public void setSize(int w , int h)
    {
        Size[0] = w;
        Size[1] = h;
    }

    public void setColor(int a , int r , int g, int b)
    {
        Color[0] = a;
        Color[1] = r;
        Color[2] = g;
        Color[3] = b;
    }
}

public class HomeMapManager : BaseSingleton<HomeMapManager> {
    [SerializeField]
    private GameObject m_HomeEditor = null ;

    // 与服务端的像素缩放比
    public const int GRID_PIXEL_W = 100;
    public const int GRID_PIXEL_H = 100;

    private Vector3 m_startPos = new Vector3(91f, 0.01f, 37f);

    public Vector3 StartPos
    {
        get { return m_startPos; }
        set { m_startPos = value; }
    }

    private Vector3 m_zeroPos = new Vector3(16.5f, 0.05f, -37.5f);

    public Vector3 ZeroPos
    {
        get { return m_zeroPos; }
        set { m_zeroPos = value; }
    }

    private RegionData m_region;

    internal RegionData Region
    {
        get { return m_region; }
        set { m_region = value; }
    }

    private Dictionary<int, RegionData> m_regionDict = new Dictionary<int,RegionData> ();

	HomeMapManager()
    {

    }

    ~HomeMapManager()
    {

    }
    /**
     * 创建地图编辑器
     */ 
    public void createHomeEditor()
    {
        if(m_HomeEditor == null )
        {
            m_HomeEditor = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/MapEditor/MapEditor"), Vector3.zero, Quaternion.identity) as GameObject;
            m_HomeEditor.name = "HomeEditor";
            m_HomeEditor.SetActive(false);
        }
    }

    /**
     * 地图编辑器显示开关
     */
    public void setHomeEditActive(bool b)
    {
        m_HomeEditor.SetActive(b);
    }


    public bool CreateRegion(string id , string x , string y , string width , string height )
    {
        int key = int.Parse(id);

        if (m_regionDict.ContainsKey(key))
            return false ;

        RegionData region = new RegionData();
        m_region.Id = int.Parse(id);
//         m_region.x = int.Parse(x);
//         m_region.y = int.Parse(y);
//         m_region.width = int.Parse(width);
//         m_region.height = int.Parse(height);
        m_regionDict.Add(key, region);

        return true;       
    }



    /**
     * 客户端格子转客户端位置
     */ 
    public Vector3 gridToPosition(float gridx , float gridz)
    {
        Vector3 pos = new Vector3();
        pos.x = gridx + HomeMapManager.getInstance().ZeroPos.x;
        pos.y = 0;
        pos.z = gridz + HomeMapManager.getInstance().ZeroPos.z ;
        return pos;
    }

    /**
     * 客户端位置转客户端格子
     */ 
    public Vector3 PositionToGrid(float posx , float posz)
    {
        Vector3 grid = new Vector3();
        grid.x = Mathf.Floor( posx - HomeMapManager.getInstance().ZeroPos.x );
        grid.y = 0;
        grid.z = Mathf.Floor( posz - HomeMapManager.getInstance().ZeroPos.z );
        return grid;
    }

    

    
    /**
     * 服务器逻辑位置转换成游戏内位置 
     * 
     */
    public Vector3 pixelPositonToGamePosition(float x , float y)
    {
        Vector3 pos = new Vector3();
        pos.x = Mathf.Floor(x / GRID_PIXEL_W) + (x % GRID_PIXEL_W) / 100 + HomeMapManager.getInstance().ZeroPos.x;
        pos.y = 0;
        pos.z = Mathf.Floor(y / GRID_PIXEL_H) + (y % GRID_PIXEL_H) / 100 + HomeMapManager.getInstance().ZeroPos.z;
        return pos;
    }
}
