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

    public void LoginRequest(string name, string password)
    {
        if (name == "bwb" && password == "bwb")
        {
            LoginResponse();
        }
    }

    public void LoginResponse()
    {
        RoleData roledata = new RoleData();
        List<RoleClass> rolelist = new List<RoleClass>();
        RoleClass roleclass = new RoleClass();
        roleclass.Name = "英雄饶命";
        roleclass.Level = 1;
        roleclass.Job = 1;
        roleclass.Exp = 120;
        rolelist.Add(roleclass);
        roledata.RoleList = rolelist;

        DataManager.Instance.RoleData = roledata;
        GameEventHandler.Messenger.DispatchEvent(EventConstant.Login);
    }

    public void EnterGameRequest(string name)
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
        EnterGameResponse();
    }

    public void EnterGameResponse()
    {
        GameEventHandler.Messenger.DispatchEvent(EventConstant.CreatRole);
    }

    public void EquipRequest(double uniqueID, int equipPos)
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
        EquipResponse(uniqueID, equipPos);
    }

    public void EquipResponse(double uniqueID, int equipPos)
    {
        GameEventHandler.Messenger.DispatchEvent(EventConstant.Equip);
    }

    public void UnEquipRequest(double uniqueID)
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
        UnEquipResponse(uniqueID);
    }

    public void UnEquipResponse(double uniqueID)
    {
        GameEventHandler.Messenger.DispatchEvent(EventConstant.UnEquip);
    }

    public void RemouldRequest(double uniqueID, int optionIndex)
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
        RemouldResponse(uniqueID, optionIndex);
    }

    public void RemouldResponse(double uniqueID, int optionIndex)
    {
        GameEventHandler.Messenger.DispatchEvent(EventConstant.Remould, uniqueID);
    }
}