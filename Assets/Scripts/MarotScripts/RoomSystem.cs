using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 该脚本用于初始化房间信息
/// 马罗 2023 08 27
/// V 1.0.0
/// </summary>
public class RoomSystem : MonoBehaviour
{
    #region 单例模式及初始化
    public static RoomSystem instance;
    void Awake()
    {
        instance = this;
        //Init();
    }
    #endregion

    public Transform Blocks;        //生成的房间块的父路径
    public GameObject blockPrefab;  //生成的房间块的模板
    public Vector2 RoomSize;        //房间的尺寸，x是长，y是宽

    RoomBlock[,] roomInfo;          //存储房间方格块的信息

    List<RoomBlock> VisibleBlocks = new List<RoomBlock>();

    //颜色信息
    Color HightLight = new Color(1, 1, 1, 0.5f);            //高亮，用于判定玩家可移动的范围
    Color DarkShadow = new Color(0.2f, 0.2f, 0.2f, 0.5f);   //变暗，用于展示玩家的视野阴影
    Color NormalColor = new Color(1, 1, 1, 0);              //默认，其他情况下使用

    void Start()
    {

        //初始化房间的尺寸
        roomInfo = new RoomBlock[(int)RoomSize.x, (int)RoomSize.y];     //根据RoomSize的X与Y来设置

        for (int i = 0; i < roomInfo.GetLength(0); i++)
        {
            for (int j = 0; j < roomInfo.GetLength(1); j++)
            {
                roomInfo[i, j] = new RoomBlock(2, i, j);
            }
        }

        //以下是测试用信息，用来初始化地图的
        roomInfo[1, 2].SetBlockType(1);
        roomInfo[2, 2].SetBlockType(1);
        roomInfo[4, 2].SetBlockType(1);
        roomInfo[5, 2].SetBlockType(1);
        roomInfo[7, 2].SetBlockType(1);

        CharacterSystem.instance.Init();

        OpenPlayerVisible(5, 4);

    }

    public void OpenPlayerVisible(int posX, int posY)
    {
        RemoveClick(true);
        AddVisible(posX, posY, posX, posY);
        RemoveClick();
    }

    //将目标地块标记为玩家可视范围
    void AddVisible(int posX, int posY, int tarX, int tarY)
    {
        if (roomInfo[posX, posY].isHighLight) return;       //检测该地块是否已经判定过了，避免重复操作
        roomInfo[posX, posY].isHighLight = true;
        VisibleBlocks.Add(roomInfo[posX, posY]);
        roomInfo[posX, posY].SetShadowColor(HightLight);
        float dis = Distance(posX, posY, tarX, tarY);

        //洪水算法，以该地块为中心，向周围八个格子蔓延，不断递归，直到找到所有可以看见的地块
        CheckCanMove(posX + 1, posY, tarX, tarY, dis);
        CheckCanMove(posX - 1, posY, tarX, tarY, dis);
        CheckCanMove(posX, posY + 1, tarX, tarY, dis);
        CheckCanMove(posX, posY - 1, tarX, tarY, dis);
        CheckCanMove(posX + 1, posY + 1, tarX, tarY, dis);
        CheckCanMove(posX + 1, posY - 1, tarX, tarY, dis);
        CheckCanMove(posX - 1, posY + 1, tarX, tarY, dis);
        CheckCanMove(posX - 1, posY - 1, tarX, tarY, dis);
    }

    //判断目标地块是否在玩家角色的可视范围内
    void CheckCanMove(int posX, int posY, int tarX, int tarY, float dis)
    {
        if (posX >= RoomSize.x || posY >= RoomSize.y || posX < 0 || posY < 0) return;
        float thisDis = Distance(posX, posY, tarX, tarY);
        if (thisDis < dis || thisDis > CharacterSystem.instance.CharList[CharacterSystem.instance.CharaNow].power) return;

        //经过初筛后，从目标点逆推回玩家所在地，看是否路径上有障碍物
        if (BackTrack(posX + 0.5f, posY + 0.5f, tarX + 0.5f, tarY + 0.5f))
        {
            AddVisible(posX, posY, tarX, tarY);
        }

    }

    bool BackTrack(float posX, float posY, float tarX, float tarY)
    {
        //这里额外添加地形判断
        if (roomInfo[(int)posX, (int)posY].GetBlockType() == 1) return false;   //临时拿物块1当作障碍物

        //算法逻辑是从物块的中心点出发，不断逼近玩家角色（递归），直到碰到障碍物就返回false，或抵达玩家角色返回true
        if ((int)tarX == (int)posX && (int)tarY == (int)posY) return true;

        Vector2 pos = new Vector2(posX, posY);
        Vector2 tar = new Vector2(tarX, tarY);
        Vector2 newPos = pos + (tar - pos).normalized * 0.5f;

        return BackTrack(newPos.x, newPos.y, tarX, tarY);
    }

    //判断两点间距离
    float Distance(int posX, int posY, int tarX, int tarY)
    {
        return Mathf.Sqrt(Mathf.Pow((posX - tarX), 2) + Mathf.Pow((posY - tarY), 2));
    }

    //把之前高亮的物块取消高亮，该函数一般用于每次准备移动前和移动结束后
    public void RemoveClick(bool isAll)
    {
        if (isAll)
        {
            while (VisibleBlocks.Count > 0)
            {
                RemoveClick();
            }
        }
        else
        {
            RemoveClick();
        }
    }

    public void RemoveClick()
    {
        VisibleBlocks[0].SetShadowColor(NormalColor);
        VisibleBlocks[0].isHighLight = false;
        VisibleBlocks.RemoveAt(0);
    }

    //物块类，用于存储房间内单个物块的相关信息
    public class RoomBlock
    {
        Vector2 pos;        //物块的位置信息
        int type;        //物块的种类信息，具体种类对应BlockSystem文档内的信息
        GameObject obj;     //物块对应的GameOBJ
        public bool isHighLight { get; set; }

        //构造函数，填写物块的种类，X坐标，Y坐标
        public RoomBlock(int Type, int PosX, int PosY)
        {
            type = Type;
            pos = new Vector2(PosX, PosY);
            obj = Instantiate(instance.blockPrefab, pos, Quaternion.identity, instance.Blocks);
            //调用BlockSystem文档内的贴图信息
            obj.transform.GetComponent<SpriteRenderer>().sprite = BlockSystem.instance.GetBlockSprite(type);
            isHighLight = false;
        }

        //调取物块种类信息
        public int GetBlockType()
        {
            return type;
        }

        //重设地块种类
        public void SetBlockType(int Type)
        {
            type = Type;
            obj.transform.GetComponent<SpriteRenderer>().sprite = BlockSystem.instance.GetBlockSprite(type);
        }

        //该函数用于更换物块的阴影颜色
        public void SetShadowColor(Color nowColor)
        {
            obj.transform.Find("Shadow").GetComponent<SpriteRenderer>().color = nowColor;
        }
    }
}
