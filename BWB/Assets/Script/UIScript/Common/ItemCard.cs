using UnityEngine;
using FairyGUI;
using FairyGUI.Utils;
using System.Collections;
using System;

public enum ITEM_TIPS_TYPE
{
    DEFAULT = 0,    //默认
    NOTIPS = 1,     //不显示tips
    ONLYSHOW = 2,   //显示tips但不显示按钮
}

public enum ITEM_TYPE
{
    EQUIP = 1,
    ITEM = 2,
    SKILL = 3,
}

public class ItemCard : GComponent
{
    private GImage _Bg;
    public GLoader _IconLoader;
    private GTextField _Level;
    private GTextField _Num;

    public ITEM_TIPS_TYPE _TipsType;
    public ITEM_TYPE _ItemType;
    public ItemClass _CurItemData;
    public SkillStruct _CurSkillData;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _Bg = this.GetChild("_Bg").asImage;
        _IconLoader = this.GetChild("_IconLoader").asLoader;
        _Level = this.GetChild("_Level").asTextField;
        _Num = this.GetChild("_Num").asTextField;
        onClick.Add(OnShowTips);
    }

    public void SetEquipData(ItemClass data, ITEM_TIPS_TYPE tipsType)
    {
        ClearShow();
        _CurItemData = data;
        _TipsType = tipsType;
        if (_CurItemData == null)
        {
            return;
        }
        if (_CurItemData.ItemType == Constant.EQUIP)
        {
            _ItemType = ITEM_TYPE.EQUIP;
            EquipStruct equipStruct = EquipConfig.Instance.GetEquipFromID(_CurItemData.EquipID);
            _IconLoader.url = UIPackage.GetItemURL("IconEquip", equipStruct.Icon);
            if (_CurItemData.Level > 0)
            {
                _Level.text = _CurItemData.Level.ToString();
            }
        }
        else if (_CurItemData.ItemType == Constant.ITEM)
        {
            _ItemType = ITEM_TYPE.ITEM;
        }
    }

    public void SetSkillData(SkillStruct data, ITEM_TIPS_TYPE tipsType)
    {
        ClearShow();
        _CurSkillData = data;
        _TipsType = tipsType;
        _ItemType = ITEM_TYPE.SKILL;
        _IconLoader.url = UIPackage.GetItemURL("IconSkill", _CurSkillData.Icon);
    }

    public void ClearShow()
    {
        _IconLoader.url = "";
        _Level.text = "";
        _Num.text = "";
        _CurItemData = null;
        _CurSkillData = new SkillStruct();
    }

    private void OnShowTips()
    {
        if (_TipsType == ITEM_TIPS_TYPE.NOTIPS)
        {
            return;
        }
        switch (_ItemType)
        {
            case ITEM_TYPE.EQUIP:
            {
                if (_CurItemData == null)
                {
                    return;
                }
                GUIManager.Instance.OpenEquipTips(_CurItemData, ITEM_TIPS_TYPE.DEFAULT);
                break;
            }
            case ITEM_TYPE.ITEM:
            {
                if (_CurItemData == null)
                {
                    return;
                }
                break;
            }
            case ITEM_TYPE.SKILL:
            {
                break;
            }
            default:
                return;
        }
    }
}