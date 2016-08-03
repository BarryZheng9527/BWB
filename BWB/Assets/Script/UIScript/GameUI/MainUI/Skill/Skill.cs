using FairyGUI;
using FairyGUI.Utils;
using System.Collections.Generic;

public class Skill : GComponent
{
    private GButton _BattleBtn;
    private GButton _PassiveBtn;
    private GList _SkillList;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _BattleBtn = GetChild("_BattleBtn").asButton;
        _PassiveBtn = GetChild("_PassiveBtn").asButton;
        _SkillList = GetChild("_SkillList").asList;
        onAddedToStage.Add(AddedToStage);
        onRemovedFromStage.Add(RemovedFromStage);
    }

    private void AddedToStage()
    {
        _BattleBtn.onClick.Add(OnUpdateShowList);
        _PassiveBtn.onClick.Add(OnUpdateShowList);
        GameEventHandler.Messenger.AddEventListener(EventConstant.SkillUpdate, OnSkillUpdate);
        OnUpdateShowList();
    }

    private void RemovedFromStage()
    {
        _BattleBtn.onClick.Remove(OnUpdateShowList);
        _PassiveBtn.onClick.Remove(OnUpdateShowList);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.SkillUpdate, OnSkillUpdate);
    }

    /*
     * 更新技能列表
     */
    private void OnUpdateShowList()
    {
        _SkillList.RemoveChildrenToPool();
        int iType = GetController("c1").selectedIndex + 1;
        foreach (KeyValuePair<int, SkillStruct> skillPair in SkillConfig.Instance.GetDictSkill())
        {
            if (iType == skillPair.Value.Type)
            {
                SkillListItem skillListItem = _SkillList.AddItemFromPool() as SkillListItem;
                skillListItem.SetData(skillPair.Value);
            }
        }
    }

    private void OnSkillUpdate()
    {
        OnUpdateShowList();
    }
}