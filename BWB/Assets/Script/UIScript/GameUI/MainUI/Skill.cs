using FairyGUI;
using FairyGUI.Utils;
using System.Collections.Generic;

public class Skill : GComponent
{
    private ItemCard _MySkill1;
    private ItemCard _MySkill2;
    private ItemCard _MySkill3;
    private ItemCard _MySkill4;
    private GButton _BattleBtn;
    private GButton _PassiveBtn;
    private GList _SkillList;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _MySkill1 = GetChild("_MySkill1") as ItemCard;
        _MySkill2 = GetChild("_MySkill2") as ItemCard;
        _MySkill3 = GetChild("_MySkill3") as ItemCard;
        _MySkill4 = GetChild("_MySkill4") as ItemCard;
        _MySkill1.onDrop.Add((EventContext context) =>
        {
            int iSkillID1 = (int)context.data;
            OnDropEnd(iSkillID1, 1);
        });
        _MySkill2.onDrop.Add((EventContext context) =>
        {
            int iSkillID2 = (int)context.data;
            OnDropEnd(iSkillID2, 2);
        });
        _MySkill3.onDrop.Add((EventContext context) =>
        {
            int iSkillID3 = (int)context.data;
            OnDropEnd(iSkillID3, 3);
        });
        _MySkill4.onDrop.Add((EventContext context) =>
        {
            int iSkillID4 = (int)context.data;
            OnDropEnd(iSkillID4, 4);
        });
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
        GameEventHandler.Messenger.AddEventListener(EventConstant.SkillEquipUpdate, OnMySkillUpdate);
        OnUpdateShowList();
        OnUpdateMySkill();
    }

    private void RemovedFromStage()
    {
        _BattleBtn.onClick.Remove(OnUpdateShowList);
        _PassiveBtn.onClick.Remove(OnUpdateShowList);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.SkillUpdate, OnSkillUpdate);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.SkillEquipUpdate, OnMySkillUpdate);
    }

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

    private void OnDropEnd(int iSkillID, int iPos)
    {
        NetManager.Instance.SkillEquipRequest(iSkillID, iPos);
    }

    private void OnSkillUpdate()
    {
        OnUpdateShowList();
    }

    private void OnMySkillUpdate()
    {
        OnUpdateMySkill();
    }
}