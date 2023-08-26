using UnityEngine;
/// <summary>
/// 该脚本用于存储场景中所有可交互物体的相关信息
/// 马罗 2023 08 27
/// V 1.0.0
/// 没写完，等待补充
/// </summary>
public class ObjectSystem : MonoBehaviour
{
    #region 单例模式及初始化
    public static ObjectSystem instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    ActiveObject[,] ObjList;    //存储可交互物体的相关信息

    public void Init(int sizeX, int sizeY)
    {
        ObjList = new ActiveObject[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                ObjList[i, j] = new ActiveObject(i, j, false);
            }
        }
    }

    public class ActiveObject
    {
        Vector2 pos;            //物体的位置
        bool isPlayerChar;      //该物体是否是玩家角色
        bool isStatic;          //该物体是否是静止物体

        /// <summary>
        /// 备用变量1
        /// 当物体是玩家角色时，该变量表示为玩家角色ID，与CharacterSystem对应
        /// 当物体是其他时，我暂定设定其为物体的编号，0表示空物体，即这里没有物体
        /// </summary>
        int var1;

        public ActiveObject(int PosX, int PosY, bool IsPlayer, int Var1 = 0, bool isStatic = true)
        {
            pos = new Vector2(PosX, PosY);
            if (IsPlayer)
            {
                isStatic = false;
                isPlayerChar = true;
                var1 = Var1;
            }
            else
            {
                var1 = 0;
                isPlayerChar = false;
                isStatic = true;
            }
        }

        //物体的移动函数，当玩家角色要移动时，需要调用这个函数
        //没写完
        public void ObjectMove()
        {
            if (isPlayerChar)
            {

            }
        }


    }
}
