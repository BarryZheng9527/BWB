using FairyGUI;
using FairyGUI.Utils;
using System.Collections.Generic;

public class Role : GComponent
{
    private GComponent _EquipList;
    private GList _AttrList;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _EquipList = GetChild("_EquipList").asCom;
        _AttrList = GetChild("_AttrList").asList;
        onAddedToStage.Add(AddedToStage);
        onRemovedFromStage.Add(RemovedFromStage);
    }

    private void AddedToStage()
    {
        GameEventHandler.Messenger.AddEventListener(EventConstant.Equip, OnEquip);
        GameEventHandler.Messenger.AddEventListener(EventConstant.UnEquip, OnUnEquip);
        GameEventHandler.Messenger.AddEventListener(EventConstant.TotalAttr, OnTotalAttr);
        UpdateEquipList();
        UpdateAttrList();
    }

    private void RemovedFromStage()
    {
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.Equip, OnEquip);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.UnEquip, OnUnEquip);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.TotalAttr, OnTotalAttr);
    }

    private void UpdateEquipList()
    {
        for (int index = 1; index <= Constant.EQUIPPOSNUM; ++index)
        {
            ItemCard itemCard0 = _EquipList.GetChild("_EquipPos" + index) as ItemCard;
            itemCard0.SetData(null, ITEM_TIPS_TYPE.NOTIPS);
        }
        for (int iIndex = 0; iIndex < DataManager.Instance.ItemData.ItemList.Count; ++iIndex)
        {
            ItemClass item = DataManager.Instance.ItemData.ItemList[iIndex];
            if (item.ItemType == Constant.EQUIP && item.EquipPos > 0)
            {
                ItemCard itemCard = _EquipList.GetChild("_EquipPos" + item.EquipPos) as ItemCard;
                itemCard.SetData(item, ITEM_TIPS_TYPE.DEFAULT);
            }
        }
    }

    private void UpdateAttrList()
    {
        for (int iAttrIndex = 1; iAttrIndex <= Constant.ATTRNUM; ++iAttrIndex)
        {
            string attrName = "AttrName_" + iAttrIndex;
            string showAttr = LanguageConfig.Instance.GetText(attrName);
            Attr attr = _AttrList.GetChildAt(iAttrIndex - 1) as Attr;
            attr.SetNameAndValue(showAttr, DataManager.Instance.DictBaseAttrShow[iAttrIndex]);
        }
    }

    private void OnEquip()
    {
        UpdateEquipList();
    }

    private void OnUnEquip()
    {
        UpdateEquipList();
    }

    private void OnTotalAttr()
    {
        UpdateAttrList();
    }
}