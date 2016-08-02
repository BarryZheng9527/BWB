using FairyGUI;
using FairyGUI.Utils;

public class BagPackage : GComponent
{
    private GList _ItemList;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _ItemList = GetChild("_ItemList").asList;
        onAddedToStage.Add(AddedToStage);
        onRemovedFromStage.Add(RemovedFromStage);
    }

    private void AddedToStage()
    {
        GameEventHandler.Messenger.AddEventListener(EventConstant.Equip, OnEquip);
        GameEventHandler.Messenger.AddEventListener(EventConstant.UnEquip, OnUnEquip);
        GameEventHandler.Messenger.AddEventListener(EventConstant.ItemUpdate, OnItemUpdate);
        UpdateItemList();
    }

    private void RemovedFromStage()
    {
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.Equip, OnEquip);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.UnEquip, OnUnEquip);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.ItemUpdate, OnItemUpdate);
    }

    private void UpdateItemList()
    {
        _ItemList.RemoveChildrenToPool();
        for (int iIndex = 0; iIndex < DataManager.Instance.ItemData.ItemList.Count; ++iIndex)
        {
            ItemClass item = DataManager.Instance.ItemData.ItemList[iIndex];
            if ((item.ItemType == Constant.EQUIP && item.EquipPos == 0) || item.ItemType == Constant.ITEM)
            {
                ItemCard itemCard = _ItemList.AddItemFromPool() as ItemCard;
                itemCard.SetEquipData(item, ITEM_TIPS_TYPE.DEFAULT);
            }
        }
    }

    private void OnEquip()
    {
        UpdateItemList();
    }

    private void OnUnEquip()
    {
        UpdateItemList();
    }

    private void OnItemUpdate()
    {
        UpdateItemList();
    }
}