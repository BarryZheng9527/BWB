using System.Collections.Generic;

public class SkillData
{
    public List<SkillClass> BattleSkillDataList;
    public List<SkillClass> PassiveSkillDataList;
}

public class SkillClass
{
    public int SkillID;
    public int Level;
    public int NextExp;
}