using UnityEngine;
using FairyGUI;
using FairyGUI.Utils;
using System.Collections;
using System;

public enum ITEM_TIPS_TYPE
{
    DEFAULT = 0,
    NOTIPS = 1,
    ONLYSHOW = 2,
}

public class ItemCard : GComponent
{
    private GImage _Bg;
    private GLoader _IconLoader;
    private GTextField _Level;
    private GTextField _Num;

    public ITEM_TIPS_TYPE _TipsType;
    public ItemClass _CurItemData;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _Bg = this.GetChild("_Bg").asImage;
        _IconLoader = this.GetChild("_IconLoader").asLoader;
        _Level = this.GetChild("_Level").asTextField;
        _Num = this.GetChild("_Num").asTextField;
        onClick.Add(OnShowTips);
    }

    public void SetData(ItemClass data, ITEM_TIPS_TYPE tipsType)
    {
        _CurItemData = data;
        _TipsType = tipsType;
        if (_CurItemData != null)
        {
            if (_CurItemData.ItemType == Constant.EQUIP)
            {
                EquipStruct equipStruct = EquipConfig.Instance.GetEquipFromID(_CurItemData.EquipID);
                _IconLoader.url = UIPackage.GetItemURL("IconEquip", equipStruct.Icon);
                if (_CurItemData.Level > 0)
                {
                    _Level.text = _CurItemData.Level.ToString();
                }
                else
                {
                    _Level.text = "";
                }
                _Num.text = "";
            }
            else if (_CurItemData.ItemType == Constant.ITEM)
            {
            }
        }
        else
        {
            _IconLoader.url = "";
            _Level.text = "";
            _Num.text = "";
        }
    }

    private void OnShowTips()
    {
        if (_CurItemData == null)
        {
			return;
		}
        if (_TipsType == ITEM_TIPS_TYPE.NOTIPS)
        {
            return;
        }
        if (_CurItemData.ItemType == Constant.EQUIP)
        {
            GUIManager.Instance.OpenEquipTips(_CurItemData, ITEM_TIPS_TYPE.DEFAULT);
        }
        else if (_CurItemData.ItemType == Constant.ITEM)
        {
        }
    }
}