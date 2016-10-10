using System;
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

    /*
     * 金币增量
     */
    public void GoldNotify(double dGold)
    {
        DataManager.Instance.CurrentRole.Gold += dGold;
        GameEventHandler.Messenger.DispatchEvent(EventConstant.GoldUpdate);
    }

    /*
     * 经验增量
     */
    public void ExpNotify(double dExp)
    {
        DataManager.Instance.CurrentRole.Exp += dExp;
        GameEventHandler.Messenger.DispatchEvent(EventConstant.ExpUpdate);
        int iMyLevel = LevelConfig.Instance.GetLevelFromExp(DataManager.Instance.CurrentRole.Exp);
        if (iMyLevel > DataManager.Instance.CurrentRole.Level)
        {
            DataManager.Instance.CurrentRole.Level = iMyLevel;
            GameEventHandler.Messenger.DispatchEvent(EventConstant.LevelUpdate);
            AttrHandler.CalculateTotalAttr();
        }
    }

    /*
     * 新增装备
     */
    public void EquipNotify(int iEquipID)
    {
        ItemClass itemclass = new ItemClass();
        itemclass.ItemType = Constant.EQUIP;
        itemclass.UniqueID = CommonHandler.GetUniqueID();
        itemclass.EquipID = iEquipID;
        DataManager.Instance.ItemData.ItemList.Add(itemclass);
        GameEventHandler.Messenger.DispatchEvent(EventConstant.ItemUpdate);
    }

    /*
     * 新增道具
     */
    public void ItemNotify(int iItemID)
    {
        ItemClass itemclass = new ItemClass();
        itemclass.ItemType = Constant.ITEM;
        itemclass.ItemID = iItemID;
        DataManager.Instance.ItemData.ItemList.Add(itemclass);
        GameEventHandler.Messenger.DispatchEvent(EventConstant.ItemUpdate);
    }

    /*
     * 注册
     */
    public void RegisterRequest(string name, string password)
    {
        RegisterResponse(name, password);
    }

    public void RegisterResponse(string name, string password)
    {
        RegisterResponse response = new RegisterResponse();
        if (name == "bwb" && password == "bwb")
        {
        }
        else
        {
            response.iResponseId = ErrorConstant.ERROR_100005;
        }

        GameEventHandler.Messenger.DispatchEvent(EventConstant.Register, response);
    }

    /*
     * 登陆
     */
    public void LoginRequest(string name, string password)
    {
        LoginResponse(name, password);
    }

    public void LoginResponse(string name, string password)
    {
        LoginResponse response = new LoginResponse();
        if (name == "bwb" && password == "bwb")
        {
            response.name = name;
            response.password = password;

            RoleData roledata = new RoleData();
            List<RoleClass> rolelist = new List<RoleClass>();
            RoleClass roleclass = new RoleClass();
            roleclass.Name = "英雄饶命";
            roleclass.Level = 1;
            roleclass.Job = 1;
            roleclass.Exp = 120;
            roleclass.Gold = 220;
            roleclass.Money = 20;
            roleclass.MonsterIndex = 1;
            rolelist.Add(roleclass);
            roledata.RoleList = rolelist;

            DataManager.Instance.RoleData = roledata;
        }
        else
        {
            response.iResponseId = ErrorConstant.ERROR_100002;
        }

        GameEventHandler.Messenger.DispatchEvent(EventConstant.Login, response);
    }

    /*
     * 选角进入游戏
     */
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
        itemclass.UniqueID = CommonHandler.GetUniqueID();
        itemclass.EquipID = 100001;
        itemclass.EquipPos = 1;
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

    /*
     * 装备
     */
    public void EquipRequest(string uniqueID, int equipPos)
    {
        EquipResponse(uniqueID, equipPos);
    }

    public void EquipResponse(string uniqueID, int equipPos)
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

    /*
     * 卸载装备
     */
    public void UnEquipRequest(string uniqueID)
    {
        UnEquipResponse(uniqueID);
    }

    public void UnEquipResponse(string uniqueID)
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

    /*
     * 装备改造
     */
    public void RemouldRequest(string uniqueID, int optionIndex)
    {
        RemouldResponse(uniqueID, optionIndex);
    }

    public void RemouldResponse(string uniqueID, int optionIndex)
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

    /*
     * 技能获取
     */
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
        double addGold = 0 - skillStruct.Gold;
        GoldNotify(addGold);

        AttrHandler.CalculateTotalAttr();
        GameEventHandler.Messenger.DispatchEvent(EventConstant.SkillUpdate);
    }

    /*
     * 技能升级
     */
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

    /*
     * 技能装载
     */
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

    /*
     * 技能经验增加
     */
    public void SkillExpRequest(int iSkillID)
    {
        SkillExpResponse(iSkillID);
    }

    public void SkillExpResponse(int iSkillID)
    {
        foreach (SkillClass skillClass in DataManager.Instance.SkillData.SkillDataList)
        {
            SkillStruct skillStruct = SkillConfig.Instance.GetSkill(skillClass.SkillID);
            SkillLevelStruct skillLevelStruct = skillStruct.GetSkillLevel(skillClass.Level);
            if (iSkillID == skillLevelStruct.SkillID && skillClass.NextExp < skillLevelStruct.Exp)
            {
                skillClass.NextExp++;
            }
        }

        GameEventHandler.Messenger.DispatchEvent(EventConstant.SkillUpdate);
    }

    /*
     * 更新关卡
     */
    public void MonsterIndexRequest(int iMonsterIndex, bool bWin = false)
    {
        MonsterIndexResponse(iMonsterIndex, bWin);
    }

    public void MonsterIndexResponse(int iMonsterIndex, bool bWin = false)
    {
        if (bWin)
        {
            MonsterStruct curMonster = MonsterConfig.Instance.GetMonster(iMonsterIndex);
            MonsterStruct nextMonster = MonsterConfig.Instance.GetMonster(iMonsterIndex + 1);
            if (nextMonster.Index > 0 && DataManager.Instance.AutoMonster)
            {
                DataManager.Instance.CurrentRole.MonsterIndex = nextMonster.Index;
            }
            else
            {
                DataManager.Instance.CurrentRole.MonsterIndex = iMonsterIndex;
            }
            double addGold = curMonster.Gold;
            if (addGold > 0)
            {
                GoldNotify(addGold);
            }
            double addExp = curMonster.Exp;
            if (addExp > 0)
            {
                ExpNotify(addExp);
            }
            double dRate0 = UnityEngine.Random.Range(1, 101);
            double dRate1 = UnityEngine.Random.Range(1, 101);
            if (dRate0 < (100 * curMonster.EquipRate))
            {
                EquipNotify(curMonster.EquipID);
            }
            if (dRate1 < (100 * curMonster.ItemRate))
            {
                ItemNotify(curMonster.ItemID);
            }
        }
        else
        {
            DataManager.Instance.CurrentRole.MonsterIndex = iMonsterIndex;
            if (DataManager.Instance.AutoMonster)
            {
                DataManager.Instance.AutoMonster = false;
            }
        }
        BattleManager.Instance.BattleManagerStart();
    }
}