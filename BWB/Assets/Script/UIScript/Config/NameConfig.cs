using UnityEngine;
using FairyGUI;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;

public class NameConfig
{
    static private NameConfig instance = null;
    public List<string> _SurNameList = new List<string>();
    public List<string> _MaleNamesList = new List<string>();
    public List<string> _FemaleNamesList = new List<string>();

    public static NameConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NameConfig();
            }
            return instance;
        }
    }

    public void ReadXml(XmlNode root)
    {
        _SurNameList.Clear();
        _MaleNamesList.Clear();
        _FemaleNamesList.Clear();
        XmlNodeList NodeList = root.ChildNodes;
        foreach (XmlNode node in NodeList)
        {
            if (node.Name == "Surnames")
            {
                XmlNodeList ItemList = node.ChildNodes;
                foreach (XmlNode item in ItemList)
                {
                    XmlElement CurItem = (XmlElement)item;
                    _SurNameList.Add(CurItem.GetAttribute("Name"));
                }
            }
            else if (node.Name == "MaleNames")
            {
                XmlNodeList ItemList1 = node.ChildNodes;
                foreach (XmlNode item1 in ItemList1)
                {
                    XmlElement CurItem1 = (XmlElement)item1;
                    _MaleNamesList.Add(CurItem1.GetAttribute("Name"));
                }
            }
            else if (node.Name == "FemaleNames")
            {
                XmlNodeList ItemList2 = node.ChildNodes;
                foreach (XmlNode item2 in ItemList2)
                {
                    XmlElement CurItem2 = (XmlElement)item2;
                    _FemaleNamesList.Add(CurItem2.GetAttribute("Name"));
                }
            }
        }
    }

    public string GetRandomName(bool bIsMale)
    {
        int iIndex = UnityEngine.Random.Range(0, _SurNameList.Count);
        string surname = LanguageConfig.Instance.GetText(_SurNameList[iIndex]);
        if (bIsMale)
        {
            iIndex = UnityEngine.Random.Range(0, _MaleNamesList.Count);
            surname += LanguageConfig.Instance.GetText(_MaleNamesList[iIndex]);
        }
        else
        {
            iIndex = UnityEngine.Random.Range(0, _FemaleNamesList.Count);
            surname += LanguageConfig.Instance.GetText(_FemaleNamesList[iIndex]);
        }
        return surname;
    }
}