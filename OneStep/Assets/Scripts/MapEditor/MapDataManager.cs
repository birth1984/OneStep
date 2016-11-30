using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Linq;
using Newtonsoft.Json;
#if UNITY_EDITOR 
using UnityEditor ;
#endif
public class MapDataManager : BaseSingleton<MapDataManager> {

    private Byte[] m_mapByteData = new Byte[150 * 150];
    
    public Byte[] MapByteData
    {
        get { return m_mapByteData; }
        set { m_mapByteData = value; }
    }

    private char[] m_mapCharData = new char[150 * 150];

    public char[] MapCharData
    {
        get { return m_mapCharData; }
        set { m_mapCharData = value; }
    }


    private List<Vector2> m_gridDatas = new List<Vector2>();
    public List<Vector2> GridDatas
    {
        get { return m_gridDatas; }
        set { m_gridDatas = value; }
    }
    MapDataManager()
    {
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

    ~MapDataManager()
    {

    }


    /**
     * path：文件创建目录
     * name：文件的名称
     * info：写入的内容
     */
    public void CreateFile(string path, string name, string info)
    {
        //文件流信息
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.CreateText();
        }
        else
        {
            //如果此文件存在则打开
            sw = t.AppendText();
        }
        //以行的形式写入信息
        sw.WriteLine(info);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }

    /**
    * path：读取文件的路径
    * name：读取文件的名称
    */
    public ArrayList LoadFile(string path, string name)
    {
        //使用流的形式读取
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            //路径与名称未找到文件则直接返回空
            return null;
        }
        string line;
        ArrayList arrlist = new ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            //一行一行的读取
            //将每一行的内容存入数组链表容器中
            arrlist.Add(line);
        }
        //关闭流
        sr.Close();
        //销毁流
        sr.Dispose();
        //将数组链表容器返回
        return arrlist;
    }

    /**
    * path：删除文件的路径
    * name：删除文件的名称
    */

    public void DeleteFile(string path, string name)
    {
        File.Delete(path + "//" + name);
    }

    public void SaveMap()
    {
        DeleteFile(Application.persistentDataPath, "FileName.txt"); 
        Vector2 point = new Vector2();
        int index;
        for (int i = 0; i < m_mapCharData.Length; i++ )
        {
            if (!m_mapCharData[i].Equals('1'))
                m_mapCharData[i] = '0';
        }
//         for (int i = 0; i < m_gridDatas.Count; i++)
//         {
//             point = m_gridDatas[i];
//             index = (int)(point.y * 150 + point.x);
//             m_mapByteData[index] = '1';
//         }
        //Debug.Log("Count:" + datas.Count + datas.ToString());
        //byte[] tempbyte = new byte[] { 0, 0, 1, 0, 1 };
        //char[] tempchar = new char[]{ '0', '0', '0', '0', '1', '1' }; //Encoding.ASCII.GetChars(m_mapByteData);
        string tempstring = new string(m_mapCharData); // new string(tempchar);// Encoding.UTF8.GetString(m_mapByteData);
        CreateFile(Application.persistentDataPath, "FileName.txt", "" + tempstring);
    }



    public void SaveJsonData()
    {
        string path = "Assets/Resources/Configuration/GameData/Map/GridInfo.txt";
        
        //List<GridInfo> list = new List<GridInfo>();

//         for (int i = 0; i < m_grids.Count; i++)
//         {
//             list.Add(m_grids[i].Value);
//         }

        RegionData rdata = new RegionData();
        rdata.Id = 1;
        //         rdata.x = 47;
        //         rdata.y = 62;
        //         rdata.width = 82;
        //         rdata.height = 82;
        string info = "";// JsonConvert.SerializeObject(rdata);

        StreamWriter streamWriter = new StreamWriter(path);

        streamWriter.Write("");

        streamWriter.Write(info);

        streamWriter.Close();

        AssetDatabase.Refresh();
    }


    private XmlDocument xmlDoc = new XmlDocument();
    /**
     * 增加一条Region数据
     */
    public void InsertRegionXML(string id , string name , string x , string y , string width , string height)
    {       
        xmlDoc.Load("Assets/Resources/Configuration/GameData/Map/HomeRegion.xml");
        XmlNode root = xmlDoc.SelectSingleNode("config");
        XmlElement region = xmlDoc.CreateElement("Region");
        
        region.SetAttribute("ID", id);
        region.SetAttribute("Name", name);
        region.SetAttribute("X", x);
        region.SetAttribute("Y", y);
        region.SetAttribute("Width", width);
        region.SetAttribute("Height", height);

        root.AppendChild(region);
        xmlDoc.Save("Assets/Resources/Configuration/GameData/Map/HomeRegion.xml");
    }

    public void UpdateRegionXML(string id , string name , string x , string y , string width , string height)
    {
        xmlDoc =new XmlDocument();
        xmlDoc.Load("Assets/Resources/Configuration/GameData/Map/HomeRegion.xml"); 

        XmlNodeList nodeList = xmlDoc.SelectSingleNode("Regions").ChildNodes;

        //遍历所有子节点 
        XmlNode node;
        //foreach (XmlNode node in nodeList)
        for(int i=0 ; i<nodeList.Count ; i++)
        {
            XmlElement region = (XmlElement)nodeList[i];

            Debug.Log(region.GetAttribute("ID").ToString() );                  

            if (region.GetAttribute("ID") == id)//如果genre属性值为“Sky_Kwolf” 
            {
                region.SetAttribute("Name" , name) ;
                break;
//                 region.SetAttribute("X", x);
//                 region.SetAttribute("Y", y);
//                 region.SetAttribute("Width", width);
//                 region.SetAttribute("Height", height);

                //继续获取xe子节点的所有子节点 
//                XmlNodeList regionChildren = region.ChildNodes;

//                 foreach (XmlNode child in regionChildren)//遍历 
//                 {
//                     XmlElement xe = (XmlElement)child; //转换类型
//                     if (xe.Name =="author") 
//                     {
//                         xe.InnerText ="jason";//则修改 
//                         break;//找到退出 
//                     }
//                 }
//                break;
            }            
        }
        xmlDoc.Save("Assets/Resources/Configuration/GameData/Map/HomeRegion.xml");
    }
}
