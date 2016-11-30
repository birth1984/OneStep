using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using IGG.CCTwo.Data;
using System.IO;
using System;
using System.Text;

public class SoldierInfo
{
    public int Id = 0;
    public string Name = "";
    public int Space = 0;
    public int NameId = 0;
    public int OccupationType = 0;
    public int Attack_Type = 0;
    public int Defense_Type = 0;
    public int Range = 0;
    public int Attack_Speed = 0;
    public int Move_Speed = 0;
    public int Attack_Favorite = 0;
    public int Range_Target = 0;
    public int Attack_Target = 0;
    public string ModelID = "";
    public string Icon = "";
    public string Icon_s = "";
    public int Move_Type = 0;
    public int Death_Sound = 0;
    public int Attack_Sound = 0;
    public int Birth_Sound = 0;
    public int Damage_Reduce = 0;
    public int Frame = 0;
    public string NeedLvl = "";
    public string ResearchId = "";
    public Hashtable allLvHT = new Hashtable();
}

public class SoldierLvInfo
{
    public int Id = 0;
    public int BuildTime = 0;
}

public class SoldierConfig
{
    public List<SoldierInfo> SoldierXmlList = new List<SoldierInfo>();
    private Dictionary<int, SoldierInfo> _soldierList = new Dictionary<int, SoldierInfo>();
    public void LoadFromXML(string text)
    {

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(text);

        //XmlNodeList nodeList = xmlDoc.SelectSingleNode("Soldier").ChildNodes;
        XmlNodeList nodeList = xmlDoc.DocumentElement.ChildNodes;
        foreach (XmlElement pSoldier in nodeList)
        {
            SoldierInfo pSoldierInfo = new SoldierInfo();


            pSoldierInfo.Id = XmlConvert.ToInt32(pSoldier.GetAttribute("Id"));
            pSoldierInfo.Name = pSoldier.GetAttribute("Name");

            pSoldierInfo.NameId = pSoldier.HasAttribute("NameId") ? XmlConvert.ToInt32(pSoldier.GetAttribute("NameId")) : 0;
            pSoldierInfo.Space = XmlConvert.ToInt32(pSoldier.GetAttribute("Space"));
            pSoldierInfo.NeedLvl = pSoldier.GetAttribute("NeedLvl");
            pSoldierInfo.ResearchId = pSoldier.GetAttribute("ResearchId");

            XmlNodeList lvList = pSoldier.ChildNodes;
            foreach (XmlElement pLv in lvList)
            {
                SoldierLvInfo pSoldierLvInfo = new SoldierLvInfo();
                pSoldierLvInfo.Id = XmlConvert.ToInt32(pLv.GetAttribute("Lvl"));
                pSoldierLvInfo.BuildTime = XmlConvert.ToInt32(pLv.GetAttribute("BuildTime"));

                pSoldierInfo.allLvHT.Add(pSoldierLvInfo.Id, pSoldierLvInfo);
            }
            SoldierXmlList.Add(pSoldierInfo);
            _soldierList.Add(pSoldierInfo.Id, pSoldierInfo);
        }
    }

    public Dictionary<int, SoldierInfo> SoldierList()
    {
        return _soldierList;
    }

    public SoldierInfo GetSoldierById(int soldierId)
    {
        if (_soldierList.ContainsKey(soldierId))
            return _soldierList[soldierId];
        else
        {
            //IGG.Debug.Fail("SoldierInfo:GetSoldierById  not exists id:" + soldierId);
            return null;
        }
    }
}

