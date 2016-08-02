using FairyGUI;
using FairyGUI.Utils;

public class Remould : GComponent
{
    private ItemCard _CurEquip;
    private GButton _EquipTab;
    private GButton _UnEquipTab;
    private GList _OptionList;
    private GList _EquipList;

    private ItemClass _CurItemData;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _CurEquip = GetChild("_CurEquip") as ItemCard;
        _EquipTab = GetChild("_EquipTab").asButton;
        _EquipTab.onClick.Add(ShowEquipList);
        _UnEquipTab = GetChild("_UnEquipTab").asButton;
        _UnEquipTab.onClick.Add(ShowUnEquipList);
        _OptionList = GetChild("_OptionList").asList;
        _EquipList = GetChild("_EquipList").asList;
        onAddedToStage.Add(AddedToStage);
        onRemovedFromStage.Add(RemovedFromStage);
    }

    private void AddedToStage()
    {
        _EquipList.onClickItem.Add(OnSelectEquip);
        GameEventHandler.Messenger.AddEventListener(EventConstant.Remould, OnRemould);
        UpdateEquipList(GetController("c1").selectedIndex == 0);
        _CurItemData = null;
        _CurEquip.SetEquipData(_CurItemData, ITEM_TIPS_TYPE.NOTIPS);
    }

    private void RemovedFromStage()
    {
        _EquipList.onClickItem.Remove(OnSelectEquip);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.Remould, OnRemould);
        _OptionList.RemoveChildrenToPool();
    }

    private void UpdateEquipList(bool bEquip)
    {
        _EquipList.RemoveChildrenToPool();
        for (int iIndex = 0; iIndex < DataManager.Instance.ItemData.ItemList.Count; ++iIndex)
        {
            ItemClass item = DataManager.Instance.ItemData.ItemList[iIndex];
            if (item.ItemType == Constant.EQUIP && ((bEquip && item.EquipPos > 0) || (!bEquip && item.EquipPos == 0)))
            {
                EquipStruct equipStruct = EquipConfig.Instance.GetEquipFromID(item.EquipID);
                GButton remouldListItem = _EquipList.AddItemFromPool() as GButton;
                (remouldListItem.GetChild("_CurItemCard") as ItemCard).SetEquipData(item, ITEM_TIPS_TYPE.NOTIPS);
                remouldListItem.GetChild("_Name").asTextField.text = equipStruct.GetColorName();
                remouldListItem.GetChild("_Type").asTextField.text = equipStruct.GetTypeDesc();
            }
        }
    }

    private void UpdateOptionInfo()
    {
        _CurEquip.SetEquipData(_CurItemData, ITEM_TIPS_TYPE.NOTIPS);
        _OptionList.RemoveChildrenToPool();
        EquipStruct equipStruct = EquipConfig.Instance.GetEquipFromID(_CurItemData.EquipID);
        if (_CurItemData.Level < Constant.REMOULDNUM)
        {
            RemouldStruct remouldStruct = RemouldConfig.Instance.GetRemouldStructFromID(equipStruct.RemouldList[_CurItemData.Level]);
            for (int iIndex = 0; iIndex < remouldStruct.OptionList.Count; ++iIndex)
            {
                Option optionItem = _OptionList.AddItemFromPool() as Option;
                OptionStruct optionStruct = RemouldConfig.Instance.GetOptionStructFromID(remouldStruct.OptionList[iIndex]);
                optionItem.SetData(_CurItemData, optionStruct);
            }
        }
    }

    private void OnSelectEquip(EventContext context)
    {
        GButton item = (GButton)context.data;
        _CurItemData = (item.GetChild("_CurItemCard") as ItemCard)._CurItemData;
        UpdateOptionInfo();
    }

    private void ShowEquipList()
    {
        UpdateEquipList(true);
    }

    private void ShowUnEquipList()
    {
        UpdateEquipList(false);
    }

    private void OnRemould(EventContext context)
    {
        string uniqueID = (string)context.data;
        UpdateEquipList(GetController("c1").selectedIndex == 0);
        for (int iIndex = 0; iIndex < DataManager.Instance.ItemData.ItemList.Count; ++iIndex)
        {
            ItemClass item = DataManager.Instance.ItemData.ItemList[iIndex];
            if (item.ItemType == Constant.EQUIP && item.UniqueID == uniqueID)
            {
                _CurItemData = item;
                break;
            }
        }
        UpdateOptionInfo();
    }
}