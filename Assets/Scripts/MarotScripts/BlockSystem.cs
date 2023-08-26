using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 该脚本用于存放游戏中所有地块的信息
/// 马罗 2023 08 26
/// V 1.0.0
/// </summary>
public class BlockSystem : MonoBehaviour
{
    #region 单例模式及初始化
    public static BlockSystem instance;
    void Awake()
    {
        instance = this;
        Init();
    }
    #endregion

    public List<Sprite> SpriteList = new List<Sprite>();    //存储所有地块的贴图
    List<Block> BlockList = new List<Block>();              //存储所有地块的数据信息

    //初始化函数，所有的地块信息应在这里填写
    //目前还没写完
    void Init()
    {
        BlockList.Add(new Block(SpriteList[0]));
        BlockList.Add(new Block(SpriteList[1]));
        BlockList.Add(new Block(SpriteList[2]));
    }

    //返回对应物块种类的贴图
    public Sprite GetBlockSprite(int id)
    {
        return BlockList[id].imageSprite;
    }

    //物块类，用于存储某种物块的信息
    public class Block
    {
        public Sprite imageSprite;      //物块的贴图

        //构造函数
        //目前只有一个贴图相关信息，其他的还没来得及写
        public Block(Sprite sprite)
        {
            imageSprite = sprite;
        }

        //物块的特殊功能，还没开始写，有机会补充
        public void BlockFunction()
        {

        }
    }
}
