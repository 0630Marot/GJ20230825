using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 该脚本用于存储玩家操控角色的相关信息
/// 马罗 2023 08 27
/// V 1.0.0
/// </summary>
public class CharacterSystem : MonoBehaviour
{
    #region 单例模式及初始化
    public static CharacterSystem instance;
    void Awake()
    {
        instance = this;
        //Init();
    }
    #endregion


    public int CharaNow;    //记录当前玩家操控的角色，为其他脚本文件做准备

    public List<Character> CharList = new List<Character>();    //记录玩家拥有的角色

    //初始化
    //目前的功能仅仅是生成一个角色，作为测试在RoomSystem脚本中临时调用
    public void Init()
    {
        CharaNow = 0;
        CharList.Add(new Character(10, 10, 10, 5));
    }

    //改变当前操控的角色
    //没写完，后续补充
    public void ChangeCharacter(int order = -1)
    {
        if (order >= 0)
        {
            if (order <= CharList.Count)
            {

            }
            else
            {
                //触发回合结束
            }
        }
        else
        {
            ChangeCharacter(1 + CharaNow);
        }
    }

    //储存角色信息的类
    public class Character
    {
        float hp;       //生命值
        float maxHp;    //生命最大值
        float sp;       //护盾值
        float maxSp;    //护盾最大值
        float dp;       //耐久值
        float maxDp;    //耐久最大值

        public int power { get; set; }      //体力，后续可能改成函数操作
        int maxPower;   //体力上限

        Transform obj;  //存储角色的位置

        public Character(float MaxHp, float MaxSp, float MaxDp, int MaxPower)
        {
            maxHp = MaxHp;
            maxSp = MaxSp;
            maxDp = MaxDp;
            maxPower = MaxPower;
            hp = 0;
            sp = 0;
            dp = 0;
            dp = 0;
            power = maxPower;

            //这里还应该生成角色obj

            //该行代码注释掉是因为没准备好角色的相关UI，准备好后正常使用
            //SetValue(maxHp);
        }

        //设定角色的属性值，同时对UI绘制进行调整
        public void SetValue(float value, int type = 0)
        {
            switch (type)
            {
                case 0:
                    hp += value;
                    hp = Mathf.Clamp(hp, 0, maxHp);
                    obj.Find("HPMask").localPosition = new Vector2(0, -(hp / maxHp) * 0.92f);
                    break;
                case 1:
                    sp += value;
                    sp = Mathf.Clamp(sp, 0, maxSp);
                    if (sp > 0)
                    {
                        obj.Find("SPMask").localPosition = new Vector2(0, -(sp / maxSp) * 0.98f);
                    }
                    else
                    {
                        obj.Find("SPMask").localPosition = new Vector2(0, -2);
                    }
                    break;
                case 2:
                    dp += value;
                    dp = Mathf.Clamp(dp, 0, maxDp);
                    obj.Find("DPMask").localPosition = new Vector2(0, -0.01f - (dp / maxDp) * 0.79f);
                    break;
            }
        }
    }
}
