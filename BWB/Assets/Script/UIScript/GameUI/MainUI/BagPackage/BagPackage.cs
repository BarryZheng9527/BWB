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

    /*
     * 更新背包显示列表
     */
    private void UpdateItemList()
    {
        _ItemList.RemoveChildrenToPool();
        for (int iIndex0 = 0; iIndex0 < DataManager.Instance.EquipData.EquipList.Count; ++iIndex0)
        {
            EquipClass equip = DataManager.Instance.EquipData.EquipList[iIndex0];
            if (equip.EquipPos == 0)
            {
                ItemCard itemCard = _ItemList.AddItemFromPool() as ItemCard;
                itemCard.SetEquipData(equip, ITEM_TIPS_TYPE.DEFAULT);
            }
        }
        for (int iIndex1 = 0; iIndex1 < DataManager.Instance.ItemData.ItemList.Count; ++iIndex1)
        {
            ItemClass item = DataManager.Instance.ItemData.ItemList[iIndex1];
            ItemCard itemCard = _ItemList.AddItemFromPool() as ItemCard;
            itemCard.SetItemData(item, ITEM_TIPS_TYPE.DEFAULT);
        }
    }

    /*
     * 装备
     */
    private void OnEquip()
    {
        UpdateItemList();
    }

    /*
     * 卸载装备
     */
    private void OnUnEquip()
    {
        UpdateItemList();
    }

    /*
     * 获取道具
     */
    private void OnItemUpdate()
    {
        UpdateItemList();
    }
}