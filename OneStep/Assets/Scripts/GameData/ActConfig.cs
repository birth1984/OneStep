using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System;

namespace IGG.CCTwo.Data
{
    public class PlayingActionInfo
    {
        public INode srcNode;
        public ActionInfo actionInfo;
        public ActionData actionData;
    }

    public class ActionInfo
    {
        public ushort Id;
        public ActionInfo nextActionInfo;
        public ActionInfo()
        {
            nextActionInfo = null;
        }
    }

    public class ActionSequence
    {
        public ActionInfo actionInfo;
        public ActionSequence nextActionSequence;
        public ActionSequence()
        {
            actionInfo = null;
            nextActionSequence = null;
        }
    }

    public class ActConfig
    {
        public Dictionary<ushort, ActionSequence> ActionList;
        bool m_isInit;

        public ActConfig()
        {
            ActionList = new Dictionary<ushort, ActionSequence> ();
            m_isInit = false;
        }

        public void LoadFromXML(string pXmlString)
        {
            XmlDocument xDoc = new XmlDocument();

            xDoc.LoadXml(pXmlString);
            XmlElement parent = xDoc.DocumentElement;
            XmlNodeList nodeList = parent.ChildNodes;
            foreach (XmlNode act in nodeList)
            {
                ActionSequence actionSeq = new ActionSequence();
                ActionSequence preActionSeq = null;
                ushort bId = Convert.ToUInt16((act as XmlElement).GetAttribute("Id"));

                foreach (XmlNode list in act.ChildNodes)
                {
                    ActionInfo actionInfo = new ActionInfo();
                    if (preActionSeq == null)
                    {
                        actionSeq.actionInfo = actionInfo;
                        preActionSeq = actionSeq;
                    }
                    else
                    {
                        ActionSequence nextActionSeq = new ActionSequence();
                        preActionSeq.nextActionSequence = nextActionSeq;
                        nextActionSeq.actionInfo = actionInfo;
                        preActionSeq = nextActionSeq;
                    }
                    bool bIsFirst = true;
                    foreach(XmlAttribute pEle in list.Attributes)
                    {
                        if (bIsFirst)
                        {
                            actionInfo.Id = Convert.ToUInt16(pEle.Value);
                            bIsFirst = false;
                        }
                        else
                        {
                            ActionInfo nextActionInfo = new ActionInfo();
                            nextActionInfo.Id = Convert.ToUInt16(pEle.Value);
                            actionInfo.nextActionInfo = nextActionInfo;
                            actionInfo = nextActionInfo;
                        }
                    }
                }

                if (!ActionList.ContainsKey(bId))
                    ActionList.Add(bId, actionSeq);
            }
        }

        //获取建筑信息
        public ActionSequence GetActionSequence(int id)
        {
            GameData.Instance.initData(E_StaticDB_DBType.ActionInfo);
            ushort idx = (ushort)id;
            if (ActionList.ContainsKey(idx))
                return ActionList[idx];
            else
            {
                //Debug.Log("BuildingConfig:GetBuildInfo  not exists id:" + id);
                return null;
            }

        }
    }
}