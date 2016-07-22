using System.Xml;
using System;
using System.Collections.Generic;

public struct AttrStruct
{
    public int Index;
    public int Type;
    public double Value;
}

public struct OptionStruct
{
    public int Index;
    public int Cost;
    public List<int> AttrList;
}

public struct RemouldStruct
{
    public int Index;
    public List<int> OptionList;
}

public class RemouldConfig
{
    static private RemouldConfig instance = null;
    Dictionary<int, AttrStruct> DictAttr;
    Dictionary<int, OptionStruct> DictOption;
    Dictionary<int, RemouldStruct> DictRemould;

    public static RemouldConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new RemouldConfig();
            }
            return instance;
        }
    }

    public void ReadXml(XmlNode root)
    {
        DictAttr = new Dictionary<int, AttrStruct>();
        DictOption = new Dictionary<int, OptionStruct>();
        DictRemould = new Dictionary<int, RemouldStruct>();
        XmlNodeList NodeList = root.ChildNodes;
        foreach (XmlNode node in NodeList)
        {
            if (node.Name == "Attrs")
            {
                XmlNodeList ItemList = node.ChildNodes;
                foreach (XmlNode item in ItemList)
                {
                    XmlElement CurItem = (XmlElement)item;
                    AttrStruct attr = new AttrStruct();
                    attr.Index = Convert.ToInt32(CurItem.GetAttribute("Index"));
                    attr.Type = Convert.ToInt32(CurItem.GetAttribute("Type"));
                    attr.Value = Convert.ToDouble(CurItem.GetAttribute("Value"));
                    DictAttr.Add(attr.Index, attr);
                }
            }
            else if (node.Name == "Options")
            {
                XmlNodeList ItemList1 = node.ChildNodes;
                foreach (XmlNode item1 in ItemList1)
                {
                    XmlElement CurItem1 = (XmlElement)item1;
                    OptionStruct option = new OptionStruct();
                    option.Index = Convert.ToInt32(CurItem1.GetAttribute("Index"));
                    option.Cost = Convert.ToInt32(CurItem1.GetAttribute("Cost"));
                    option.AttrList = new List<int>();
                    int iAttr1 = Convert.ToInt32(CurItem1.GetAttribute("Attr1"));
                    int iAttr2 = Convert.ToInt32(CurItem1.GetAttribute("Attr2"));
                    int iAttr3 = Convert.ToInt32(CurItem1.GetAttribute("Attr3"));
                    if (iAttr1 > 0)
                    {
                        option.AttrList.Add(iAttr1);
                    }
                    if (iAttr2 > 0)
                    {
                        option.AttrList.Add(iAttr2);
                    }
                    if (iAttr3 > 0)
                    {
                        option.AttrList.Add(iAttr3);
                    }
                    DictOption.Add(option.Index, option);
                }
            }
            else if (node.Name == "Remoulds")
            {
                XmlNodeList ItemList2 = node.ChildNodes;
                foreach (XmlNode item2 in ItemList2)
                {
                    XmlElement CurItem2 = (XmlElement)item2;
                    RemouldStruct remould = new RemouldStruct();
                    remould.Index = Convert.ToInt32(CurItem2.GetAttribute("Index"));
                    remould.OptionList = new List<int>();
                    int iOption1 = Convert.ToInt32(CurItem2.GetAttribute("Option1"));
                    int iOption2 = Convert.ToInt32(CurItem2.GetAttribute("Option2"));
                    int iOption3 = Convert.ToInt32(CurItem2.GetAttribute("Option3"));
                    int iOption4 = Convert.ToInt32(CurItem2.GetAttribute("Option4"));
                    int iOption5 = Convert.ToInt32(CurItem2.GetAttribute("Option5"));
                    if (iOption1 > 0)
                    {
                        remould.OptionList.Add(iOption1);
                    }
                    if (iOption2 > 0)
                    {
                        remould.OptionList.Add(iOption2);
                    }
                    if (iOption3 > 0)
                    {
                        remould.OptionList.Add(iOption3);
                    }
                    if (iOption4 > 0)
                    {
                        remould.OptionList.Add(iOption4);
                    }
                    if (iOption5 > 0)
                    {
                        remould.OptionList.Add(iOption5);
                    }
                    DictRemould.Add(remould.Index, remould);
                }
            }
        }
    }

    public AttrStruct GetAttrStructFromID(int AttrIndex)
    {
        if (DictAttr.ContainsKey(AttrIndex))
        {
            return DictAttr[AttrIndex];
        }
        return new AttrStruct();
    }

    public OptionStruct GetOptionStructFromID(int OptionIndex)
    {
        if (DictOption.ContainsKey(OptionIndex))
        {
            return DictOption[OptionIndex];
        }
        return new OptionStruct();
    }

    public RemouldStruct GetRemouldStructFromID(int RemouldIndex)
    {
        if (DictRemould.ContainsKey(RemouldIndex))
        {
            return DictRemould[RemouldIndex];
        }
        return new RemouldStruct();
    }
}