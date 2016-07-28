using UnityEngine;
using FairyGUI;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;

public struct SkillLevelStruct
{
    public int Level;
    public int SkillID;
    public int Exp;
    public double CD;
    public double Sing;
    public double MPCost;
    public double Attack;
    public double HoldBuffTime;
    public double AttrValue1;
    public double AttrValue2;
    public double AttrValue3;
    public double BuffAttrValue1;
    public double BuffAttrValue2;
    public double BuffAttrValue3;
}

public struct SkillStruct
{
    public int ID;
    public int Type;
    public string Name;
    public string Desc;
    public string Icon;
    public double Gold;
    public int CustomID;
    public int SkillID;
    public int SkillLevel;
    public int AttrType1;
    public int AttrType2;
    public int AttrType3;
    public int BuffAttrType1;
    public int BuffAttrType2;
    public int BuffAttrType3;
    public Dictionary<int, SkillLevelStruct> DictSkillLevel;

    public string GetName()
    {
        return LanguageConfig.Instance.GetText(Name);
    }

    public string GetDesc()
    {
        return LanguageConfig.Instance.GetText(Desc);
    }

    public SkillLevelStruct GetSkillLevel(int iLevel)
    {
        return DictSkillLevel[iLevel];
    }
}

public class SkillConfig
{
    static private SkillConfig instance = null;
    Dictionary<int, SkillStruct> DictSkill;

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
        DictSkill = new Dictionary<int, SkillStruct>();
        XmlNodeList NodeList = root.ChildNodes;
        foreach (XmlNode node in NodeList)
        {
            if (node.Name == "Skill")
            {
                XmlNodeList ItemList = node.ChildNodes;
                foreach (XmlNode item in ItemList)
                {
                    XmlElement CurItem = (XmlElement)item;
                    SkillStruct skill = new SkillStruct();
                    skill.ID = Convert.ToInt32(CurItem.GetAttribute("ID"));
                    skill.Type = Convert.ToInt32(CurItem.GetAttribute("Type"));
                    skill.Name = CurItem.GetAttribute("Name");
                    skill.Desc = CurItem.GetAttribute("Desc");
                    skill.Icon = CurItem.GetAttribute("Icon");
                    skill.Gold = Convert.ToDouble(CurItem.GetAttribute("Gold"));
                    skill.CustomID = Convert.ToInt32(CurItem.GetAttribute("CustomID"));
                    skill.SkillID = Convert.ToInt32(CurItem.GetAttribute("SkillID"));
                    skill.SkillLevel = Convert.ToInt32(CurItem.GetAttribute("SkillLevel"));
                    skill.AttrType1 = Convert.ToInt32(CurItem.GetAttribute("AttrType1"));
                    skill.AttrType2 = Convert.ToInt32(CurItem.GetAttribute("AttrType2"));
                    skill.AttrType3 = Convert.ToInt32(CurItem.GetAttribute("AttrType3"));
                    skill.BuffAttrType1 = Convert.ToInt32(CurItem.GetAttribute("BuffAttrType1"));
                    skill.BuffAttrType2 = Convert.ToInt32(CurItem.GetAttribute("BuffAttrType2"));
                    skill.BuffAttrType3 = Convert.ToInt32(CurItem.GetAttribute("BuffAttrType3"));
                    skill.DictSkillLevel = new Dictionary<int, SkillLevelStruct>();
                    XmlNodeList LevelList = item.ChildNodes;
                    foreach (XmlNode level in LevelList)
                    {
                        XmlElement CurLevel = (XmlElement)level;
                        SkillLevelStruct skillLevel = new SkillLevelStruct();
                        skillLevel.Level = Convert.ToInt32(CurLevel.GetAttribute("Level"));
                        skillLevel.SkillID = Convert.ToInt32(CurLevel.GetAttribute("SkillID"));
                        skillLevel.Exp = Convert.ToInt32(CurLevel.GetAttribute("Exp"));
                        skillLevel.CD = Convert.ToDouble(CurLevel.GetAttribute("CD"));
                        skillLevel.Sing = Convert.ToDouble(CurLevel.GetAttribute("Sing"));
                        skillLevel.MPCost = Convert.ToDouble(CurLevel.GetAttribute("MPCost"));
                        skillLevel.Attack = Convert.ToDouble(CurLevel.GetAttribute("Attack"));
                        skillLevel.HoldBuffTime = Convert.ToDouble(CurLevel.GetAttribute("HoldBuffTime"));
                        skillLevel.AttrValue1 = Convert.ToDouble(CurLevel.GetAttribute("AttrValue1"));
                        skillLevel.AttrValue2 = Convert.ToDouble(CurLevel.GetAttribute("AttrValue2"));
                        skillLevel.AttrValue3 = Convert.ToDouble(CurLevel.GetAttribute("AttrValue3"));
                        skillLevel.BuffAttrValue1 = Convert.ToDouble(CurLevel.GetAttribute("BuffAttrValue1"));
                        skillLevel.BuffAttrValue2 = Convert.ToDouble(CurLevel.GetAttribute("BuffAttrValue2"));
                        skillLevel.BuffAttrValue3 = Convert.ToDouble(CurLevel.GetAttribute("BuffAttrValue3"));
                        skill.DictSkillLevel.Add(skillLevel.Level, skillLevel);
                    }
                    DictSkill.Add(skill.ID, skill);
                }
            }
        }
    }

    public Dictionary<int, SkillStruct> GetDictSkill()
    {
        return DictSkill;
    }

    public SkillStruct GetSkill(int skillID)
    {
        if (DictSkill.ContainsKey(skillID))
        {
            return DictSkill[skillID];
        }
        return new SkillStruct();
    }
}