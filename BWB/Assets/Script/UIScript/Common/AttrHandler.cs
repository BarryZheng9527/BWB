using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AttrHandler
{
    /*
     * 计算存储属性
     */
    static public void CalculateTotalAttr()
    {
        Dictionary<int, double> DictBaseAttr = new Dictionary<int, double>();
        Dictionary<int, string> DictBaseAttrShow = new Dictionary<int, string>();
        Dictionary<int, double> DictTotalAttr = new Dictionary<int, double>();
        //初始化
        for (int iIndex = 0; iIndex < Constant.ATTRNUM; ++iIndex)
        {
            DictBaseAttr.Add(iIndex + 1, 0);
            DictBaseAttrShow.Add(iIndex + 1, "");
        }
        EquipClass lordEquip = null; //主手武器
        EquipClass assistantEquip = null; //副手单手武器
        //计算装备附加属性并找出主副手武器
        for (int iIndex0 = 0; iIndex0 < DataManager.Instance.ItemData.ItemList.Count; ++iIndex0)
        {
            EquipClass equip = DataManager.Instance.EquipData.EquipList[iIndex0];
            if (equip.EquipPos > 0)
            {
                EquipStruct equipStruct = EquipConfig.Instance.GetEquipFromID(equip.EquipID);
                for (int iIndex1 = 0; iIndex1 < Constant.ATTRNUM; ++iIndex1)
                {
                    double Value = equipStruct.AttrList[iIndex1];
                    if (Value != 0)
                    {
                        DictBaseAttr[iIndex1 + 1] += Value;
                    }
                }
                //改造属性
                for (int iIndex2 = 0; iIndex2 < equip.RemouldOptionList.Count; ++iIndex2)
                {
                    OptionStruct optionStruct = RemouldConfig.Instance.GetOptionStructFromID(equip.RemouldOptionList[iIndex2]);
                    for (int iIndex3 = 0; iIndex3 < optionStruct.AttrList.Count; ++iIndex3)
                    {
                        AttrStruct attrStruct = RemouldConfig.Instance.GetAttrStructFromID(optionStruct.AttrList[iIndex3]);
                        if (attrStruct.Value != 0)
                        {
                            DictBaseAttr[attrStruct.Type] += attrStruct.Value;
                        }
                    }
                }
                if (equip.EquipPos == Constant.EQUIPPOS1)
                {
                    lordEquip = equip;
                }
                if (equip.EquipPos == Constant.EQUIPPOS2 && equipStruct.EquipType == Constant.ONEHANDED)
                {
                    assistantEquip = equip;
                }
            }
        }
        //计算技能附加属性
        for (int iIndex5 = 0; iIndex5 < DataManager.Instance.SkillData.SkillDataList.Count; ++iIndex5)
        {
            SkillClass skill = DataManager.Instance.SkillData.SkillDataList[iIndex5];
            SkillStruct skillStruct = SkillConfig.Instance.GetSkill(skill.SkillID);
            SkillLevelStruct skillLevelStruct = skillStruct.GetSkillLevel(skill.Level);
            if (skillStruct.AttrType1 > 0 && skillLevelStruct.AttrValue1 > 0)
            {
                DictBaseAttr[skillStruct.AttrType1] += skillLevelStruct.AttrValue1;
            }
            if (skillStruct.AttrType2 > 0 && skillLevelStruct.AttrValue2 > 0)
            {
                DictBaseAttr[skillStruct.AttrType2] += skillLevelStruct.AttrValue2;
            }
            if (skillStruct.AttrType3 > 0 && skillLevelStruct.AttrValue3 > 0)
            {
                DictBaseAttr[skillStruct.AttrType3] += skillLevelStruct.AttrValue3;
            }
        }
        //展示属性
        for (int iIndex4 = 1; iIndex4 <= Constant.ATTRNUM; ++iIndex4)
        {
            if (iIndex4 > 11)
            {
                DictBaseAttrShow[iIndex4] = 100 * DictBaseAttr[iIndex4] + "%";
            }
            else
            {
                DictBaseAttrShow[iIndex4] = DictBaseAttr[iIndex4] + "";
            }
        }
        //装备武器带来的基础属性
        double minArmAttack = 0;
        double maxArmAttack = 0;
        double armMatk = 0;
        double armFirerate = 1;
        double armBalance = 0;
        if (lordEquip != null)
        {
            EquipStruct equipStruct0 = EquipConfig.Instance.GetEquipFromID(lordEquip.EquipID);
            minArmAttack += equipStruct0.MinAttack;
            maxArmAttack += equipStruct0.MaxAttack;
            armMatk += equipStruct0.Matk;
            armFirerate = equipStruct0.Firerate;
            armBalance += equipStruct0.Balance;
            if (assistantEquip != null)
            {
                EquipStruct equipStruct1 = EquipConfig.Instance.GetEquipFromID(assistantEquip.EquipID);
                minArmAttack += Constant.EQUIPPOS2RATE * equipStruct1.MinAttack;
                maxArmAttack += Constant.EQUIPPOS2RATE * equipStruct1.MaxAttack;
                armMatk += Constant.EQUIPPOS2RATE * equipStruct1.Matk;
                armFirerate += Constant.EQUIPPOS2RATE * equipStruct1.Firerate;
                armBalance += Constant.EQUIPPOS2RATE * equipStruct1.Balance;
            }
        }
        else
        {
            if (assistantEquip != null)
            {
                EquipStruct equipStruct2 = EquipConfig.Instance.GetEquipFromID(assistantEquip.EquipID);
                minArmAttack += equipStruct2.MinAttack;
                maxArmAttack += equipStruct2.MaxAttack;
                armMatk += equipStruct2.Matk;
                armFirerate = equipStruct2.Firerate;
                armBalance += equipStruct2.Balance;
            }
        }
        //最终战斗属性
        int iMyLevel = DataManager.Instance.CurrentRole.Level;
        DictTotalAttr[Constant.STRENGTH] = DictBaseAttr[Constant.STRENGTH];
        DictTotalAttr[Constant.AGILITY] = DictBaseAttr[Constant.AGILITY];
        DictTotalAttr[Constant.WIT] = DictBaseAttr[Constant.WIT];
        DictTotalAttr[Constant.LUCY] = DictBaseAttr[Constant.LUCY];
        DictTotalAttr[Constant.DEFENSE] = Constant.INITDEFENSE + DictBaseAttr[Constant.DEFENSE];
        DictTotalAttr[Constant.PROTECT] = Constant.INITPROTECT + DictBaseAttr[Constant.PROTECT];
        DictTotalAttr[Constant.HP] = iMyLevel * Constant.HPMULTIPLE + DictBaseAttr[Constant.HP];
        DictTotalAttr[Constant.MP] = iMyLevel * Constant.MPMULTIPLE + DictBaseAttr[Constant.MP];
        DictTotalAttr[Constant.MINATTACK] = minArmAttack + DictTotalAttr[Constant.STRENGTH] + DictBaseAttr[Constant.MINATTACK];
        DictTotalAttr[Constant.MAXATTACK] = maxArmAttack + Constant.ATTACKMULTIPLE * DictTotalAttr[Constant.STRENGTH] + DictBaseAttr[Constant.MAXATTACK];
        //主手为弓
        if (lordEquip != null)
        {
            EquipStruct equipStruct2 = EquipConfig.Instance.GetEquipFromID(lordEquip.EquipID);
            if (equipStruct2.EquipType == Constant.BOW)
            {
                DictTotalAttr[Constant.MINATTACK] = minArmAttack + DictTotalAttr[Constant.AGILITY] + DictBaseAttr[Constant.MINATTACK];
                DictTotalAttr[Constant.MAXATTACK] = maxArmAttack + Constant.ATTACKMULTIPLE * DictTotalAttr[Constant.AGILITY] + DictBaseAttr[Constant.MAXATTACK];
            }
        }
        DictTotalAttr[Constant.MATK] = armMatk + Constant.ATTACKMULTIPLE * DictTotalAttr[Constant.WIT] + DictBaseAttr[Constant.MATK];
        DictTotalAttr[Constant.CRIT] = DictTotalAttr[Constant.LUCY] / (DictTotalAttr[Constant.LUCY] + Constant.LUCYOVERFLOW) + DictBaseAttr[Constant.CRIT];
        DictTotalAttr[Constant.CRITDAMAGE] = DictBaseAttr[Constant.CRITDAMAGE] + 1;
        double balanceTemp = armBalance + (DictBaseAttr[Constant.AGILITY] - Constant.AGILITYWEAKEN) / (DictBaseAttr[Constant.AGILITY] + Constant.AGILITYOVERFLOW);
        if (balanceTemp < 0)
        {
            balanceTemp = 0;
        }
        else if (balanceTemp > 0.8)
        {
            balanceTemp = 0.8;
        }
        DictTotalAttr[Constant.BALANCE] = balanceTemp;
        DictTotalAttr[Constant.FIRERATE] = armFirerate * (DictBaseAttr[Constant.FIRERATE] + 1);
        DictTotalAttr[Constant.SINGRATE] = DictBaseAttr[Constant.SINGRATE] + 1;
        DictTotalAttr[Constant.REDUCEDAMAGE] = DictTotalAttr[Constant.PROTECT] / (DictTotalAttr[Constant.PROTECT] + Constant.PROTECTOVERFLOW) + DictBaseAttr[Constant.REDUCEDAMAGE];

        DataManager.Instance.DictBaseAttr = DictBaseAttr;
        DataManager.Instance.DictBaseAttrShow = DictBaseAttrShow;
        DataManager.Instance.DictTotalAttr = DictTotalAttr;
        GameEventHandler.Messenger.DispatchEvent(EventConstant.TotalAttr);
    }
}