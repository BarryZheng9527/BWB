using System.Xml;
using System;
using System.Collections.Generic;

public struct EquipStruct
{
    public int ID;
    public string Name;
    public string TypeDesc;
    public string Desc;
    public string Icon;
    public int Quality;
    public int EquipType;
    public int EquipPos;
    public List<double> AttrList;
    public List<int> RemouldList;

    public string GetColorName()
    {
        string colorName = LanguageConfig.Instance.GetText(Name);
        colorName = ColorHandler.GetEquipColorText(colorName, Quality);
        return colorName;
    }

    public string GetTypeDesc()
    {
        return LanguageConfig.Instance.GetText(TypeDesc);
    }

    public string GetDesc()
    {
        return LanguageConfig.Instance.GetText(Desc);
    }
}

public class EquipConfig
{
    static private EquipConfig instance = null;
    Dictionary<int, EquipStruct> DictEquip;

    public static EquipConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EquipConfig();
            }
            return instance;
        }
    }

    public void ReadXml(XmlNode root)
    {
        DictEquip = new Dictionary<int, EquipStruct>();
        XmlNodeList NodeList = root.ChildNodes;
        foreach (XmlNode node in NodeList)
        {
            if (node.Name == "Equips")
            {
                XmlNodeList ItemList = node.ChildNodes;
                foreach (XmlNode item in ItemList)
                {
                    XmlElement CurItem = (XmlElement)item;
                    EquipStruct equip = new EquipStruct();
                    equip.ID = Convert.ToInt32(CurItem.GetAttribute("ID"));
                    equip.Name = CurItem.GetAttribute("Name");
                    equip.TypeDesc = CurItem.GetAttribute("TypeDesc");
                    equip.Desc = CurItem.GetAttribute("Desc");
                    equip.Icon = CurItem.GetAttribute("Icon");
                    equip.Quality = Convert.ToInt32(CurItem.GetAttribute("Quality"));
                    equip.EquipType = Convert.ToInt32(CurItem.GetAttribute("EquipType"));
                    equip.EquipPos = Convert.ToInt32(CurItem.GetAttribute("EquipPos"));
                    equip.AttrList = new List<double>();
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr1")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr2")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr3")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr4")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr5")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr6")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr7")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr8")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr9")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr10")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr11")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr12")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr13")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr14")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr15")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr16")));
                    equip.AttrList.Add(Convert.ToDouble(CurItem.GetAttribute("Attr17")));
                    equip.RemouldList = new List<int>();
                    equip.RemouldList.Add(Convert.ToInt32(CurItem.GetAttribute("Remould1")));
                    equip.RemouldList.Add(Convert.ToInt32(CurItem.GetAttribute("Remould2")));
                    equip.RemouldList.Add(Convert.ToInt32(CurItem.GetAttribute("Remould3")));
                    equip.RemouldList.Add(Convert.ToInt32(CurItem.GetAttribute("Remould4")));
                    equip.RemouldList.Add(Convert.ToInt32(CurItem.GetAttribute("Remould5")));
                    DictEquip.Add(equip.ID, equip);
                }
            }
        }
    }

    public EquipStruct GetEquipFromID(int EquipID)
    {
        if (DictEquip.ContainsKey(EquipID))
        {
            return DictEquip[EquipID];
        }
        return new EquipStruct();
    }
}