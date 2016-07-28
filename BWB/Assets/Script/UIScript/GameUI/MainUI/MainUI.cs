using UnityEngine;
using FairyGUI;

public class MainUI : Window {
    private GImage _Head1;
    private GImage _Head2;
    private GTextField _Name;
    private GTextField _TotalAttr;
    private GTextField _Level;
    private GTextField _Gold;
    private GTextField _Money;
    private GProgressBar _Exp;

    protected override void OnInit()
    {
        base.OnInit();
        contentPane = UIPackage.CreateObject("MainUI", "MainUI").asCom;
        Center();
        modal = true;
        _Head1 = contentPane.GetChild("_Head1").asImage;
        _Head2 = contentPane.GetChild("_Head2").asImage;
        _Name = contentPane.GetChild("_Name").asTextField;
        _TotalAttr = contentPane.GetChild("_TotalAttr").asTextField;
        _Level = contentPane.GetChild("_Level").asTextField;
        _Gold = contentPane.GetChild("_Gold").asTextField;
        _Money = contentPane.GetChild("_Money").asTextField;
        _Exp = contentPane.GetChild("_Exp").asProgress;
    }

    protected override void OnShown()
    {
        base.OnShown();
        GameEventHandler.Messenger.AddEventListener(EventConstant.TotalAttr, OnTotalAttr);
        GameEventHandler.Messenger.AddEventListener(EventConstant.GoldUpdate, OnGoldUpdate);
        InitShowInfo();
    }

    protected override void OnHide()
    {
        base.OnHide();
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.TotalAttr, OnTotalAttr);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.GoldUpdate, OnGoldUpdate);
    }

    private void InitShowInfo()
    {
        RoleClass currentRole = DataManager.Instance.CurrentRole;
        if (currentRole.Job == 1)
        {
            _Head1.visible = true;
        }
        else if (currentRole.Job == 2)
        {
            _Head2.visible = true;
        }
        _Name.text = currentRole.Name;
        _TotalAttr.text = LanguageConfig.Instance.GetText("Text_100003") + "????";
        _Level.text = "Lv." + currentRole.Level;
        _Gold.text = currentRole.Gold.ToString();
        _Money.text = currentRole.Money.ToString();
        double[] expProgress = LevelConfig.Instance.GetExpProgress(currentRole.Exp);
        _Exp.value = (int)expProgress[0];
        _Exp.max = (int)expProgress[1];
    }

    private void OnTotalAttr()
    {
        _TotalAttr.text = LanguageConfig.Instance.GetText("Text_100003") + "????";
    }

    private void OnGoldUpdate(EventContext context)
    {
        double nowGold = (double)context.data;
        _Gold.text = nowGold.ToString();
    }
}