using UnityEngine;

using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;

 
namespace IGG.CCTwo.Data
{
    public enum E_StaticDB_DBType
    {//数据类型
        TextInfo = 0,			//多语言语言包
        HeroInfo,				//英雄信息
        HeroLvlInfo,			//英雄等级信息
        HeroStar,           //英雄升星消耗
        HeroSkin,			//英雄皮肤资源
        BuildInfo,			//建筑信息
        SoldierInfo,			//士兵信息
        SoldierUnitInfo,			//士兵烘焙信息

        TechInfo,           //科技

        PropInfo,               //道具信息

        ItemInfo,               //物品信息
        ShopConfigInfo,               //商城

		InstanceInfo,			//副本信息
        ActionInfo,             //动作信息
        END					//结束
    }


    public class GameData
    {
        private static GameData instance = null;

        private Dictionary<E_StaticDB_DBType, bool> m_dictInited;
        private Dictionary<E_StaticDB_DBType, KeyValuePair<Action<string>, string>> m_dictLists;

    
        // 
        public bool IsInited { get; private set; }
            

        public static GameData Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameData();

                return instance;
            }
        }

        public ActConfig actConfig { get; protected set; }
        public HeroConfig heroConfig { get; protected set; }
        public SoldierConfig soldierConfig { get; protected set; }

        public SoldierUnitConfig soldierUnitConfig { get; protected set; }

        private GameData()
        {            
            IsInited = false;
            m_dictInited = new Dictionary<E_StaticDB_DBType, bool>();
            m_dictLists = new Dictionary<E_StaticDB_DBType, KeyValuePair<Action<string>, string>>();
            ReadResourcesFolderData();
        }       

        public void Inits()
        {//本方法没什么用,仅仅是为了在代码里更清晰指明初始化时机
            if(!IsInited)
            {                
                IsInited = true;
            }
        }
        // Reads the data from the files located in the Resources folder.
        private void ReadResourcesFolderData ()
        {
            actConfig = new ActConfig();
            registloadData(E_StaticDB_DBType.ActionInfo, actConfig.LoadFromXML, "Configuration/GameData/act/list");
            heroConfig = new HeroConfig();
            registloadData(E_StaticDB_DBType.HeroInfo, heroConfig.LoadFromXML, "Configuration/GameData/Hero");
            soldierConfig = new SoldierConfig();
            registloadData(E_StaticDB_DBType.SoldierInfo, soldierConfig.LoadFromXML, "Configuration/GameData/Soldier");

            soldierUnitConfig = new SoldierUnitConfig();
            registloadData(E_StaticDB_DBType.SoldierUnitInfo, soldierConfig.LoadFromXML, "Configuration/GameData/SoldierUnitConfig");
            IsInited = true;
        }


        
        //加载配置数据
        public bool LoadData(ref int curValue)
        {
            if (curValue >= (int)E_StaticDB_DBType.END)
                return true;
            
            if (initData((E_StaticDB_DBType)curValue))
            {
                curValue++;
                if (curValue >= (int)E_StaticDB_DBType.END)
                return true;
            }

            return false ;
	    
        }

        /// <summary>
        /// 注册加载
        /// </summary>
        /// <param name="type"></param>
        /// <param name="callBack"></param>
        /// <param name="fileName">配置文件路径 类似 "Configuration/GameData/Hero"</param>
        /// <returns></returns>
        void registloadData(E_StaticDB_DBType type, Action<string> callBack, string fileName)
        {
            
            m_dictInited[type] = false;
            TextAsset text = Resources.Load(fileName) as TextAsset;
            string str = text != null ? text.ToString() : string.Empty;
            m_dictLists[type] = new KeyValuePair<Action<string>, string>(callBack, str);
        }
        
        internal bool initData( E_StaticDB_DBType type)
        {
            if (m_dictInited[type])
                return true;

            m_dictInited[type] = loadData(type);

            return m_dictInited[type];
        }

        bool loadData(E_StaticDB_DBType type)
        {
            KeyValuePair<Action<string>, string> keyValue = m_dictLists[type];
            if (string.IsNullOrEmpty(keyValue.Value))
                return false;
            else
                keyValue.Key(keyValue.Value);            
            return true;
        }

        //本方法提供给非游戏环境场合用
        public void LoadAllData()
        {
            Array ary = Enum.GetValues(typeof(E_StaticDB_DBType));

            foreach (E_StaticDB_DBType myCode in ary)
            {
                if (myCode != E_StaticDB_DBType.END)
                {
                    initData(myCode);
                }
            }         
        }
    }
}
