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

    public void SetData(SkillStruct skillStruct)
    {
        _SkillStruct = skillStruct;
        
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
    }

    private void OnLevelUp()
    {
    }
}