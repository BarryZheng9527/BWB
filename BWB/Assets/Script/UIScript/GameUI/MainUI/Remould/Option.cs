﻿using FairyGUI;
using FairyGUI.Utils;

public class Option : GComponent
{
    private GTextField _AttrDesc;
    private GButton _RemouldBtn;

    private EquipClass _CurEquipData;
    private OptionStruct _OptionStruct;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _AttrDesc = GetChild("_AttrDesc").asTextField;
        _RemouldBtn = GetChild("_RemouldBtn").asButton;
        _RemouldBtn.onClick.Add(OnRemould);
        onAddedToStage.Add(AddedToStage);
        onRemovedFromStage.Add(RemovedFromStage);
    }

    private void AddedToStage()
    {
    }

    private void RemovedFromStage()
    {
    }

    /*
     * 改造信息
     */
    public void SetData(EquipClass equipData, OptionStruct optionStruct)
    {
        _CurEquipData = equipData;
        _OptionStruct = optionStruct;
        string attrString = "";
        for (int index = 0; index < _OptionStruct.AttrList.Count; ++index)
        {
            AttrStruct attrStruct = RemouldConfig.Instance.GetAttrStructFromID(_OptionStruct.AttrList[index]);
            string attrName = "AttrName_" + attrStruct.Type;
            string showAttr = LanguageConfig.Instance.GetText(attrName);
            if (attrStruct.Value < 1)
            {
                showAttr += " +" + (100 * attrStruct.Value) + "%";
            }
            else
            {
                showAttr += " +" + attrStruct.Value;
            }
            attrString += showAttr + "\n";
        }
        _AttrDesc.text = attrString + LanguageConfig.Instance.GetText("Text_100004") + _OptionStruct.Cost;
    }

    private void OnRemould()
    {
        double myGold = DataManager.Instance.CurrentRole.Gold;
        if (myGold < _OptionStruct.Cost)
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(ErrorConstant.ERROR_100002));
        }
        else
        {
            NetManager.Instance.RemouldRequest(_CurEquipData.UniqueID, _OptionStruct.Index);
        }
    }
}