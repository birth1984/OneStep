using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using IGG.CCTwo.Data;

public class T_HeroInfo
{
    public int Id = 0;
    public string Name = "";
    public int NameId =0;

    public List<T_HeroStartInfo> pHeroStartInfo = new List<T_HeroStartInfo>();
}

public class T_HeroStartInfo
{
    public int Star = 0;
    public int NeedLvl = 0;
    public int Attack = 0;
    public int HP = 0;
    public int MP = 0;
    public int Attack_Speed = 0;
    public int Move_Speed = 0;
    public int Attack_Favorite = 0;
    public int Tenacity = 0;
}

public class T_HeroStarNeed
{
    public int Star = 0;
    public int Need_Num = 0;
    public int CostMoney = 0;
}


    public class T_HeroExpInfo
{//英雄的等级静态数据
    public int pLevel = 0;
    public uint pExp = 0;
    
}

public class T_HeroSkin
{
    public int Id = 0;
    public string Icon_s = "";
    public string Icon_a = "";
    public string Skeleton = "";
    public int Sound = 0;
    public int SoundDelay = 0;
    public float HeroX = 0;
    public float HeroY = 0;
    public float ScaleX = 0;
    public float ScaleY = 0;
}

public class HeroConfig
{
    private int m_heroMaxLv;
    private List<T_HeroInfo> _heroInfoList = new List<T_HeroInfo>();
    private List<T_HeroExpInfo> _pHeroExpInfoList = new List<T_HeroExpInfo>();

    private List<T_HeroStarNeed> _heroStarInfoList = new List<T_HeroStarNeed>();

    private List<T_HeroSkin> _heroSkinList = new List<T_HeroSkin>();

    public void LoadFromXML(string pXmlString)
    {//英雄配置
        XmlDocument xDoc = new XmlDocument();

        xDoc.LoadXml(pXmlString);
        XmlElement parent = xDoc.DocumentElement;
        XmlNodeList nodeList = parent.ChildNodes;

        foreach (XmlElement pHero in nodeList)
        {
            T_HeroInfo pHeroInfo = new T_HeroInfo();
            _heroInfoList.Add(pHeroInfo);

            pHeroInfo.Id = XmlHelper.ReadIntegerAttribute(pHero, "Id");
            pHeroInfo.Name = XmlHelper.ReadStringAttribute(pHero, "Name");
          //  pHeroInfo.NameId = XmlHelper.ReadIntegerAttribute(pHero, "NameId");

            //XmlNodeList pStarList = pHero.SelectSingleNode("Star").ChildNodes;
            XmlNodeList pStarList = pHero.ChildNodes;
            foreach (XmlElement pStart in pStarList)
            {
                T_HeroStartInfo pHeroStartInfo = new T_HeroStartInfo();
                pHeroInfo.pHeroStartInfo.Add(pHeroStartInfo);

                pHeroStartInfo.Star = XmlHelper.ReadIntegerAttribute(pStart, "Star");
                pHeroStartInfo.NeedLvl = XmlHelper.ReadIntegerAttribute(pStart, "NeedLvl");

                pHeroStartInfo.Attack = XmlHelper.ReadIntegerAttribute(pStart, "Attack");
                pHeroStartInfo.HP = XmlHelper.ReadIntegerAttribute(pStart, "HP");
                pHeroStartInfo.MP = XmlHelper.ReadIntegerAttribute(pStart, "MP");
                pHeroStartInfo.Attack_Speed = XmlHelper.ReadIntegerAttribute(pStart, "Attack_Speed");
                pHeroStartInfo.Move_Speed = XmlHelper.ReadIntegerAttribute(pStart, "Move_Speed");
                pHeroStartInfo.Attack_Favorite = XmlHelper.ReadIntegerAttribute(pStart, "Attack_Favorite");
                pHeroStartInfo.Tenacity = XmlHelper.ReadIntegerAttribute(pStart, "Tenacity");
            }
             
        }
    }

