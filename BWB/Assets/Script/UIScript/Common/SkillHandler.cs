using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SkillHandler
{
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

    static public List<SkillClass> GetMySkillList()
    {
        List<SkillClass> mySkillList = new List<SkillClass>();
        if (DataManager.Instance.SkillData.SkillDataList.Count > 0)
        {
            foreach (SkillClass skillClass in DataManager.Instance.SkillData.SkillDataList)
            {
                if (skillClass.Pos > 0)
                {
                    mySkillList.Add(skillClass);
                }
            }
        }
        mySkillList.Sort(delegate(SkillClass skillClass1, SkillClass skillClass2)
        {
            return skillClass1.Pos.CompareTo(skillClass2.Pos);
        });
        return mySkillList;
    }
}