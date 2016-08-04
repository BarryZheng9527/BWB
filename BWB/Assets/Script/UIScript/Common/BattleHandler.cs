using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class BattleHandler
{
    /*
     * 角色普攻
     */
    static public double GetRoleAttack(Dictionary<int, double> dictTotalAttr, MonsterStruct monster)
    {
        double attack = 0;
        double dRate0 = UnityEngine.Random.Range(1, 101);
        double dRate1 = UnityEngine.Random.Range(1, 11);
        int iAttackDistance = (int)(dictTotalAttr[Constant.MAXATTACK] - dictTotalAttr[Constant.MINATTACK]);
        double dRate2 = UnityEngine.Random.Range(1, iAttackDistance + 1);
        double dRate3 = UnityEngine.Random.Range(1, 101);
        if (dRate0 < (100 * dictTotalAttr[Constant.BALANCE]))
        {
            attack = dRate1 - 5 + (dictTotalAttr[Constant.MAXATTACK] - dictTotalAttr[Constant.MINATTACK]) * (1 + dictTotalAttr[Constant.BALANCE]);
        }
        else
        {
            attack = dictTotalAttr[Constant.MINATTACK] + dRate2;
        }
        if (dRate3 < (100 * dictTotalAttr[Constant.CRIT]))
        {
            attack = attack * dictTotalAttr[Constant.CRITDAMAGE];
        }
        if (attack < 0)
        {
            attack = 0;
        }
        return attack;
    }

    /*
     * 怪物普攻
     */
    static public double GetMonsterAttack(Dictionary<int, double> dictTotalAttr, MonsterStruct monster)
    {
        double attack = (monster.Attack - Constant.DEFENSEMULTIPLE * dictTotalAttr[Constant.DEFENSE]) * (1 - dictTotalAttr[Constant.REDUCEDAMAGE]);
        if (attack < 0)
        {
            attack = 0;
        }
        return attack;
    }

    /*
     * 角色技能
     */
    static public double GetSkillAttack(Dictionary<int, double> dictTotalAttr, SkillClass skill, MonsterStruct monster)
    {
        double attack = 0;
        SkillStruct skillStruct = SkillConfig.Instance.GetSkill(skill.SkillID);
        SkillLevelStruct skillLevelStruct = skillStruct.GetSkillLevel(skill.Level);
        double dRate0 = UnityEngine.Random.Range(1, 101);
        double dRate1 = UnityEngine.Random.Range(1, 11);
        int iAttackDistance = (int)(dictTotalAttr[Constant.MAXATTACK] - dictTotalAttr[Constant.MINATTACK]);
        double dRate2 = UnityEngine.Random.Range(1, iAttackDistance + 1);
        double dRate3 = UnityEngine.Random.Range(1, 101);
        if (skillStruct.AttackType == Constant.PHYSICSSKILL)
        {
            if (dRate0 < (100 * dictTotalAttr[Constant.BALANCE]))
            {
                attack = dRate1 - 5 + (dictTotalAttr[Constant.MAXATTACK] - dictTotalAttr[Constant.MINATTACK]) * (1 + dictTotalAttr[Constant.BALANCE]);
            }
            else
            {
                attack = dictTotalAttr[Constant.MINATTACK] + dRate2;
            }
        }
        else if (skillStruct.AttackType == Constant.MAGICSKILL)
        {
            attack = Constant.MAGICATTACKMULTIPLE * dictTotalAttr[Constant.MATK] + skillLevelStruct.MATK;
        }
        if (dRate3 < (100 * dictTotalAttr[Constant.CRIT]))
        {
            attack = attack * dictTotalAttr[Constant.CRITDAMAGE];
        }
        if (skillStruct.AttackType == Constant.PHYSICSSKILL)
        {
            attack = attack * skillLevelStruct.Attack;
        }
        if (attack < 0)
        {
            attack = 0;
        }
        return attack;
    }

    /*
     * 怪物技能
     */
    static public double GetMonsterSkillAttack(Dictionary<int, double> dictTotalAttr, MonsterStruct monster, MonsterSkillStruct monsterSkill)
    {
        double attack = 0;
        if (monsterSkill.AttackType == Constant.PHYSICSSKILL)
        { 
            attack = (monster.Attack * monsterSkill.Attack - Constant.DEFENSEMULTIPLE * dictTotalAttr[Constant.DEFENSE]) * (1 - dictTotalAttr[Constant.REDUCEDAMAGE]);
        }
        else if (monsterSkill.AttackType == Constant.MAGICSKILL)
        {
            attack = (Constant.MAGICATTACKMULTIPLE * monster.Attack + monsterSkill.MATK) * (1 - dictTotalAttr[Constant.REDUCEDAMAGE]);
        }
        if (attack < 0)
        {
            attack = 0;
        }
        return attack;
    }

    /*
     * 属性克隆
     */
    static public Dictionary<int, double> CloneTotalAttr(Dictionary<int, double> dictTotalAttr)
    {
        Dictionary<int, double> DictTotalAttr = new Dictionary<int, double>();
        DictTotalAttr[Constant.STRENGTH] = dictTotalAttr[Constant.STRENGTH];
        DictTotalAttr[Constant.AGILITY] = dictTotalAttr[Constant.AGILITY];
        DictTotalAttr[Constant.WIT] = dictTotalAttr[Constant.WIT];
        DictTotalAttr[Constant.LUCY] = dictTotalAttr[Constant.LUCY];
        DictTotalAttr[Constant.DEFENSE] = dictTotalAttr[Constant.DEFENSE];
        DictTotalAttr[Constant.PROTECT] = dictTotalAttr[Constant.PROTECT];
        DictTotalAttr[Constant.HP] = dictTotalAttr[Constant.HP];
        DictTotalAttr[Constant.MP] = dictTotalAttr[Constant.MP];
        DictTotalAttr[Constant.MINATTACK] = dictTotalAttr[Constant.MINATTACK];
        DictTotalAttr[Constant.MAXATTACK] = dictTotalAttr[Constant.MAXATTACK];
        DictTotalAttr[Constant.MATK] = dictTotalAttr[Constant.MATK];
        DictTotalAttr[Constant.CRIT] = dictTotalAttr[Constant.CRIT];
        DictTotalAttr[Constant.CRITDAMAGE] = dictTotalAttr[Constant.CRITDAMAGE];
        DictTotalAttr[Constant.BALANCE] = dictTotalAttr[Constant.BALANCE];
        DictTotalAttr[Constant.FIRERATE] = dictTotalAttr[Constant.FIRERATE];
        DictTotalAttr[Constant.SINGRATE] = dictTotalAttr[Constant.SINGRATE];
        DictTotalAttr[Constant.REDUCEDAMAGE] = dictTotalAttr[Constant.REDUCEDAMAGE];
        return DictTotalAttr;
    }
}