using FairyGUI;
using FairyGUI.Utils;
using System.Collections.Generic;

public class Role : GComponent
{
    private GComponent _EquipList;
    private GList _AttrList;
	private ItemCard _MySkill1;
	private ItemCard _MySkill2;
	private ItemCard _MySkill3;
	private ItemCard _MySkill4;
	private GList _MySkillList;

	private int _iPos;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _EquipList = GetChild("_EquipList").asCom;
        _AttrList = GetChild("_AttrList").asList;
		_MySkill1 = GetChild("_MySkill1") as ItemCard;
		_MySkill1.onClick.Add(ShowMySkillList1);
		_MySkill2 = GetChild("_MySkill2") as ItemCard;
		_MySkill2.onClick.Add(ShowMySkillList2);
		_MySkill3 = GetChild("_MySkill3") as ItemCard;
		_MySkill3.onClick.Add(ShowMySkillList3);
		_MySkill4 = GetChild("_MySkill4") as ItemCard;
		_MySkill4.onClick.Add(ShowMySkillList4);
		_MySkillList = GetChild("_MySkillList").asList;
		_MySkillList.visible = false;
		_MySkillList.onClickItem.Add(OnClickSkillItem);
        onAddedToStage.Add(AddedToStage);
        onRemovedFromStage.Add(RemovedFromStage);
    }

    private void AddedToStage()
    {
        GameEventHandler.Messenger.AddEventListener(EventConstant.Equip, OnEquip);
        GameEventHandler.Messenger.AddEventListener(EventConstant.UnEquip, OnUnEquip);
        GameEventHandler.Messenger.AddEventListener(EventConstant.TotalAttr, OnTotalAttr);
		GameEventHandler.Messenger.AddEventListener(EventConstant.SkillEquipUpdate, OnMySkillUpdate);
        UpdateEquipList();
        UpdateAttrList();
		OnUpdateMySkill();
    }

    private void RemovedFromStage()
    {
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.Equip, OnEquip);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.UnEquip, OnUnEquip);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.TotalAttr, OnTotalAttr);
		GameEventHandler.Messenger.RemoveEventListener(EventConstant.SkillEquipUpdate, OnMySkillUpdate);
    }

    private void UpdateEquipList()
    {
        for (int index = 1; index <= Constant.EQUIPPOSNUM; ++index)
        {
            ItemCard itemCard0 = _EquipList.GetChild("_EquipPos" + index) as ItemCard;
            itemCard0.SetEquipData(null, ITEM_TIPS_TYPE.NOTIPS);
        }
        for (int iIndex = 0; iIndex < DataManager.Instance.ItemData.ItemList.Count; ++iIndex)
        {
            ItemClass item = DataManager.Instance.ItemData.ItemList[iIndex];
            if (item.ItemType == Constant.EQUIP && item.EquipPos > 0)
            {
                ItemCard itemCard = _EquipList.GetChild("_EquipPos" + item.EquipPos) as ItemCard;
                itemCard.SetEquipData(item, ITEM_TIPS_TYPE.DEFAULT);
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

	private void OnUpdateMySkill()
	{
		_MySkill1.ClearShow();
		_MySkill2.ClearShow();
		_MySkill3.ClearShow();
		_MySkill4.ClearShow();
		foreach (SkillClass skillClass in DataManager.Instance.SkillData.SkillDataList)
		{
			if (skillClass.Pos > 0)
			{
				SkillStruct skillStruct = SkillConfig.Instance.GetSkill(skillClass.SkillID);
				if (1 == skillClass.Pos)
				{
					_MySkill1.SetSkillData(skillStruct, ITEM_TIPS_TYPE.NOTIPS);
				}
				else if (2 == skillClass.Pos)
				{
					_MySkill2.SetSkillData(skillStruct, ITEM_TIPS_TYPE.NOTIPS);
				}
				else if (3 == skillClass.Pos)
				{
					_MySkill3.SetSkillData(skillStruct, ITEM_TIPS_TYPE.NOTIPS);
				}
				else if (4 == skillClass.Pos)
				{
					_MySkill4.SetSkillData(skillStruct, ITEM_TIPS_TYPE.NOTIPS);
				}
			}
		}
	}

	private void ShowMySkillList1()
	{
		_iPos = 1;
		ShowMySkillList();
	}

	private void ShowMySkillList2()
	{
		_iPos = 2;
		ShowMySkillList();
	}

	private void ShowMySkillList3()
	{
		_iPos = 3;
		ShowMySkillList();
	}

	private void ShowMySkillList4()
	{
		_iPos = 4;
		ShowMySkillList();
	}

	private void ShowMySkillList()
	{
		_MySkillList.x = _iPos * 120;
		_MySkillList.visible = true;
		_MySkillList.RemoveChildrenToPool();
		foreach (SkillClass skillClass in DataManager.Instance.SkillData.SkillDataList) 
		{
			SkillStruct skillStruct = SkillConfig.Instance.GetSkill(skillClass.SkillID);
			if (skillStruct.Type == Constant.BATTLESKILL) 
			{
				ItemCard itemCard = _MySkillList.AddItemFromPool() as ItemCard;
				itemCard.SetSkillData(skillStruct, ITEM_TIPS_TYPE.NOTIPS);
			}
		}
	}

	private void OnClickSkillItem(EventContext context)
	{
		ItemCard item = (ItemCard)context.data;
		_MySkillList.visible = false;
		NetManager.Instance.SkillEquipRequest (item._CurSkillData.ID, _iPos);
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

	private void OnMySkillUpdate()
	{
		OnUpdateMySkill();
	}
}