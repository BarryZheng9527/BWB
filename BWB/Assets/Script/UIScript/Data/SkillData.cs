using System.Collections.Generic;

public class SkillData
{
    public List<SkillClass> SkillDataList = new List<SkillClass>();
}

public class SkillClass
{
    public string UniqueID; //技能唯一ID
    public int SkillType; //技能类型
    public int SkillID; //技能ID
    public int Pos; //技能装备位
    public int Level; //技能等级
    public int NextExp; //下一级技能经验
}