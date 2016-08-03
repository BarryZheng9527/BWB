using FairyGUI;
using FairyGUI.Utils;
using System.Collections.Generic;
using UnityEngine;

public class Battle : GComponent
{
    private GImage _Container;
    private RenderTexture _TargetTexture;
    private GProgressBar _MyHP;
    private GProgressBar _MyMP;
    private GProgressBar _EnemyHP;
    private GProgressBar _EnemyMP;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _Container = GetChild("_Container").asImage;
        _MyHP = GetChild("_MyHP").asProgress;
        _MyMP = GetChild("_MyMP").asProgress;
        _EnemyHP = GetChild("_EnemyHP").asProgress;
        _EnemyMP = GetChild("_EnemyMP").asProgress;
        _TargetTexture = new RenderTexture(720, 406, 0);
        Camera.main.targetTexture = _TargetTexture;
        _Container.texture = new NTexture(_TargetTexture);
        onAddedToStage.Add(AddedToStage);
        onRemovedFromStage.Add(RemovedFromStage);
    }

    private void AddedToStage()
    {
        GameEventHandler.Messenger.AddEventListener(EventConstant.InitHpMp, OnInitHpMp);
        GameEventHandler.Messenger.AddEventListener(EventConstant.HPUpdate, OnHPUpdate);
        GameEventHandler.Messenger.AddEventListener(EventConstant.MPUpdate, OnMPUpdate);
        GameEventHandler.Messenger.AddEventListener(EventConstant.MonsterHPUpdate, OnMonsterHPUpdate);
        GameEventHandler.Messenger.AddEventListener(EventConstant.MonsterMPUpdate, OnMonsterMPUpdate);
        UpdateShowInfo();
    }

    private void RemovedFromStage()
    {
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.InitHpMp, OnInitHpMp);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.HPUpdate, OnHPUpdate);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.MPUpdate, OnMPUpdate);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.MonsterHPUpdate, OnMonsterHPUpdate);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.MonsterMPUpdate, OnMonsterMPUpdate);
    }

    private void OnInitHpMp()
    {
        Dictionary<int, double> dictTotalAttr = DataManager.Instance.DictTotalAttr;
        MonsterStruct monster = MonsterConfig.Instance.GetMonster(DataManager.Instance.CurrentRole.MonsterIndex);
        _MyHP.max = (int)dictTotalAttr[Constant.HP];
        _MyMP.max = (int)dictTotalAttr[Constant.MP];
        _EnemyHP.max = (int)monster.HP;
        _EnemyMP.max = (int)monster.MP;
    }

    private void OnHPUpdate(EventContext context)
    {
        double hp = (double)context.data;
        _MyHP.value = (int)hp;
    }

    private void OnMPUpdate(EventContext context)
    {
        double mp = (double)context.data;
        _MyMP.value = (int)mp;
    }

    private void OnMonsterHPUpdate(EventContext context)
    {
        double monsterHp = (double)context.data;
        _EnemyHP.value = (int)monsterHp;
    }

    private void OnMonsterMPUpdate(EventContext context)
    {
        double monsterMp = (double)context.data;
        _EnemyMP.value = (int)monsterMp;
    }

    private void UpdateShowInfo()
    {
        OnInitHpMp();
        if (BattleManager.Instance.DictTotalAttr != null)
        {
            _MyHP.value = (int)BattleManager.Instance.DictTotalAttr[Constant.HP];
            _MyMP.value = (int)BattleManager.Instance.DictTotalAttr[Constant.MP];
        }
        if (BattleManager.Instance.CurMonster.Index > 0)
        {
            _EnemyHP.value = (int)BattleManager.Instance.CurMonster.HP;
            _EnemyMP.value = (int)BattleManager.Instance.CurMonster.MP;
        }
    }
}