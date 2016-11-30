using UnityEngine;
using System.Collections;
using System;

public class MapManager
{
    private static MapManager m_instance = null;

    public const int MAP_CELL_ROW = 82;
    public const int MAP_CELL_COL = 82;
    private static float m_cellWidth = 1.0f;
    private static float m_cellHeight = 1.0f;

    private byte[] m_tileData;

    private MapInfo m_currentMapInfo;
    private BaseMap m_currentMap;
    private bool _bIsSelfMap = true;

    private bool _isDraging = false;

    private MapManager()
    {
        m_tileData = new byte[MAP_CELL_ROW * MAP_CELL_COL];
        //Array.Clear(m_tileData, 0, m_tileData.Length);
    }

    public static MapManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new MapManager();
            }
            return m_instance;
        }
    }

    public MapInfo CurrentMapInfo
    {
        get
        {
            return m_currentMapInfo;
        }
        set
        {
            m_currentMapInfo = value;
        }
    }

    public BaseMap CurrentMap
    {
        get
        {
            return m_currentMap;
        }
        set
        {
            m_currentMap = value;
        }
    }

    public bool IsDragingNode
    {
        get
        {
            return _isDraging;
        }
        set
        {
            _isDraging = value;
        }
    }

    public bool IsSelfMap()
    {
        return _bIsSelfMap;
    }

    /**
	* 获取某一位置阻挡信息
	*/
    public int GetTileType(int cellX, int cellY)
    {
        if (cellY * m_currentMapInfo.CellX + cellX > m_tileData.Length || cellY * m_currentMapInfo.CellX + cellX < 0)
        {
            return 1;
        }
        else
        {
            return m_tileData[cellY * m_currentMapInfo.CellX + cellX];
        }
    }

    public byte[] TileData
    {
        get { return m_tileData; }
    }

    public Vector3 GetCellPosition(Vector3 pixelPos)
    {
        float cellX = Mathf.Floor(pixelPos.x / m_cellWidth);
        float cellZ = Mathf.Floor(pixelPos.z / m_cellHeight);
        return new Vector3(cellX, 0, cellZ);
    }

    public Vector3 GetPixelPosition(Vector3 cellPos)
    {
        float pixelX = (2 * cellPos.x + 1) * m_cellWidth / 2.0f;
        float pixelZ = (2 * cellPos.y + 1) * m_cellHeight / 2.0f;
        return new Vector3(pixelX, 0, pixelZ);
    }
    //根据格子坐标得到地图坐标。
   /* public Vector3 GetMapPositionByCell(int AreaId, int CellX, int CellY, bool Center = true)
    {
        bool flag = CellX < 0;
        if (flag)
        {
            CellX = -CellX;
            CellY = -CellY;
        }
        AreaInfo aInfo = _currentMapInfo.AreaList[AreaId] as AreaInfo;
        float p1 = (float)CellX + aInfo.HalfCellX;
        float p2 = (float)CellY - aInfo.HalfCellY;

        float x1 = (p1 - p2) * aInfo.HalfCellWidth;
        float y1 = (p1 + p2) * aInfo.HalfCellHeight;

        x1 = x1 + aInfo.TopLeftX;

        y1 = aInfo.TopLeftY - y1;
        if (Center) //计算格子中心点坐标。
        {
            y1 -= aInfo.HalfCellHeight;
        }
        if (flag)
        {
            return new Vector3(-x1, -y1, 0);
        }
        else
        {
            return new Vector3(x1, y1, 0);
        }
    }

    //根据地图象素坐标取得区域格子坐标。
    public Vector3 GetCellPosition(int x, int y, int AreaId = 0, bool adjust = false)
    {
        foreach (AreaInfo aInfo in _currentMapInfo.AreaList.Values)
        {

            float AreaX = x - aInfo.TopLeftX;
            float AreaY = aInfo.TopLeftY - y;

            float x1 = AreaX / aInfo.CellWidth;
            float y1 = AreaY / aInfo.CellHeight;

            float CellX = x1 + y1 - aInfo.HalfCellX;
            float CellY = y1 - x1 + aInfo.HalfCellY;

            if (CellX > 0 && CellX < aInfo.CellX && CellY > 0 && CellY < aInfo.CellY)
            {
                if (AreaId != 0)
                    AreaId = aInfo.Id;
                return new Vector3((int)CellX, (int)CellY, 0);
            }
            else if (CellX < 0 || CellY < 0 || CellX >= aInfo.CellX || CellY >= aInfo.CellY)
            {
                if (AreaId != 0)
                    AreaId = aInfo.Id;
                return new Vector3(Mathf.Floor(CellX), Mathf.Floor(CellY), 0);
            }
            else if (adjust && AreaId != 0 && AreaId == aInfo.Id) //在相应区域内找最近的格子.
            {
                if (CellX >= aInfo.CellX)
                    CellX = aInfo.CellX - 1;
                else if (CellX < 0)
                    CellX = 0;

                if (CellY >= aInfo.CellY)
                    CellY = aInfo.CellY - 1;
                else if (CellY < 0)
                    CellY = 0;
                return new Vector3((int)-CellX, (int)-CellY, 0);
            }
        }
        if (AreaId != 0) AreaId = -1;
        return new Vector3(-1, -1, 0);
    }*/
}
