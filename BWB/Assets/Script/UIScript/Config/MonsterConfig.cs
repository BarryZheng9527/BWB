using UnityEngine;
using FairyGUI;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;

public struct MonsterStruct
{
    public int Index;
    public int IsBoss;
    public string Name;
    public double Attack;
    public double HP;
    public double MP;
    public int SkillID1;
    public int SkillID2;
    public int SkillID3;
    public int SkillID4;
    public double Gold;
    public double Exp;
    public int EquipID;
    public double EquipRate;
    public int ItemID;
    public double ItemRate;

    public string GetName()
    {
        return LanguageConfig.Instance.GetText(Name);
    }
}

public struct MonsterSkillStruct
{
    public int Index;
    public int AttackType;
    public double CD;
    public double Sing;
    public double MPCost;
    public double Attack;
    public double MATK;
    public double HoldBuffTime;
}

public class MonsterConfig
{
    static private MonsterConfig instance = null;
    Dictionary<int, MonsterStruct> DictMonster;
    Dictionary<int, MonsterSkillStruct> DictMonsterSkill;

    public static MonsterConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MonsterConfig();
            }
            return instance;
        }
    }

    public void ReadXml(XmlNode root)
    {
        DictMonster = new Dictionary<int, MonsterStruct>();
        DictMonsterSkill = new Dictionary<int, MonsterSkillStruct>();
        XmlNodeList NodeList = root.ChildNodes;
        foreach (XmlNode node in NodeList)
        {
            if (node.Name == "Monster")
            {
                XmlNodeList ItemList = node.ChildNodes;
                foreach (XmlNode item in ItemList)
                {
                    XmlElement CurItem = (XmlElement)item;
                    MonsterStruct monster = new MonsterStruct();
                    monster.Index = Convert.ToInt32(CurItem.GetAttribute("Index"));
                    monster.IsBoss = Convert.ToInt32(CurItem.GetAttribute("IsBoss"));
                    monster.Name = CurItem.GetAttribute("Name");
                    monster.Attack = Convert.ToDouble(CurItem.GetAttribute("Attack"));
                    monster.HP = Convert.ToDouble(CurItem.GetAttribute("HP"));
                    monster.MP = Convert.ToDouble(CurItem.GetAttribute("MP"));
                    monster.SkillID1 = Convert.ToInt32(CurItem.GetAttribute("SkillID1"));
                    monster.SkillID2 = Convert.ToInt32(CurItem.GetAttribute("SkillID2"));
                    monster.SkillID3 = Convert.ToInt32(CurItem.GetAttribute("SkillID3"));
                    monster.SkillID4 = Convert.ToInt32(CurItem.GetAttribute("SkillID4"));
                    monster.Gold = Convert.ToDouble(CurItem.GetAttribute("Gold"));
                    monster.Exp = Convert.ToDouble(CurItem.GetAttribute("Exp"));
                    monster.EquipID = Convert.ToInt32(CurItem.GetAttribute("EquipID"));
                    monster.EquipRate = Convert.ToDouble(CurItem.GetAttribute("EquipRate"));
                    monster.ItemID = Convert.ToInt32(CurItem.GetAttribute("ItemID"));
                    monster.ItemRate = Convert.ToDouble(CurItem.GetAttribute("ItemRate"));
                    DictMonster.Add(monster.Index, monster);
                }
            }
            if (node.Name == "MonsterSkill")
            {
                XmlNodeList ItemList0 = node.ChildNodes;
                foreach (XmlNode item0 in ItemList0)
                {
                    XmlElement CurItem0 = (XmlElement)item0;
                    MonsterSkillStruct monsterSkill = new MonsterSkillStruct();
                    monsterSkill.Index = Convert.ToInt32(CurItem0.GetAttribute("Index"));
                    monsterSkill.AttackType = Convert.ToInt32(CurItem0.GetAttribute("AttackType"));
                    monsterSkill.CD = Convert.ToDouble(CurItem0.GetAttribute("CD"));
                    monsterSkill.Sing = Convert.ToDouble(CurItem0.GetAttribute("Sing"));
                    monsterSkill.MPCost = Convert.ToDouble(CurItem0.GetAttribute("MPCost"));
                    monsterSkill.Attack = Convert.ToDouble(CurItem0.GetAttribute("Attack"));
                    monsterSkill.MATK = Convert.ToDouble(CurItem0.GetAttribute("MATK"));
                    monsterSkill.HoldBuffTime = Convert.ToDouble(CurItem0.GetAttribute("HoldBuffTime"));
                    DictMonsterSkill.Add(monsterSkill.Index, monsterSkill);
                }
            }
        }
    }

    public MonsterStruct GetMonster(int monsterID)
    {
        if (DictMonster.ContainsKey(monsterID))
        {
            return DictMonster[monsterID];
        }
        return new MonsterStruct();
    }

    public MonsterSkillStruct GetMonsterSkill(int monsterSkillID)
    {
        if (DictMonsterSkill.ContainsKey(monsterSkillID))
        {
            return DictMonsterSkill[monsterSkillID];
        }
        return new MonsterSkillStruct();
    }
}