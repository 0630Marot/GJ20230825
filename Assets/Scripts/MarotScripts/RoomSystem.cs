using UnityEngine;
/// <summary>
/// 该脚本用于初始化房间信息
/// 马罗 2023 08 26
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
    }

    Vector2 NowClickBlock;  //测试用变量，存储上一次用户点击的方块位置
    //这个函数的临时测试用函数，效果是点击哪个块，哪个块就高亮
    public void ClickBlock(Vector2 pos)
    {
        pos = new Vector2(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));                    //先对鼠标点击的坐标进行取整

        if (pos.x >= RoomSize.x || pos.y >= RoomSize.y || pos.x < 0 || pos.y < 0) return;         //判断是否点击的位置是否在房间外

        if (pos != NowClickBlock)
        {
            RemoveClick(NowClickBlock);                                                         //先把之前高亮的地块取消高亮
            NowClickBlock = pos;
            roomInfo[(int)pos.x, (int)pos.y].SetShadowColor(HightLight);
        }
    }

    //测试用函数，该函数的效果是把之前高亮的物块取消高亮
    public void RemoveClick(Vector2 pos)
    {
        roomInfo[(int)pos.x, (int)pos.y].SetShadowColor(NormalColor);
    }

    //物块类，用于存储房间内单个物块的相关信息
    public class RoomBlock
    {
        Vector2 pos;        //物块的位置信息
        int type;           //物块的种类信息，具体种类对应BlockSystem文档内的信息
        GameObject obj;     //物块对应的GameOBJ

        //构造函数，填写物块的种类，X坐标，Y坐标
        public RoomBlock(int Type, int PosX, int PosY)
        {
            type = Type;
            pos = new Vector2(PosX, PosY);
            obj = Instantiate(instance.blockPrefab, pos, Quaternion.identity, instance.Blocks);
            //调用BlockSystem文档内的贴图信息
            obj.transform.GetComponent<SpriteRenderer>().sprite = BlockSystem.instance.GetBlockSprite(type);
        }

        //该函数用于更换物块的阴影颜色
        public void SetShadowColor(Color nowColor)
        {
            obj.transform.Find("Shadow").GetComponent<SpriteRenderer>().color = nowColor;
        }
    }
}
