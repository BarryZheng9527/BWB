using UnityEngine;
using FairyGUI;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;

public struct BattleSkillLevelStruct
{
    public int Level;
    public int SkillType;
    public int SkillID;
    public int Exp;
    public double CD;
    public double Sing;
    public double MPCost;
    public double Attack;
    public double HoldBuffTime;
    public int AttrValue1;
    public int AttrValue2;
    public int AttrValue3;
    public int BuffAttrValue1;
    public int BuffAttrValue2;
    public int BuffAttrValue3;
}

public struct BattleSkillStruct
{
    public int ID;
    public string Name;
    public string Desc;
    public string Icon;
    public int Gold;
    public int CustomID;
    public int SkillType;
    public int SkillID;
    public int SkillLevel;
    public int AttrType1;
    public int AttrType2;
    public int AttrType3;
    public int BuffAttrType1;
    public int BuffAttrType2;
    public int BuffAttrType3;
    public Dictionary<int, BattleSkillLevelStruct> DictBattleSkillLevel;

    public string GetName()
    {
        return LanguageConfig.Instance.GetText(Name);
    }

    public string GetDesc()
    {
        return LanguageConfig.Instance.GetText(Desc);
    }
}

public struct PassiveSkillLevelStruct
{
    public int Level;
    public int SkillType;
    public int SkillID;
    public int Exp;
    public int AttrValue1;
    public int AttrValue2;
    public int AttrValue3;
}

public struct PassiveSkillStruct
{
    public int ID;
    public string Name;
    public string Desc;
    public string Icon;
    public int Gold;
    public int CustomID;
    public int SkillType;
    public int SkillID;
    public int SkillLevel;
    public int AttrType1;
    public int AttrType2;
    public int AttrType3;
    public Dictionary<int, PassiveSkillLevelStruct> DictPassiveSkillLevel;

    public string GetName()
    {
        return LanguageConfig.Instance.GetText(Name);
    }

    public string GetDesc()
    {
        return LanguageConfig.Instance.GetText(Desc);
    }
}

public class SkillConfig
{
    static private SkillConfig instance = null;
    Dictionary<int, BattleSkillStruct> DictBattleSkill;
    Dictionary<int, PassiveSkillStruct> DictPassiveSkill;

