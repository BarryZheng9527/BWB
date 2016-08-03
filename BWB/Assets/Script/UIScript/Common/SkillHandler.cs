using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SkillHandler
{
    /*
     * 获取已拥有的某技能信息
     */
    static public SkillClass GetSkillData(int iSkillID)
    {
        if (DataManager.Instance.SkillData.SkillDataList.Count > 0)
        {
            foreach (SkillClass skillClassData in DataManager.Instance.SkillData.SkillDataList)
            {
                if (iSkillID == skillClassData.SkillID)
                {
                    return skillClassData;
                }
            }
        }
        return null;
    }

    /*
     * 获取装载技能列表
     */
    static public List<SkillClass> GetMySkillList()
    {
        List<SkillClass> MySkillList = new List<SkillClass>();
        Dictionary<int, SkillClass> DictMySkill = new Dictionary<int, SkillClass>();
        for (int iIndex = 0; iIndex < Constant.SKILLNUM; ++iIndex)
        {
            DictMySkill.Add(iIndex + 1, null);
        }
        if (DataManager.Instance.SkillData.SkillDataList.Count > 0)
        {
            foreach (SkillClass skillClass in DataManager.Instance.SkillData.SkillDataList)
            {
                if (skillClass.Pos > 0)
                {
                    DictMySkill[skillClass.Pos] = skillClass;
                }
            }
        }
        for (int iIndex0 = 1; iIndex0 <= Constant.SKILLNUM; ++iIndex0)
        {
            if (DictMySkill[iIndex0] != null)
            {
                MySkillList.Add(DictMySkill[iIndex0]);
            }
        }
        return MySkillList;
    }
}