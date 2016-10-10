using UnityEngine;
using FairyGUI;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;

public class LanguageConfig
{
    static private LanguageConfig instance = null;
    Dictionary<string, string> DictError;
    Dictionary<string, string> DictText;

    public static LanguageConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LanguageConfig();
            }
            return instance;
        }
    }

    public void ReadXml(XmlNode root)
    {
        DictError = new Dictionary<string, string>();
        DictText = new Dictionary<string, string>();
        XmlNodeList NodeList = root.ChildNodes;
        foreach (XmlNode node in NodeList)
        {
            if (node.Name == "Errors")
            {
                XmlNodeList ItemList = node.ChildNodes;
                foreach (XmlNode item in ItemList)
                {
                    XmlElement CurItem = (XmlElement)item;
                    string szErrorID = CurItem.GetAttribute("ErrorID");
                    string szText = CurItem.GetAttribute("Text");
                    DictError.Add(szErrorID, szText);
                }
            }
            else if (node.Name == "Texts")
            {
                XmlNodeList ItemList1 = node.ChildNodes;
                foreach (XmlNode item1 in ItemList1)
                {
                    XmlElement CurItem1 = (XmlElement)item1;
                    string szTextID = CurItem1.GetAttribute("TextID");
                    string szText1 = CurItem1.GetAttribute("Text");
                    DictText.Add(szTextID, szText1);
                }
            }
        }
    }

    public string GetErrorText(int iErrorID)
    {
        string szErrorID = iErrorID.ToString();
        if (DictError.ContainsKey(szErrorID))
        {
            return DictError[szErrorID];
        }
        return "";
    }

    public string GetText(string szTextID)
    {
        if (DictText.ContainsKey(szTextID))
        {
            return DictText[szTextID];
        }
        return "";
    }
}