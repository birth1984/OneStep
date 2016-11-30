using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System;

public class NodeInfo
{
    public int BaseId;
    public byte Level;
    public byte Type;
    public string FileName;
}

public class NodeManager
{
    private static NodeManager m_instance;

    private Dictionary<int, NodeInfo> m_nodeList;

    private NodeManager()
    {
        initNodeData();
    }

    public static NodeManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new NodeManager();
            return m_instance;
        }
    }

    public INode GetNode(ushort nodeId)
    {
        return new INode();
    }

    private void initNodeData()
    {
        m_nodeList = new Dictionary<int, NodeInfo>();

        TextAsset text = Resources.Load("Configuration/GameData/node/NodeList") as TextAsset;
        string pXmlString = text != null ? text.ToString() : string.Empty;

        XmlDocument xDoc = new XmlDocument();

        xDoc.LoadXml(pXmlString);
        XmlElement parent = xDoc.DocumentElement;
        XmlNodeList nodeList = parent.ChildNodes;
        foreach (XmlNode node in nodeList)
        {
            int bId = Convert.ToUInt16((node as XmlElement).GetAttribute("Id"));

            foreach (XmlNode levelNode in node.ChildNodes)
            {
                NodeInfo pInfo = new NodeInfo();
                pInfo.BaseId = bId;
                pInfo.Level = Convert.ToByte((levelNode as XmlElement).GetAttribute("Id"));
                pInfo.Type = Convert.ToByte((levelNode as XmlElement).GetAttribute("Type"));
                pInfo.FileName = (levelNode as XmlElement).GetAttribute("FileName");
                int nodeId = 100000 * (pInfo.Level - 1) + bId;
                m_nodeList.Add(nodeId, pInfo);
            }
        }
    }

    private NodeInfo getNodeInfo(ushort Id,byte level)
    {
        int nodeId = 100000 * (level - 1) + Id;
        if (m_nodeList.ContainsKey(nodeId))
            return m_nodeList[nodeId];
        return null;
    }

    public INode GetNode(ushort Id,byte level)
    {
        NodeInfo pInfo = getNodeInfo(Id, level);
        if (pInfo == null)
            return null;
        INode pNode = null;
        switch(pInfo.Type)
        {
            case 0://动画
                pNode = new ActionNode(Id);
                break;
            case 1://
                break;
            case 2:
                break;
            case 3://人物
                if (Id > 10000)
                    pNode = new Hero(1,1);
                else
                    pNode = new Soldier(1,1);
                break;
            case 4://飞行物
                break;
            case 5:
                break;
        }
        return new INode();
    }
}
