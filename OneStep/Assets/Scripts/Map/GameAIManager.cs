using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameAIManager : MonoBehaviour {
    private float _timeAtLastFrame;
    private Player _currentPlayer;
    private BaseMap _currentMap;
    // Use this for initialization
    void Start () {
        _timeAtLastFrame = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (_currentPlayer == null)
            return;
        float deltaTime = (Time.time - _timeAtLastFrame) * 1000;
        if(deltaTime >= 200)
        {
            _timeAtLastFrame = Time.time;
            soldierWalkAI();
            
        }
//         if (m_autoWalkTime == 2)
//         {
//             m_autoWalkTime = 0;
//             autoWalkAI();
//             autoNPC_WalkAI(dt * 1000);
//             autoHero_WalkAI();
//             barrackMoveEnd(-1);
//         }
//         else
//         {
//             m_autoWalkTime++;
//         }
    }

    public void SetCurrPlayer(Player player)
    {
        _currentPlayer = player;
    }

    public void SetCurrMap(BaseMap map)
    {
        _currentMap = map;
    }

    private void soldierWalkAI()
    {
        /*List<Soldier> soldierList = _currentPlayer.SoldierList;
        int len = soldierList.Count;
        for(int i = 0;i < len;i++)
        {
            
        }
        foreach (int buildingIndex in soldierList)
        {
            List<Soldier> buildSoldiers = soldierList[buildingIndex] as List<Soldier>;
            Building pBuild = (_currentMap as NormalMap).GetBuildingByIndexID(buildingIndex);
            foreach (Soldier pSoldier in buildSoldiers)
            {
                if (pSoldier.IsMoving()) continue;
                if (pSoldier.moveData.MoveDelay > 0) continue;
                if (Rand_Get(buildSoldiers.Count) != 0) continue;

                Vector3 currPoint = pSoldier.GetCellPosition();
                Vector3 RandomCell = MapManager.Instance.GetRandomCellByNode(pBuild);
                if (RandomCell.x < 0 || RandomCell.y < 0) continue;
                if (currPoint.x == RandomCell.x && currPoint.y == RandomCell.y) continue;
                if (pSoldier.layerId == E_SortingLayer.SL_Sky)//空中飞行物
                {
                    RandomCell = MapManager.Instance.GetRandomPositionByCell(0, (int)RandomCell.x, (int)RandomCell.y);
                    ActionPlayer.Instance.AddMovePoint(pSoldier, RandomCell.x, RandomCell.y);
                    continue;
                }
                Vector3 RandomPosition = MapManager.Instance.GetRandomPositionByCell(0, (int)RandomCell.x, (int)RandomCell.y);

                walkParam wp = new walkParam();
                wp._startX = (byte)pSoldier.GetCellPosition().x;
                wp._startY = (byte)pSoldier.GetCellPosition().y;
                wp._destX = (byte)RandomCell.x;
                wp._destY = (byte)RandomCell.y;
                AStarWalk.Instance.startSearch(wp, 0, E_AstarType.Normal);
                List<point> findPath = new List<point>();
                AStarWalk.Instance.getPathList(findPath);
                for (int i = 0;i < findPath.Count - 1;i++)
                {
                    Vector3 pos = MapManager.Instance.GetMapPositionByCell(0, findPath[i].x, findPath[i].y);
                    ActionPlayer.Instance.AddMovePoint(pSoldier, pos.x, pos.y);
                }
                ActionPlayer.Instance.AddMovePoint(pSoldier, RandomPosition.x, RandomPosition.y);
            }
        }*/
    }

    public int Rand_Get(int iMax)
    {
        if (iMax <= 0)
            iMax = 1;
        return UnityEngine.Random.Range(0, 10000) % iMax;
    }

    public int getObjectRange(Vector3 pPoint1, Vector3 pPoint2)
    {
        int x = (int)(pPoint1.x - pPoint2.x);
        int y = (int)(pPoint1.y - pPoint2.y);
        return (int)Mathf.Sqrt((float)(x * x + y * y));
    }
}
