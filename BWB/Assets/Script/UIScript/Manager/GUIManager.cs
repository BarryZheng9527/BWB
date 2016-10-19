using UnityEngine;
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
        UIPackage.AddPackage("FGUI/IconSkill/IconSkill");
        UIPackage.AddPackage("FGUI/Login/Login");
        UIPackage.AddPackage("FGUI/MainUI/MainUI");
        UIPackage.AddPackage("FGUI/CreatRole/CreatRole");
        UIPackage.AddPackage("FGUI/Tips/Tips");
    }

    void SetPackageExtension()
    {
       // UIObjectFactory.SetLoaderExtension(typeof(MyGLoader));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "Role"), typeof(Role));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "Attr"), typeof(Attr));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "BagPackage"), typeof(BagPackage));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "Battle"), typeof(Battle));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "BattleMessage"), typeof(BattleMessage));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "BattleMessageItem"), typeof(BattleMessageItem));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "Skill"), typeof(Skill));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "SkillListItem"), typeof(SkillListItem));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "Remould"), typeof(Remould));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("MainUI", "Option"), typeof(Option));
        UIObjectFactory.SetPackageItemExtension(UIPackage.GetItemURL("Common", "ItemCard"), typeof(ItemCard));
    }

    /*
     * 登陆界面
     */
    public void OpenLogin()
    {
        login.Show();
    }

    /*
     * 主界面
     */
    public void OpenMainUI()
    {
        mainUI.Show();
    }

    /*
     * 选角界面
     */
    public void OpenCreatRole()
    {
        creatRole.Show();
    }

    /*
     * 装备tips
     */
    public void OpenEquipTips(EquipClass equip, ITEM_TIPS_TYPE tipsType)
    {
        equipTips.SetData(equip, tipsType);
        equipTips.Show();
    }

    /*
     * 弹字信息
     */
    public void OpenPopMessage(string text)
    {
        popMessage.setText(text);
    }
}