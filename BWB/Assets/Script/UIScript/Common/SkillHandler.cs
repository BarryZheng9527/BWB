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
}