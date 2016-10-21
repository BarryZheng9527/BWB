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
	private GLoader _IconLoader;
    private GTextField _Level;
    private GTextField _Num;

    public ITEM_TIPS_TYPE _TipsType;
    public ITEM_TYPE _ItemType;
    public EquipClass _CurEquipData;
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

    /*
     * 装备
     */
    public void SetEquipData(EquipClass data, ITEM_TIPS_TYPE tipsType)
    {
        ClearShow();
        _CurEquipData = data;
        _TipsType = tipsType;
        if (_CurItemData == null)
        {
            return;
        }
        _ItemType = ITEM_TYPE.EQUIP;
        EquipStruct equipStruct = EquipConfig.Instance.GetEquipFromID(_CurEquipData.EquipID);
        _IconLoader.url = UIPackage.GetItemURL("IconEquip", equipStruct.Icon);
        if (_CurEquipData.Level > 0)
        {
            _Level.text = _CurEquipData.Level.ToString();
        }
    }

    /*
     * 道具
     */
    public void SetItemData(ItemClass data, ITEM_TIPS_TYPE tipsType)
    {
        ClearShow();
        _CurItemData = data;
        _TipsType = tipsType;
        if (_CurItemData == null)
        {
            return;
        }
        _ItemType = ITEM_TYPE.ITEM;
    }

    /*
     * 技能
     */
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
        _CurEquipData = null;
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
                if (_CurEquipData == null)
                {
                    return;
                }
                GUIManager.Instance.OpenEquipTips(_CurEquipData, _TipsType);
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