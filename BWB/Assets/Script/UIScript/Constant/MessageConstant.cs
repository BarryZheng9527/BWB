﻿class MessageConstant
{
    MessageConstant() { }

    public const int CHECK_REGISTER_RESPONSE = 0x1001; //检测注册
    public const int REGISTER_RESPONSE = 0x1002; //注册

    public const int CHECK_LOGIN_RESPONSE = 0x1003; //检测登陆
    public const int LOGIN_RESPONSE = 0x1004; //登陆

    public const int CREAT_ROLE_RESPONSE = 0x1005; //创角
    public const int START_GAME_RESPONSE = 0x1006; //开始游戏

    public const int EQUIP_RESPONSE = 0x1007; //装备
    public const int UN_EQUIP_RESPONSE = 0x1008; //装备卸载
    public const int REMOULD_EQUIP_RESPONSE = 0x1009; //装备改造
    public const int EQUIP_NOTIFY = 0x1014; //装备下发
    public const int ITEM_NOTIFY = 0x1015; //道具下发

    public const int SKILL_GET_RESPONSE = 0x1010; //技能获取
    public const int SKILL_LEVEL_UP_RESPONSE = 0x1011; //技能升级
    public const int SKILL_EQUIP_RESPONSE = 0x1012; //技能装载

    public const int MONSTER_INDEX_RESPONSE = 0x1013; //更新关卡

    public const int GOLD_NOTIFY = 0x1016; //金币下发
    public const int EXP_NOTIFY = 0x1017; //经验下发
}