using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AttrHandler
{
    static public void CalculateTotalAttr()
    {
        Dictionary<int, double> DictBaseAttr = new Dictionary<int, double>();
        Dictionary<int, string> DictBaseAttrShow = new Dictionary<int, string>();
        Dictionary<int, double> DictTotalAttr = new Dictionary<int, double>();

        for (int iIndex = 0; iIndex < Constant.ATTRNUM; ++iIndex)
        {
            DictBaseAttr.Add(iIndex + 1, 0);
            DictBaseAttrShow.Add(iIndex + 1, "");
        }
        for (int iIndex0 = 0; iIndex0 < DataManager.Instance.ItemData.ItemList.Count; ++iIndex0)
        {
            ItemClass item = DataManager.Instance.ItemData.ItemList[iIndex0];
            if (item.ItemType == Constant.EQUIP && item.EquipPos > 0)
            {
                EquipStruct equipStruct = EquipConfig.Instance.GetEquipFromID(item.EquipID);
                for (int iIndex1 = 0; iIndex1 < Constant.ATTRNUM; ++iIndex1)
                {
                    double Value = equipStruct.AttrList[iIndex1];
                    if (Value != 0)
                    {
                        DictBaseAttr[iIndex1 + 1] += Value;
                    }
                }
                for (int iIndex2 = 0; iIndex2 < item.RemouldOptionList.Count; ++iIndex2)
                {
                    OptionStruct optionStruct = RemouldConfig.Instance.GetOptionStructFromID(item.RemouldOptionList[iIndex2]);
                    for (int iIndex3 = 0; iIndex3 < optionStruct.AttrList.Count; ++iIndex3)
                    {
                        AttrStruct attrStruct = RemouldConfig.Instance.GetAttrStructFromID(optionStruct.AttrList[iIndex3]);
                        if (attrStruct.Value != 0)
                        {
                            DictBaseAttr[attrStruct.Type] += attrStruct.Value;
                        }
                    }
                }
            }
        }
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

        int iMyLevel = DataManager.Instance.CurrentRole.Level;
        DictTotalAttr[Constant.STRENGTH] = DictBaseAttr[Constant.STRENGTH];
        DictTotalAttr[Constant.AGILITY] = DictBaseAttr[Constant.AGILITY];
        DictTotalAttr[Constant.WIT] = DictBaseAttr[Constant.WIT];
        DictTotalAttr[Constant.LUCY] = DictBaseAttr[Constant.LUCY];
        DictTotalAttr[Constant.DEFENSE] = Constant.INITDEFENSE + DictBaseAttr[Constant.DEFENSE];
        DictTotalAttr[Constant.PROTECT] = Constant.INITPROTECT + DictBaseAttr[Constant.PROTECT];
        DictTotalAttr[Constant.HP] = iMyLevel * Constant.HPMULTIPLE + DictBaseAttr[Constant.HP];
        DictTotalAttr[Constant.MP] = iMyLevel * Constant.MPMULTIPLE + DictBaseAttr[Constant.MP];

        DataManager.Instance.DictBaseAttr = DictBaseAttr;
        DataManager.Instance.DictBaseAttrShow = DictBaseAttrShow;
        DataManager.Instance.DictTotalAttr = DictTotalAttr;
        GameEventHandler.Messenger.DispatchEvent(EventConstant.TotalAttr);
    }
}