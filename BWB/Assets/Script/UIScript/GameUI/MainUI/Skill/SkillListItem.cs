using FairyGUI;
using FairyGUI.Utils;

public class SkillListItem : GComponent
{
    private ItemCard _CurSkill;
    private GTextField _Name;
    private GProgressBar _Exp;
    private GTextField _Attr1;
    private GTextField _Attr2;
    private GTextField _Attr3;
    private GTextField _Desc;
    private GTextField _Condition;
    private GButton _LevelUpBtn;

    private SkillStruct _SkillStruct;
    private SkillClass _SkillClass;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _CurSkill = GetChild("_CurSkill") as ItemCard;
        _Name = GetChild("_Name").asTextField;
        _Exp = GetChild("_Exp").asProgress;
        _Attr1 = GetChild("_Attr1").asTextField;
        _Attr2 = GetChild("_Attr2").asTextField;
        _Attr3 = GetChild("_Attr3").asTextField;
        _Desc = GetChild("_Desc").asTextField;
        _Condition = GetChild("_Condition").asTextField;
        _LevelUpBtn = GetChild("_LevelUpBtn").asButton;
        _LevelUpBtn.onClick.Add(OnLevelUp);
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
     * 技能信息
     */
    public void SetData(SkillStruct skillStruct)
    {
        InitShow();
        _SkillStruct = skillStruct;
        if (_SkillStruct.ID <= 0)
        {
            return;
        }
        if (DataManager.Instance.SkillData.SkillDataList.Count > 0)
        {
            foreach (SkillClass skillClassData in DataManager.Instance.SkillData.SkillDataList)
            {
                if (skillClassData.SkillID == _SkillStruct.ID)
                {
                    _SkillClass = skillClassData;
                }
            }
        }
        _CurSkill.SetSkillData(_SkillStruct, ITEM_TIPS_TYPE.NOTIPS);
        _Name.text = _SkillStruct.GetName();
        string szCondition = "";
        if (_SkillClass != null)
        {
            SkillLevelStruct skillLevelStruct = _SkillStruct.GetSkillLevel(_SkillClass.Level);
            //经验
            _Exp.visible = true;
            _Exp.value = _SkillClass.NextExp;
            _Exp.max = skillLevelStruct.Exp;
            //属性
            string szAttrValue = "";
            if (_SkillStruct.AttrType1 > 0 && skillLevelStruct.AttrValue1 > 0)
            { 
                szAttrValue = _SkillStruct.AttrType1 > 11 ? skillLevelStruct.AttrValue1*100 + "%" : skillLevelStruct.AttrValue1.ToString();
                _Attr1.text = LanguageConfig.Instance.GetText("AttrName_" + _SkillStruct.AttrType1) + " +" + szAttrValue;
            }
            if (_SkillStruct.AttrType2 > 0 && skillLevelStruct.AttrValue2 > 0)
            {
                szAttrValue = _SkillStruct.AttrType2 > 11 ? skillLevelStruct.AttrValue2 * 100 + "%" : skillLevelStruct.AttrValue2.ToString();
                _Attr2.text = LanguageConfig.Instance.GetText("AttrName_" + _SkillStruct.AttrType2) + " +" + szAttrValue;
            }
            if (_SkillStruct.AttrType3 > 0 && skillLevelStruct.AttrValue3 > 0)
            {
                szAttrValue = _SkillStruct.AttrType3 > 11 ? skillLevelStruct.AttrValue3 * 100 + "%" : skillLevelStruct.AttrValue3.ToString();
                _Attr3.text = LanguageConfig.Instance.GetText("AttrName_" + _SkillStruct.AttrType3) + " +" + szAttrValue;
            }
            //条件
            szCondition = LanguageConfig.Instance.GetText("Text_100005");
            szCondition = szCondition.Replace("@param1", LanguageConfig.Instance.GetText("SkillName_" + skillLevelStruct.SkillID));
            szCondition = szCondition.Replace("@param2", skillLevelStruct.Exp.ToString());
            if (_SkillClass.NextExp >= skillLevelStruct.Exp)
            {
                _LevelUpBtn.visible = true;
                _LevelUpBtn.text = LanguageConfig.Instance.GetText("Text_100009");
            }
            else
            {
                _Condition.text = szCondition;
            }
            if (!_SkillStruct.DictSkillLevel.ContainsKey(_SkillClass.Level + 1))
            {
                _Exp.visible = false;
                _Condition.text = LanguageConfig.Instance.GetText("Text_100010");
                _LevelUpBtn.visible = false;
            }
        }
        else
        {
            bool bCondition1 = true;
            bool bCondition2 = true;
            bool bCondition3 = true;
            //条件
            if (_SkillStruct.Gold > 0)
            {
                szCondition += LanguageConfig.Instance.GetText("Text_100004") + _SkillStruct.Gold + "\n";
                if (DataManager.Instance.CurrentRole.Gold < _SkillStruct.Gold)
                {
                    bCondition1 = false;
                }
            }
            if (_SkillStruct.CustomID > 0)
            {
                if (DataManager.Instance.CurrentRole.MonsterIndex < _SkillStruct.CustomID)
                {
                    bCondition2 = false;
                }
            }
            if (_SkillStruct.SkillID > 0)
            {
                string szSkillCondition = "";
                if (_SkillStruct.SkillLevel > 0)
                {
                    szSkillCondition = LanguageConfig.Instance.GetText("Text_100006");
                    szSkillCondition = szSkillCondition.Replace("@param1", LanguageConfig.Instance.GetText("SkillName_" + _SkillStruct.SkillID));
                    szSkillCondition = szSkillCondition.Replace("@param2", _SkillStruct.SkillLevel.ToString());
                    szCondition += szSkillCondition + "\n";
                }
                else
                {
                    szSkillCondition = LanguageConfig.Instance.GetText("Text_100007");
                    szSkillCondition = szSkillCondition.Replace("@param1", LanguageConfig.Instance.GetText("SkillName_" + _SkillStruct.SkillID));
                    szCondition += szSkillCondition + "\n";
                }
                SkillClass targetSkillClass = SkillHandler.GetSkillData(_SkillStruct.SkillID);
                bCondition3 = false;
                if (targetSkillClass != null && targetSkillClass.Level >= _SkillStruct.SkillLevel)
                {
                    bCondition3 = true;
                }
            }
            if (bCondition1 && bCondition2 && bCondition3)
            {
                _LevelUpBtn.visible = true;
                _LevelUpBtn.text = LanguageConfig.Instance.GetText("Text_100008");
            }
            else
            {
                _Condition.text = szCondition;
            }
        }
        _Desc.text = _SkillStruct.GetDesc();
    }

    private void InitShow()
    {
        _Name.text = "";
        _Exp.visible = false;
        _Attr1.text = "";
        _Attr2.text = "";
        _Attr3.text = "";
        _Desc.text = "";
        _Condition.text = "";
        _LevelUpBtn.visible = false;
        _SkillStruct = new SkillStruct();
        _SkillClass = null;
    }

    private void OnLevelUp()
    {
        //解锁
        if (_SkillClass == null)
        {
            NetManager.Instance.SkillGetRequest(_SkillStruct.ID);
        }
        //升级
        else
        {
            NetManager.Instance.SkillLevelUpRequest(_SkillClass.UniqueID);
        }
    }
}