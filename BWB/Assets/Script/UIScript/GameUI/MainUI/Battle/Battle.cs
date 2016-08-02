using FairyGUI;
using FairyGUI.Utils;
using UnityEngine;

public class Battle : GComponent
{
    private GImage _Container;
    private RenderTexture _TargetTexture;
    private GProgressBar _MyHP;
    private GProgressBar _MyMP;
    private GProgressBar _EnemyHP;
    private GProgressBar _EnemyMP;

    private bool bInit = true;

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
        if (bInit)
        {
            BattleManager.Instance.BattleManagerStart();
            bInit = false;
        }
        GameEventHandler.Messenger.AddEventListener(EventConstant.HPUpdate, OnHPUpdate);
        GameEventHandler.Messenger.AddEventListener(EventConstant.MPUpdate, OnMPUpdate);
        GameEventHandler.Messenger.AddEventListener(EventConstant.MonsterHPUpdate, OnMonsterHPUpdate);
        GameEventHandler.Messenger.AddEventListener(EventConstant.MonsterMPUpdate, OnMonsterMPUpdate);
    }

    private void RemovedFromStage()
    {
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.HPUpdate, OnHPUpdate);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.MPUpdate, OnMPUpdate);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.MonsterHPUpdate, OnMonsterHPUpdate);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.MonsterMPUpdate, OnMonsterMPUpdate);
    }
}