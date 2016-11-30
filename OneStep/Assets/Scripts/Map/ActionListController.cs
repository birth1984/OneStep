using UnityEngine;
using System.Collections;
using IGG.CCTwo.Data;

public class ActionListController
{
    private static ActionListController m_instance;

    private ActionListController()
    {

    }

    public static ActionListController Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new ActionListController();
            return m_instance;
        }
    }

    public void PlayNodeAction(BaseNode pNode, ActionData aData)
    {
        ushort actionId = 1;
        if (pNode.ID == 5001)
            actionId = 1;
        if (pNode.ID == 5002)
            actionId = 2;
        if (pNode.ID == 5003)
            actionId = 3;
        ActionSequence actSeq = GameData.Instance.actConfig.GetActionSequence(actionId);

        while(actSeq != null)
        {
            PlayingActionInfo playingActionInfo = new PlayingActionInfo();
            playingActionInfo.actionInfo = actSeq.actionInfo;
            playingActionInfo.actionData = aData;
            playingActionInfo.srcNode = pNode;

            playAction(playingActionInfo);

            actSeq = actSeq.nextActionSequence;
        }
    }

    private void playAction(PlayingActionInfo playInfo)
    {
        if (playInfo.actionInfo.Id == 0)
        {
            ActionPlayer.Instance.ActionEndCallback(playInfo.srcNode, playInfo.actionData);
            playNextAction(playInfo);
        }
        else
            playActionData(playInfo);
    }

    private void playNextAction(PlayingActionInfo playInfo)
    {
        playInfo.actionInfo = playInfo.actionInfo.nextActionInfo;
        if(playInfo.actionInfo != null)
            playAction(playInfo);
    }

    private void playActionData(PlayingActionInfo playInfo)
    {
        INode pNode = MoveBaseMaker.Instance.GetNode(playInfo.actionInfo.Id);
        pNode.SetActionEndCallback(new ActionEndDelegate(OnActionEnd));
        pNode.SetPlayingActionInfo(playInfo);
    }

    public void OnActionEnd(INode pNode)
    {
        PlayingActionInfo playInfo = pNode.GetPlayingActionInfo();

        playNextAction(playInfo);

        pNode.SetPlayingActionInfo(null);
    }
}
