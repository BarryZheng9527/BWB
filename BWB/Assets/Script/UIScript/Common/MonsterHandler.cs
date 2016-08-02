using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MonsterHandler
{
    static public List<MonsterSkillStruct> GetMonsterSkillList(int iMonsterID)
    {
        List<MonsterSkillStruct> monsterSkillList = new List<MonsterSkillStruct>();
        MonsterStruct monster = MonsterConfig.Instance.GetMonster(iMonsterID);
        if (monster.SkillID1 > 0)
        {
            monsterSkillList.Add(MonsterConfig.Instance.GetMonsterSkill(monster.SkillID1));
        }
        if (monster.SkillID2 > 0)
        {
            monsterSkillList.Add(MonsterConfig.Instance.GetMonsterSkill(monster.SkillID2));
        }
        if (monster.SkillID3 > 0)
        {
            monsterSkillList.Add(MonsterConfig.Instance.GetMonsterSkill(monster.SkillID3));
        }
        if (monster.SkillID4 > 0)
        {
            monsterSkillList.Add(MonsterConfig.Instance.GetMonsterSkill(monster.SkillID4));
        }
        return monsterSkillList;
    }
}