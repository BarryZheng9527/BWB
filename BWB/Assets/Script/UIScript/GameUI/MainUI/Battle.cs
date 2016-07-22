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
    }

    private void RemovedFromStage()
    {
    }
}