    public void LoadLevelInfo(string pXmlString)
    {//等级配置
        XmlDocument xDoc = new XmlDocument();

        xDoc.LoadXml(pXmlString);
        XmlElement parent = xDoc.DocumentElement;
        XmlNodeList nodeList = parent.ChildNodes;
        m_heroMaxLv = XmlHelper.ReadIntegerAttribute(parent,"MaxLv");

        foreach (XmlElement pLvl in nodeList)
        {
            T_HeroExpInfo pHeroExpInfo = new T_HeroExpInfo();
            pHeroExpInfo.pLevel = XmlHelper.ReadIntegerAttribute(pLvl, "Level");
            pHeroExpInfo.pExp = XmlHelper.ReadUintegerAttribute(pLvl, "Exp",0);
            _pHeroExpInfoList.Add(pHeroExpInfo);
        }
    }

    public void LoadHeroStar(string pXmlString)
    {//英雄升星消耗
        XmlDocument xDoc = new XmlDocument();

        xDoc.LoadXml(pXmlString);
        XmlElement parent = xDoc.DocumentElement;
        XmlNodeList nodeList = parent.ChildNodes;

        foreach (XmlElement pHero in nodeList)
        {
            T_HeroStarNeed pHeroStarNeed = new T_HeroStarNeed();

            _heroStarInfoList.Add(pHeroStarNeed);

            pHeroStarNeed.Star = XmlHelper.ReadIntegerAttribute(pHero, "Star");
            pHeroStarNeed.Need_Num = XmlHelper.ReadIntegerAttribute(pHero, "NeedNum");
            pHeroStarNeed.CostMoney = XmlHelper.ReadIntegerAttribute(pHero, "CostMoney");

        }
    }


    public void LoadHeroSkin(string pXmlString)
    {//英雄皮肤资源配置

        XmlDocument xDoc = new XmlDocument();

        xDoc.LoadXml(pXmlString);
        XmlElement parent = xDoc.DocumentElement;
        XmlNodeList nodeList = parent.ChildNodes;

        foreach (XmlElement pNode in nodeList)
        {
            T_HeroSkin pHeroSkin = new T_HeroSkin();
            _heroSkinList.Add(pHeroSkin);

            pHeroSkin.Id = XmlHelper.ReadIntegerAttribute(pNode, "Id");
            pHeroSkin.Icon_s = XmlHelper.ReadStringAttribute(pNode, "Icon_s");
            pHeroSkin.Icon_a = XmlHelper.ReadStringAttribute(pNode, "Icon_a");
            pHeroSkin.Skeleton = XmlHelper.ReadStringAttribute(pNode, "Skeleton");
            pHeroSkin.Sound = XmlHelper.ReadIntegerAttribute(pNode, "Sound");
            pHeroSkin.SoundDelay = XmlHelper.ReadIntegerAttribute(pNode, "SoundDelay");
            pHeroSkin.HeroX = XmlHelper.ReadFloatAttribute(pNode, "HeroX");
            pHeroSkin.HeroY = XmlHelper.ReadFloatAttribute(pNode, "HeroY");
            pHeroSkin.ScaleX = XmlHelper.ReadFloatAttribute(pNode, "ScaleX");
            pHeroSkin.ScaleY = XmlHelper.ReadFloatAttribute(pNode, "ScaleY");
        }
    }


    public List<T_HeroInfo> GetHeroInfoList()
    {
        return _heroInfoList;
    }

    public T_HeroInfo GetHeroInfoById(int pId)
    {
        GameData.Instance.initData(E_StaticDB_DBType.HeroInfo);

        foreach (T_HeroInfo pHeroInfo in _heroInfoList)
        {
            if(pId== pHeroInfo.Id)
            {
                return pHeroInfo;
            }
        }
        return null;
    }

    public T_HeroStartInfo GetHeroStarInfoByIdAndLvl(int pId,int pStar)
    {
        GameData.Instance.initData(E_StaticDB_DBType.HeroInfo);

        T_HeroInfo pHeroInfo = GetHeroInfoById(pId);
        if (pHeroInfo == null)
        {
            return null;
        }

        foreach (T_HeroStartInfo pHeroStartInfo in pHeroInfo.pHeroStartInfo)
        {
            if (pStar == pHeroStartInfo.Star)
            {
                return pHeroStartInfo;
            }
        }

        return null;
    }

