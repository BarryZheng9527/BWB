﻿using UnityEngine;
using System.Collections.Generic;
using FairyGUI;

public class EquipTips : Window
{

    private ItemCard _CurItemCard;
    private GTextField _Name;
    private GTextField _Type;
    private GTextField _Attr;
    private GTextField _Desc;
    private GButton _EquipBtn;
    private GButton _UnLoadBtn;

    private ITEM_TIPS_TYPE TipsType;
    private EquipClass _CurEquipData;

    protected override void OnInit()
    {
        base.OnInit();
        GRoot.inst.SetContentScaleFactor(720, 1280, UIContentScaler.ScreenMatchMode.MatchHeight);
        contentPane = UIPackage.CreateObject("Tips", "EquipTips").asCom;
        Center();
        modal = true;
        _CurItemCard = contentPane.GetChild("_CurItemCard") as ItemCard;
        _Name = contentPane.GetChild("_Name").asTextField;
        _Type = contentPane.GetChild("_Type").asTextField;
        _Attr = contentPane.GetChild("_Attr").asTextField;
        _Desc = contentPane.GetChild("_Desc").asTextField;
        _EquipBtn = contentPane.GetChild("_EquipBtn").asButton;
        _EquipBtn.onClick.Add(OnEquip);
        _UnLoadBtn = contentPane.GetChild("_UnLoadBtn").asButton;
        _UnLoadBtn.onClick.Add(OnUnLoad);
        onClick.Add(OnClose);
    }

    protected override void OnShown()
    {
        base.OnShown();
        UpdateShowInfo();
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    void OnClose()
    {
        Hide();
    }

    public void SetData(EquipClass data, ITEM_TIPS_TYPE tipsType)
    {
        _CurEquipData = data;
        TipsType = tipsType;
    }

    /*
     * 装备信息
     */
    private void UpdateShowInfo()
    {
        if (_CurEquipData != null)
        {
            EquipStruct equipStruct = EquipConfig.Instance.GetEquipFromID(_CurEquipData.EquipID);
            _CurItemCard.SetEquipData(_CurEquipData, ITEM_TIPS_TYPE.NOTIPS);
            _Name.text = equipStruct.GetColorName();
            _Type.text = equipStruct.GetTypeDesc();
            SetBasePropStr();
            _Desc.text = equipStruct.GetDesc();
            _Desc.y = _Attr.y + _Attr.height + 10;
            _UnLoadBtn.visible = false;
            _EquipBtn.visible = false;
            if (TipsType != ITEM_TIPS_TYPE.ONLYSHOW)
            {
                if (_CurEquipData.EquipPos > 0)
                {
                    _UnLoadBtn.visible = true;
                }
                else
                {
                    _EquipBtn.visible = true;
                }
            }
        }
    }

    /*
     * 装备属性
     */
    private void SetBasePropStr()
    {
        _Attr.text = "";
        EquipStruct equipStruct = EquipConfig.Instance.GetEquipFromID(_CurEquipData.EquipID);
        for (int iIndex = 0; iIndex < equipStruct.AttrList.Count; ++iIndex)
        {
            double Value = equipStruct.AttrList[iIndex];
            if (Value > 0)
            {
                string attrName = "AttrName_" + (iIndex + 1);
                string showAttr = LanguageConfig.Instance.GetText(attrName);
                if (Value < 1)
                {
                    showAttr += (100 * Value) + "%";
                }
                else
                {
                    showAttr += Value.ToString();
                }
                if (iIndex == equipStruct.AttrList.Count - 1)
                {
                    _Attr.text += showAttr;
                }
                else
                {
                    _Attr.text += showAttr + "\n";
                }
            }
        }
    }

    /*
     * 装备
     */
    private void OnEquip()
    {
        if (_CurEquipData != null)
        {
            EquipPosStrategyStruct equipPosStrategy = EquipHandler.GetEquipPosStrategy(_CurEquipData);
            if (equipPosStrategy.iErrorID > 0)
            {
                GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(equipPosStrategy.iErrorID));
            }
            else
            {
                NetManager.Instance.EquipRequest(equipPosStrategy);
            }
        }
        Hide();
    }

    /*
     * 卸载
     */
    private void OnUnLoad()
    {
        if (_CurEquipData != null)
        {
            NetManager.Instance.UnEquipRequest(_CurEquipData.UniqueID);
        }
        Hide();
    }
}