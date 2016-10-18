using System.Collections.Generic;

public class RoleData
{
    public List<RoleClass> RoleList = new List<RoleClass>();
}

public class RoleClass
{
    public string UniqueID; //唯一ID
    public string Name; //名
    public int Level; //等级
    public int Job; //职业(1男2女)
    public double Exp; //经验
    public double Gold; //金币
    public double Money; //钻石
    public int MonsterIndex; //关卡进度
    public long CreateTime; //创建时间
    public long LastOffLineTime; //上次登录时间
}