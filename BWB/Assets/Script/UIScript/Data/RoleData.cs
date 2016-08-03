using System.Collections.Generic;

public class RoleData
{
    public List<RoleClass> RoleList;
}

public class RoleClass
{
    public string Name; //名
    public int Level; //等级
    public int Job; //职业
    public double Exp; //经验
    public double Gold; //金币
    public double Money; //钻石
    public int MonsterIndex; //关卡进度
    public double CreateTime; //创建时间
    public double LastOffLineTime; //上次登录时间
}