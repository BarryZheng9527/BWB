﻿using UnityEngine;
using FairyGUI;
using System.Collections;

public class GUIManager
{
    static private GUIManager instance;

    private Login login;
    private MainUI mainUI;
    private CreatRole creatRole;
    private EquipTips equipTips;
    private PopMessage popMessage;

    GUIManager()
    {
        login = new Login();
        mainUI = new MainUI();
        creatRole = new CreatRole();
        equipTips = new EquipTips();
        equipTips.sortingOrder = 2;
        popMessage = new PopMessage();
        popMessage.sortingOrder = 3;
    }

    public static GUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GUIManager();
            }
            return instance;
        }
    }

    public void Init()
    {
        AddPackage();
        SetPackageExtension();
    }

    void AddPackage()
    {
        UIPackage.AddPackage("FGUI/Common/Common");
        UIPackage.AddPackage("FGUI/IconCommon/IconCommon");
        UIPackage.AddPackage("FGUI/IconEquip/IconEquip");
        UIPackage.AddPackage("FGUI/Login/Login");
        UIPackage.AddPackage("FGUI/MainUI/MainUI");
        UIPackage.AddPackage("FGUI/CreatRole/CreatRole");
        UIPackage.AddPackage("FGUI/Tips/Tips");
    }

    void SetPackageExtension()
    {
       // UIObjectFactory.SetLoaderExtension(typeof(MyGLoader));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "Attr"), typeof(Attr));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "Option"), typeof(Option));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "Role"), typeof(Role));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "BagPackage"), typeof(BagPackage));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "Battle"), typeof(Battle));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "Skill"), typeof(Skill));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "Remould"), typeof(Remould));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("Common", "ItemCard"), typeof(ItemCard));
    }

    public void OpenLogin()
    {
        login.Show();
    }

    public void OpenMainUI()
    {
        mainUI.Show();
    }

    public void OpenCreatRole()
    {
        creatRole.Show();
    }

    public void OpenEquipTips(ItemClass data, ITEM_TIPS_TYPE tipsType)
    {
        equipTips.SetData(data, tipsType);
        equipTips.Show();
    }

    public void OpenPopMessage(string text)
    {
        popMessage.setText(text);
    }
}