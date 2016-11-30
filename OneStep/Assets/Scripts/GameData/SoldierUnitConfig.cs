using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using IGG.CCTwo.Data;

namespace IGG.CCTwo.Data
{
       
    public class T_SoldierUnit
    {
        public int Id = 0;
        public int MeshCacheId = 0;        
        public string PrefabName = "";
        public string FolderName = "";
        public string AnimationName = "";

    }

    public class SoldierUnitConfig
    {

        //动作定义
        public static string ANMI_NAME_IDLE = "Idle";
        public static string ANMI_NAME_ATTACK = "Attack";
        public static string ANMI_NAME_SKILL = "SKill";
        public static string ANMI_NAME_RUN = "Run";
        public static string ANMI_NAME_RESPONSE = "Response";
        


        public List<T_SoldierUnit> ListData;

        public bool IsLoaded
        {
            get;
            private set;
        }

        public SoldierUnitConfig()
        {
            ListData = new List<T_SoldierUnit>();
            IsLoaded = false;

        }

        public void LoadFromXML(string pXmlString)
        {//英雄配置
            XmlDocument xDoc = new XmlDocument();

            xDoc.LoadXml(pXmlString);
            XmlElement parent = xDoc.DocumentElement;
            XmlNodeList nodeList = parent.ChildNodes;
                        
            foreach (XmlElement unit in nodeList)
            {
                T_SoldierUnit unitInfo = new T_SoldierUnit();

                unitInfo.Id = XmlHelper.ReadIntegerAttribute(unit, "ID");
                unitInfo.PrefabName = XmlHelper.ReadStringAttribute(unit, "PrefabName");
                unitInfo.AnimationName = XmlHelper.ReadStringAttribute(unit, "AnimationName");
                unitInfo.FolderName = XmlHelper.ReadStringAttribute(unit, "FolderName");
                unitInfo.MeshCacheId = XmlHelper.ReadIntegerAttribute(unit, "MeshCacheId");
                ListData.Add(unitInfo);
                

            }

            IsLoaded = true;
            
        }


        //获取用于识别动作的id
        public static int GetSoldierAnimIndex(int soliderId, E_NodeStatus status)
        {
            return SoldierUnitConfig.GetSoldierAnimIndex(soliderId, GetSoldierAnimNameByStatus(status));
        }

                
        public static int GetSoldierAnimIndex(int soliderId, string aniName)
        {
            int baseNum = 100;

            if (aniName.EndsWith(ANMI_NAME_IDLE))
                return soliderId * baseNum;
            else if (aniName.EndsWith(ANMI_NAME_ATTACK))            
                return soliderId * baseNum + 1;
            else if (aniName.EndsWith(ANMI_NAME_SKILL))                                    
                return soliderId * baseNum + 2;
            else if (aniName.EndsWith(ANMI_NAME_RUN))                                    
                return soliderId * baseNum + 3;
            else if (aniName.EndsWith(ANMI_NAME_RESPONSE))                        
                return soliderId * baseNum + 4;

            return 0;
        }


        public static string GetSoldierAnimNameByStatus(E_NodeStatus status)
        {

            if (status == E_NodeStatus.Idle)
                return ANMI_NAME_IDLE;
            else if (status == E_NodeStatus.attack)
                return ANMI_NAME_ATTACK;
            else if (status == E_NodeStatus.walk)
                return ANMI_NAME_RUN;
            else if (status == E_NodeStatus.skill)
                return ANMI_NAME_SKILL;
            else if (status == E_NodeStatus.cheer)
                return ANMI_NAME_RESPONSE;
            else
                return string.Empty;
        }

        public  T_SoldierUnit GetSoliderUnitById(int id)
        {
            List<T_SoldierUnit>.Enumerator ea = ListData.GetEnumerator();
            while (ea.MoveNext())
            {
              //  Debug.Log("GetSoliderUnitById:" + ea.Current.PrefabName + " " + ea.Current.Id + "__" +id);
                if (ea.Current.Id == id)
                    return ea.Current;
            }

            Debug.Log("不存在的SoldierUnitConfig配置" + id);
            return null;
        }

        public void Load()
        {
            if (IsLoaded)
                return;

            TextAsset asset = Resources.Load<TextAsset>("Configuration/GameData/SoliderUnitConfig");
            LoadFromXML(asset.ToString());

        }
    }



}
