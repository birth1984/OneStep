using UnityEngine;
using UnityEngine.EventSystems;
using System.Xml;
using System;
using System.Collections.Generic;

public class BaseMap : MonoBehaviour//, IPointerClickHandler
{
    protected float currentMapScale;
    protected GameObject line;
    protected GameObject mapBlock;
    protected GameObject block;
    protected float blockWidth = 512.0f;
    protected float blockHeight = 512.0f;
    protected MapInfo _aMapInfo;
    private const int RendomCell = 4;

    protected bool _gridVisible = false;
    protected int _mapType;
    public bool GridVisible
    {
        get
        {
            return _gridVisible;
        }
        set
        {
            _gridVisible = value;
            gridUpdate();
        }
    }

    protected bool _blockVisible = false;
    public bool BlockVisible
    {
        get
        {
            return _blockVisible;
        }
        set
        {
            _blockVisible = value;
        }
    }

    // Use this for initialization
    virtual public void Start () {
        /*_aMapInfo = new MapInfo();
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(Resources.Load<TextAsset>("Prefabs/Map/Map" + _mapType.ToString() ).text);    //加载Xml文件  
        XmlElement rootElem = doc.DocumentElement;   //获取根节点  
        XmlNodeList imageNodes = rootElem.GetElementsByTagName("Images"); //获取Images子节点集合  
        foreach (XmlNode node in imageNodes)
        {
            XmlNodeList childNodes = ((XmlElement)node).GetElementsByTagName("Image");
            foreach (XmlNode child in childNodes)
            {
                _aMapInfo.mapWidth = Convert.ToInt16(((XmlElement)child).GetAttribute("MapWidth"));
                _aMapInfo.mapHeight = Convert.ToInt16(((XmlElement)child).GetAttribute("MapHeigth"));
            }
        }*/
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    //     public void OnPointerClick(PointerEventData eventData)
    //     {
    //         Vector3 clickPos;
    //         bool b = RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out clickPos);
    //         IGG.Debug.Log("change result:" + b + " clickPos:" + clickPos);
    // 
    //         int areaId = 0;
    //         Vector3 cellPos = GetCellPosition((int)clickPos.x, (int)clickPos.y, ref areaId);
    //         IGG.Debug.Log("Cell Position:(" + (int)cellPos.x + ", " + (int)cellPos.y + ")");
    //         Vector3 pixelPos = GetMapPositionByCell(areaId, (int)cellPos.x, (int)cellPos.y, true);
    //         IGG.Debug.Log("Pixel Position:" + pixelPos);
    //    
    public void OnMouseClickHandle()
    {
        Vector3 clickPos;
        clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPos = transform.InverseTransformPoint(clickPos);
        Vector3 cellPos = MapManager.Instance.GetCellPosition(clickPos);
        //Debug.Log("Cell Position:(" + (int)cellPos.x + ", " + (int)cellPos.y + ", " + (int)cellPos.z + ")");
        Vector3 pixelPos = MapManager.Instance.GetPixelPosition(cellPos);
        //Debug.Log("Pixel Position:" + pixelPos);
    }

    protected void gridUpdate()
    {
        Transform tran = transform.FindChild("GridLayer");
        if (_gridVisible)
        {
            short cellX = MapManager.Instance.CurrentMapInfo.CellX;
            short cellZ = MapManager.Instance.CurrentMapInfo.CellZ;
            float cellWidth = MapManager.Instance.CurrentMapInfo.CellWidth;
            float cellHeight = MapManager.Instance.CurrentMapInfo.CellHeight;
            float posX = 1000;// (MapManager.Instance.CurrentMapInfo.PositionX;
            float posY = 1000;// (MapManager.Instance.CurrentMapInfo.PositionY;
            float length = Mathf.Sqrt(Mathf.Pow(cellWidth * cellX / 2, 2.0f) + Mathf.Pow(cellHeight * cellZ / 2, 2.0f));
            float angle = Mathf.Atan(cellHeight / cellWidth) * 180 / Mathf.PI;

            for (int i = 0; i <= cellX; i++)
            {
                Vector3 startPos = new Vector3(posX - cellWidth * cellX / 2 + cellWidth / 2 * i, posY - cellHeight / 2 * i, 0);
                GameObject go = (GameObject)Instantiate(line, transform.TransformPoint(startPos), Quaternion.identity);
                go.transform.SetParent(tran);
                go.transform.localScale = new Vector3(length / 3, 1, 1);
                go.transform.Rotate(new Vector3(0, 0, angle));
                
                //go.transform.position = startPos;
                //yield return null;
            }

            for (int j = 0; j <= cellZ; j++)
            {
                Vector3 startPos = new Vector3(posX - cellWidth * cellX / 2 + cellWidth / 2 * j, posY + cellHeight / 2 * j, 0);
                GameObject go = (GameObject)Instantiate(line, transform.TransformPoint(startPos), Quaternion.identity);
                go.transform.SetParent(tran);
                go.transform.localScale = new Vector3(length / 3, 1, 1);
                go.transform.Rotate(new Vector3(0, 0, 360 - angle));
                //go.transform.position = startPos;
                //yield return null;
            }
        }
        else
        {
            Transform[] objects = tran.GetComponentsInChildren<Transform>();
            foreach (var go in objects)
            {
                if(go.CompareTag("TileLine"))
                {
                    Destroy(go.gameObject);
                }
            }
        }
    }
}