    public uint GetHeroUpgradeExp(int pHeroId, int pLvl)
    {//获取英雄升级需要的经验
        if (_pHeroExpInfoList.Count == 0) return 0;
        if (pLvl == 0) return 0;//英雄是从零级开始
        T_HeroInfo  pHeroInfo = GetHeroInfoById(pHeroId);
        if (pHeroInfo.Id == 0) return 0;
        if (_pHeroExpInfoList.Count > pLvl)
        {
            return _pHeroExpInfoList[pLvl - 1].pExp;
        }
        return 0;
    }

    public uint GetHeroNowExp(int pHeroId, int pLvl, uint exp)
    {//获取英雄现在的经验
        uint pExp = 0, maxExp = 0;//当前经验值,最大经验值
        maxExp = GetHeroUpgradeExp(pHeroId, pLvl);
        pExp = exp;
	    if(pLvl > 1 )
	    {//英雄已经大于一级了
		    if(pLvl == m_heroMaxLv )
		    //if( maxExp == 0 )
		    {//表示已经达到最大等级了
			    maxExp = GetHeroUpgradeExp(pHeroId, pLvl - 1 );
                maxExp -= GetHeroUpgradeExp(pHeroId, pLvl - 2 );
                pExp = maxExp;
		    }else
		    {
			    maxExp -= GetHeroUpgradeExp(pHeroId, pLvl - 1 );//最高经验要等于当前最高经验减去上一最高经验
                pExp -= GetHeroUpgradeExp(pHeroId, pLvl - 1 );//当前经验要等于服务端发下来总经验减去上一最高经验
		    }
	    }
	    return pExp;
    }

    public uint GetHeroMaxExp(int pHeroId, int pLvl, uint exp)
    {//获取英雄当前等级最高经验

        uint pExp = 0, maxExp = 0;//当前经验值,最大经验值
        maxExp = GetHeroUpgradeExp(pHeroId, pLvl);
        pExp = exp;
	    if(pLvl > 1 )
	    {//英雄已经大于一级了
		    if(pLvl == m_heroMaxLv  )
		    {//表示已经达到最大等级了
			    maxExp = GetHeroUpgradeExp(pHeroId, pLvl - 1 );
                maxExp -= GetHeroUpgradeExp(pHeroId, pLvl - 2 );
                pExp = maxExp;
		    }else
		    {
			    maxExp -= GetHeroUpgradeExp(pHeroId, pLvl - 1 );
                pExp -= GetHeroUpgradeExp(pHeroId, pLvl - 1 );
		    }
	    }
	    return maxExp;
    }


    public int GetHeroLevel(int pHeroId, uint exp)
    {//获取英雄等级
        GameData.Instance.initData(E_StaticDB_DBType.HeroInfo);

        T_HeroInfo pHeroInfo = GetHeroInfoById(pHeroId);
        if (pHeroInfo == null) return 0;

        int lvl = 1;//英雄等级从1级开始
        for (int i = 0; i < _pHeroExpInfoList.Count; i++)
        {
            if (_pHeroExpInfoList[i].pExp == 0) continue;//不可能存在 0
             uint _exp = _pHeroExpInfoList[i].pExp;
            if (exp >= _exp)
            {
                lvl++;

                lvl =  Mathf.Min(m_heroMaxLv, lvl);
             }
	    }
	    return lvl;
    }

    public T_HeroStarNeed GetHeroCostNeed(int pStar)
    {//获取英雄升星消耗
        GameData.Instance.initData(E_StaticDB_DBType.HeroStar);

        foreach (T_HeroStarNeed pHeroInfo in _heroStarInfoList)
        {
            if (pStar == pHeroInfo.Star)
            {
                return pHeroInfo;
            }                 
        }
        return null;
    }

    public T_HeroSkin GetHeroSkinById(int pId)
    {
        GameData.Instance.initData(E_StaticDB_DBType.HeroSkin);

        foreach (T_HeroSkin pHeroSkin in _heroSkinList)
        {
            if (pHeroSkin.Id == pId)
            {
                return pHeroSkin;
            }
        }

        return null;
    }
}