    public static SkillConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SkillConfig();
            }
            return instance;
        }
    }

    public void ReadXml(XmlNode root)
    {
        DictBattleSkill = new Dictionary<int, BattleSkillStruct>();
        DictPassiveSkill = new Dictionary<int, PassiveSkillStruct>();
        XmlNodeList NodeList = root.ChildNodes;
        foreach (XmlNode node in NodeList)
        {
            if (node.Name == "BattleSkill")
            {
                XmlNodeList ItemList = node.ChildNodes;
                foreach (XmlNode item in ItemList)
                {
                    XmlElement CurItem = (XmlElement)item;
                    BattleSkillStruct battleSkill = new BattleSkillStruct();
                    battleSkill.ID = Convert.ToInt32(CurItem.GetAttribute("ID"));
                    battleSkill.Name = CurItem.GetAttribute("Name");
                    battleSkill.Desc = CurItem.GetAttribute("Desc");
                    battleSkill.Icon = CurItem.GetAttribute("Icon");
                    battleSkill.Gold = Convert.ToInt32(CurItem.GetAttribute("Gold"));
                    battleSkill.CustomID = Convert.ToInt32(CurItem.GetAttribute("CustomID"));
                    battleSkill.SkillType = Convert.ToInt32(CurItem.GetAttribute("SkillType"));
                    battleSkill.SkillID = Convert.ToInt32(CurItem.GetAttribute("SkillID"));
                    battleSkill.SkillLevel = Convert.ToInt32(CurItem.GetAttribute("SkillLevel"));
                    battleSkill.AttrType1 = Convert.ToInt32(CurItem.GetAttribute("AttrType1"));
                    battleSkill.AttrType2 = Convert.ToInt32(CurItem.GetAttribute("AttrType2"));
                    battleSkill.AttrType3 = Convert.ToInt32(CurItem.GetAttribute("AttrType3"));
                    battleSkill.BuffAttrType1 = Convert.ToInt32(CurItem.GetAttribute("BuffAttrType1"));
                    battleSkill.BuffAttrType2 = Convert.ToInt32(CurItem.GetAttribute("BuffAttrType2"));
                    battleSkill.BuffAttrType3 = Convert.ToInt32(CurItem.GetAttribute("BuffAttrType3"));
                    battleSkill.DictBattleSkillLevel = new Dictionary<int, BattleSkillLevelStruct>();
                    XmlNodeList LevelList = item.ChildNodes;
                    foreach (XmlNode level in LevelList)
                    {
                        XmlElement CurLevel = (XmlElement)level;
                        BattleSkillLevelStruct battleSkillLevel = new BattleSkillLevelStruct();
                        battleSkillLevel.Level = Convert.ToInt32(CurLevel.GetAttribute("Level"));
                        battleSkillLevel.SkillType = Convert.ToInt32(CurLevel.GetAttribute("SkillType"));
                        battleSkillLevel.SkillID = Convert.ToInt32(CurLevel.GetAttribute("SkillID"));
                        battleSkillLevel.Exp = Convert.ToInt32(CurLevel.GetAttribute("Exp"));
                        battleSkillLevel.CD = Convert.ToDouble(CurLevel.GetAttribute("CD"));
                        battleSkillLevel.Sing = Convert.ToDouble(CurLevel.GetAttribute("Sing"));
                        battleSkillLevel.MPCost = Convert.ToDouble(CurLevel.GetAttribute("MPCost"));
                        battleSkillLevel.Attack = Convert.ToDouble(CurLevel.GetAttribute("Attack"));
                        battleSkillLevel.HoldBuffTime = Convert.ToDouble(CurLevel.GetAttribute("HoldBuffTime"));
                        battleSkillLevel.AttrValue1 = Convert.ToInt32(CurLevel.GetAttribute("AttrValue1"));
                        battleSkillLevel.AttrValue2 = Convert.ToInt32(CurLevel.GetAttribute("AttrValue2"));
                        battleSkillLevel.AttrValue3 = Convert.ToInt32(CurLevel.GetAttribute("AttrValue3"));
                        battleSkillLevel.BuffAttrValue1 = Convert.ToInt32(CurLevel.GetAttribute("BuffAttrValue1"));
                        battleSkillLevel.BuffAttrValue2 = Convert.ToInt32(CurLevel.GetAttribute("BuffAttrValue2"));
                        battleSkillLevel.BuffAttrValue3 = Convert.ToInt32(CurLevel.GetAttribute("BuffAttrValue3"));
                        battleSkill.DictBattleSkillLevel.Add(battleSkillLevel.Level, battleSkillLevel);
                    }
                    DictBattleSkill.Add(battleSkill.ID, battleSkill);
                }
            }
            else if (node.Name == "PassiveSkill")
            {
                XmlNodeList ItemList1 = node.ChildNodes;
                foreach (XmlNode item1 in ItemList1)
                {
                    XmlElement CurItem1 = (XmlElement)item1;
                    PassiveSkillStruct passiveSkill = new PassiveSkillStruct();
                    passiveSkill.ID = Convert.ToInt32(CurItem1.GetAttribute("ID"));
                    passiveSkill.Name = CurItem1.GetAttribute("Name");
                    passiveSkill.Desc = CurItem1.GetAttribute("Desc");
                    passiveSkill.Icon = CurItem1.GetAttribute("Icon");
                    passiveSkill.Gold = Convert.ToInt32(CurItem1.GetAttribute("Gold"));
                    passiveSkill.CustomID = Convert.ToInt32(CurItem1.GetAttribute("CustomID"));
                    passiveSkill.SkillType = Convert.ToInt32(CurItem1.GetAttribute("SkillType"));
                    passiveSkill.SkillID = Convert.ToInt32(CurItem1.GetAttribute("SkillID"));
                    passiveSkill.SkillLevel = Convert.ToInt32(CurItem1.GetAttribute("SkillLevel"));
                    passiveSkill.AttrType1 = Convert.ToInt32(CurItem1.GetAttribute("AttrType1"));
                    passiveSkill.AttrType2 = Convert.ToInt32(CurItem1.GetAttribute("AttrType2"));
                    passiveSkill.AttrType3 = Convert.ToInt32(CurItem1.GetAttribute("AttrType3"));
                    passiveSkill.DictPassiveSkillLevel = new Dictionary<int, PassiveSkillLevelStruct>();
                    XmlNodeList LevelList1 = item1.ChildNodes;
                    foreach (XmlNode level1 in LevelList1)
                    {
                        XmlElement CurLevel1 = (XmlElement)level1;
                        PassiveSkillLevelStruct passiveSkillLevel = new PassiveSkillLevelStruct();
                        passiveSkillLevel.Level = Convert.ToInt32(CurLevel1.GetAttribute("Level"));
                        passiveSkillLevel.SkillType = Convert.ToInt32(CurLevel1.GetAttribute("SkillType"));
                        passiveSkillLevel.SkillID = Convert.ToInt32(CurLevel1.GetAttribute("SkillID"));
                        passiveSkillLevel.Exp = Convert.ToInt32(CurLevel1.GetAttribute("Exp"));
                        passiveSkillLevel.AttrValue1 = Convert.ToInt32(CurLevel1.GetAttribute("AttrValue1"));
                        passiveSkillLevel.AttrValue2 = Convert.ToInt32(CurLevel1.GetAttribute("AttrValue2"));
                        passiveSkillLevel.AttrValue3 = Convert.ToInt32(CurLevel1.GetAttribute("AttrValue3"));
                        passiveSkill.DictPassiveSkillLevel.Add(passiveSkillLevel.Level, passiveSkillLevel);
                    }
                    DictPassiveSkill.Add(passiveSkill.ID, passiveSkill);
                }
            }
        }
    }

    public BattleSkillStruct GetBattleSkill(int battleSkillID)
    {
        if (DictBattleSkill.ContainsKey(battleSkillID))
        {
            return DictBattleSkill[battleSkillID];
        }
        return new BattleSkillStruct();
    }

    public PassiveSkillStruct GetPassiveSkill(int passiveSkillID)
    {
        if (DictPassiveSkill.ContainsKey(passiveSkillID))
        {
            return DictPassiveSkill[passiveSkillID];
        }
        return new PassiveSkillStruct();
    }
}