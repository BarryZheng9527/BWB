using System.Collections.Generic;

public class NetManager
{
    static private NetManager instance;

    NetManager()
    {
    }

    public static NetManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NetManager();
            }
            return instance;
        }
    }

    public void GoldNotify(double dGold)
    {
        DataManager.Instance.CurrentRole.Gold = dGold;
        GameEventHandler.Messenger.DispatchEvent(EventConstant.GoldUpdate, dGold);
    }

    public void LoginRequest(string name, string password)
    {
        if (name == "bwb" && password == "bwb")
        {
            LoginResponse(name, password);
        }
    }

    public void LoginResponse(string name, string password)
    {
        RoleData roledata = new RoleData();
        List<RoleClass> rolelist = new List<RoleClass>();
        RoleClass roleclass = new RoleClass();
        roleclass.Name = "英雄饶命";
        roleclass.Level = 1;
        roleclass.Job = 1;
        roleclass.Exp = 120;
        roleclass.Gold = 220;
        rolelist.Add(roleclass);
        roledata.RoleList = rolelist;

        DataManager.Instance.RoleData = roledata;
        GameEventHandler.Messenger.DispatchEvent(EventConstant.Login);
    }

    public void EnterGameRequest(string name)
    {
        EnterGameResponse(name);
    }

    public void EnterGameResponse(string name)
    {
        RoleClass roleclass = new RoleClass();
        for (int iIndex = 0; iIndex < DataManager.Instance.RoleData.RoleList.Count; ++iIndex)
        {
            RoleClass role = DataManager.Instance.RoleData.RoleList[iIndex];
            if (role.Name == name)
            {
                roleclass = role;
                break;
            }
        }
        ItemData itemdata = new ItemData();
        List<ItemClass> itemlist = new List<ItemClass>();
        ItemClass itemclass = new ItemClass();
        itemclass.ItemType = Constant.EQUIP;
        itemclass.UniqueID = 10100001;
        itemclass.EquipID = 100001;
        itemclass.Level = 1;
        itemclass.RemouldOptionList.Add(1);
        itemlist.Add(itemclass);
        itemdata.ItemList = itemlist;
        SkillData skillData = new SkillData();
        skillData.SkillDataList = new List<SkillClass>();

        DataManager.Instance.CurrentRole = roleclass;
        DataManager.Instance.ItemData = itemdata;
        DataManager.Instance.SkillData = skillData;
        AttrHandler.CalculateTotalAttr();
        GameEventHandler.Messenger.DispatchEvent(EventConstant.CreatRole);
    }

    public void EquipRequest(double uniqueID, int equipPos)
    {
        EquipResponse(uniqueID, equipPos);
    }

    public void EquipResponse(double uniqueID, int equipPos)
    {
        for (int iIndex = 0; iIndex < DataManager.Instance.ItemData.ItemList.Count; ++iIndex)
        {
            ItemClass item = DataManager.Instance.ItemData.ItemList[iIndex];
            if (item.ItemType == Constant.EQUIP && item.UniqueID == uniqueID)
            {
                item.EquipPos = equipPos;
                break;
            }
        }
        AttrHandler.CalculateTotalAttr();
        GameEventHandler.Messenger.DispatchEvent(EventConstant.Equip);
    }

    public void UnEquipRequest(double uniqueID)
    {
        UnEquipResponse(uniqueID);
    }

    public void UnEquipResponse(double uniqueID)
    {
        for (int iIndex = 0; iIndex < DataManager.Instance.ItemData.ItemList.Count; ++iIndex)
        {
            ItemClass item = DataManager.Instance.ItemData.ItemList[iIndex];
            if (item.ItemType == Constant.EQUIP && item.UniqueID == uniqueID)
            {
                item.EquipPos = 0;
                break;
            }
        }
        AttrHandler.CalculateTotalAttr();
        GameEventHandler.Messenger.DispatchEvent(EventConstant.UnEquip);
    }

    public void RemouldRequest(double uniqueID, int optionIndex)
    {
        RemouldResponse(uniqueID, optionIndex);
    }

    public void RemouldResponse(double uniqueID, int optionIndex)
    {
        for (int iIndex = 0; iIndex < DataManager.Instance.ItemData.ItemList.Count; ++iIndex)
        {
            ItemClass item = DataManager.Instance.ItemData.ItemList[iIndex];
            if (item.ItemType == Constant.EQUIP && item.UniqueID == uniqueID)
            {
                item.Level++;
                item.RemouldOptionList.Add(optionIndex);
                break;
            }
        }
        AttrHandler.CalculateTotalAttr();
        GameEventHandler.Messenger.DispatchEvent(EventConstant.Remould, uniqueID);
    }

    public void SkillGetRequest(int iSkillID, int iType)
    {
        SkillGetResponse(iSkillID, iType);
    }

    public void SkillGetResponse(int iSkillID, int iType)
    {
        SkillClass skillClass = new SkillClass();
        skillClass.SkillType = iType;
        skillClass.SkillID = iSkillID;
        skillClass.Pos = 0;
        skillClass.Level = 0;
        skillClass.NextExp = 0;
        DataManager.Instance.SkillData.SkillDataList.Add(skillClass);
        SkillStruct skillStruct = SkillConfig.Instance.GetSkill(iSkillID);
        double nowGold = DataManager.Instance.CurrentRole.Gold - skillStruct.Gold;
        GoldNotify(nowGold);

        AttrHandler.CalculateTotalAttr();
        GameEventHandler.Messenger.DispatchEvent(EventConstant.SkillUpdate);
    }

    public void SkillLevelUpRequest(int iSkillID)
    {
        SkillLevelUpResponse(iSkillID);
    }

    public void SkillLevelUpResponse(int iSkillID)
    {
        foreach (SkillClass skillClassData in DataManager.Instance.SkillData.SkillDataList)
        {
            if (iSkillID == skillClassData.SkillID)
            {
                skillClassData.Level++;
            }
        }

        AttrHandler.CalculateTotalAttr();
        GameEventHandler.Messenger.DispatchEvent(EventConstant.SkillUpdate);
    }

    public void SkillEquipRequest(int iSkillID, int iPos)
    {
        SkillEquipResponse(iSkillID, iPos);
    }

    public void SkillEquipResponse(int iSkillID, int iPos)
    {
        foreach (SkillClass skillClass in DataManager.Instance.SkillData.SkillDataList)
        {
            if (iSkillID == skillClass.SkillID)
            {
                skillClass.Pos = iPos;
            }
            else if (iPos == skillClass.Pos)
            {
                skillClass.Pos = 0;
            }
        }

        GameEventHandler.Messenger.DispatchEvent(EventConstant.SkillEquipUpdate);
    }
}