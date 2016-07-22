using System.Xml;
using System;
using System.Collections.Generic;

public class LevelConfig
{
    static private LevelConfig instance = null;
    Dictionary<int, double> DictLevel;

    public static LevelConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LevelConfig();
            }
            return instance;
        }
    }

    public void ReadXml(XmlNode root)
    {
        DictLevel = new Dictionary<int, double>();
        XmlNodeList NodeList = root.ChildNodes;
        foreach (XmlNode node in NodeList)
        {
            if (node.Name == "Levels")
            {
                XmlNodeList ItemList = node.ChildNodes;
                foreach (XmlNode item in ItemList)
                {
                    XmlElement CurItem = (XmlElement)item;
                    int iLevel = Convert.ToInt32(CurItem.GetAttribute("Level"));
                    double Exp = Convert.ToDouble(CurItem.GetAttribute("Exp"));
                    DictLevel.Add(iLevel, Exp);
                }
            }
        }
    }

    public int GetLevelFromExp(double iExp)
    {
        int iLevel = 0;
        foreach (KeyValuePair<int, double> level in DictLevel)
        {
            if (iExp >= level.Value && iLevel < level.Key)
            {
                iLevel = level.Key;
            }
        }
        return iLevel;
    }

    public double[] GetExpProgress(double iExp)
    {
        double[] expProgress = new double[2];
        int iLevel = GetLevelFromExp(iExp);
        double curLevelExp = DictLevel[iLevel];
        if (DictLevel.ContainsKey(iLevel + 1))
        {
            double nextLevelExp = DictLevel[iLevel + 1];
            expProgress[0] = iExp - curLevelExp;
            expProgress[1] = nextLevelExp - curLevelExp;
        }
        else
        {
            expProgress[0] = curLevelExp;
            expProgress[1] = curLevelExp;
        }
        return expProgress;
    }
}