using System;
using System.Collections.Generic;
using AVOSCloud;
using System.Threading.Tasks;

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
        EquipClass equip = new EquipClass();
        equip.UniqueID = CommonHandler.GetUniqueID();
        equip.EquipID = iEquipID;
        DataManager.Instance.EquipData.EquipList.Add(equip);
        GameEventHandler.Messenger.DispatchEvent(EventConstant.ItemUpdate);
    }

    /*
     * 新增道具
     */
    public void ItemNotify(int iItemID)
    {
        ItemClass itemclass = new ItemClass();
        itemclass.ItemID = iItemID;
        DataManager.Instance.ItemData.ItemList.Add(itemclass);
        GameEventHandler.Messenger.DispatchEvent(EventConstant.ItemUpdate);
    }

    /*
     * 注册
     */
    public void CheckRegisterRequest(string name, string password)
    {
        AVUser.Query.WhereEqualTo("username", name).CountAsync().ContinueWith(t =>
        {
            int iCount = t.Result;
            CheckUserResponse response = new CheckUserResponse();
            response.ID = MessageConstant.CHECK_REGISTER_RESPONSE;
            if (t.IsFaulted || t.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
            }
            else if (t.IsCompleted)
            {
                if (iCount > 0)
                {
                    response.iResponseId = ErrorConstant.ERROR_100005;
                }
                else
                {
                    response.name = name;
                    response.password = password;
                }
            }
            MessageQueueManager.Instance.AddMessage(response);
        });
    }

    public void RegisterRequest(CheckUserResponse stCheckUserResponse)
    {
        if (stCheckUserResponse.iResponseId == 0)
        {
            AVUser user = new AVUser();
            user.Username = stCheckUserResponse.name;
            user.Password = stCheckUserResponse.password;
            user.SignUpAsync().ContinueWith(t =>
            {
                RegisterResponse response = new RegisterResponse();
                response.ID = MessageConstant.REGISTER_RESPONSE;
                if (t.IsFaulted || t.IsCanceled)
                {
                    response.iResponseId = ErrorConstant.ERROR_100001;
                }
                else if (t.IsCompleted)
                {
                    response.uid = user.ObjectId;
                    response.name = stCheckUserResponse.name;
                    response.password = stCheckUserResponse.password;
                }
                MessageQueueManager.Instance.AddMessage(response);
            });
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(stCheckUserResponse.iResponseId));
        }
    }

    public void RegisterResponse(RegisterResponse response)
    {
        if (response.iResponseId == 0)
        {
            GameEventHandler.Messenger.DispatchEvent(EventConstant.Register, response);
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
        }
    }

    /*
     * 登陆
     */
    public void CheckLoginRequest(string name, string password)
    {
        AVUser.Query.WhereEqualTo("username", name).CountAsync().ContinueWith(t =>
        {
            int iCount = t.Result;
            CheckUserResponse response = new CheckUserResponse();
            response.ID = MessageConstant.CHECK_LOGIN_RESPONSE;
            if (t.IsFaulted || t.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
            }
            else if (t.IsCompleted)
            {
                if (iCount == 1)
                {
                    response.name = name;
                    response.password = password;
                }
                else
                {
                    response.iResponseId = ErrorConstant.ERROR_100006;
                }
            }
            MessageQueueManager.Instance.AddMessage(response);
        });
    }

    public void LoginRequest(CheckUserResponse stCheckUserResponse)
    {
        if (stCheckUserResponse.iResponseId == 0)
        {
            AVUser.LogInAsync(stCheckUserResponse.name, stCheckUserResponse.password).ContinueWith(t =>
            {
                LoginResponse response = new LoginResponse();
                response.ID = MessageConstant.LOGIN_RESPONSE;
                if (t.IsFaulted || t.IsCanceled)
                {
                    response.iResponseId = ErrorConstant.ERROR_100007;
                }
                else if (t.IsCompleted)
                {
                    response.name = stCheckUserResponse.name;
                    response.password = stCheckUserResponse.password;
                }
                MessageQueueManager.Instance.AddMessage(response);
            });
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(stCheckUserResponse.iResponseId));
        }
    }

    public void LoginResponse(LoginResponse response)
    {
        if (response.iResponseId == 0)
        {
            AVCloud.GetServerDateTime().ContinueWith(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                {
                }
                else if (t.IsCompleted)
                {
                    DateTime time = t.Result;
                    DataManager.Instance.ServerTime = CommonHandler.ConvertDateTimeInt(time);
                    CurrentTimeHandler.Instance.StartTimer();
                }
            });

            AVQuery<AVObject> query = new AVQuery<AVObject>("Hero");
            for (int iIndex = 0; iIndex < 2; ++iIndex)
            {
                string szHero = "hero" + iIndex;
                szHero = AVUser.CurrentUser.Get<string>(szHero);
                query.GetAsync(szHero).ContinueWith(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                    }
                    else if (t.IsCompleted)
                    {
                        AVObject role = t.Result;
                        RoleClass myRole = new RoleClass();
                        myRole.UniqueID = role.ObjectId;
                        myRole.Name = role.Get<string>("name");
                        myRole.Level = role.Get<int>("level");
                        myRole.Job = role.Get<int>("job");
                        myRole.Exp = role.Get<double>("exp");
                        myRole.Gold = role.Get<double>("gold");
                        myRole.Money = role.Get<double>("money");
                        myRole.MonsterIndex = role.Get<int>("monsterIndex");
                        myRole.Equips = role.Get<string>("equips");
                        myRole.Items = role.Get<string>("items");
                        myRole.Skills = role.Get<string>("skills");
                        myRole.CreateTime = role.Get<long>("createTime");
                        myRole.LastOffLineTime = role.Get<long>("lastLoginTime");
                        DataManager.Instance.RoleData.RoleList.Add(myRole);
                    }
                });
            }

            GameEventHandler.Messenger.DispatchEvent(EventConstant.Login, response);
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
        }
    }

    /*
     * 创角
     */
    public void CreatRoleRequest(string roleName, int iJob)
    {
        AVObject hero = new AVObject("Hero")
        {
            {"name", roleName},
            {"level", 0},
            {"job", iJob},
            {"exp", 0},
            {"gold", 0},
            {"money", 0},
            {"monsterIndex", 1},
            {"equips", ""},
            {"items", ""},
            {"skills", ""},
            {"createTime", DataManager.Instance.ServerTime},
            {"lastLoginTime", 0},
        };
        hero.SaveAsync().ContinueWith(t =>
        {
            CreatRoleResponse response = new CreatRoleResponse();
            response.ID = MessageConstant.CREAT_ROLE_RESPONSE;
            if (t.IsFaulted || t.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
            }
            else if (t.IsCompleted)
            {
                response.role = new RoleClass();
                response.role.UniqueID = hero.ObjectId;
                response.role.Name = hero.Get<string>("name");
                response.role.Level = hero.Get<int>("level");
                response.role.Job = hero.Get<int>("job");
                response.role.Exp = hero.Get<double>("exp");
                response.role.Gold = hero.Get<double>("gold");
                response.role.Money = hero.Get<double>("money");
                response.role.MonsterIndex = hero.Get<int>("monsterIndex");
                response.role.Equips = hero.Get<string>("equips");
                response.role.Items = hero.Get<string>("items");
                response.role.Skills = hero.Get<string>("skills");
                response.role.CreateTime = hero.Get<long>("createTime");
                response.role.LastOffLineTime = hero.Get<long>("lastLoginTime");
            }
            MessageQueueManager.Instance.AddMessage(response);
        });
    }

    public void CreatRoleResponse(CreatRoleResponse response)
    {
        if (response.iResponseId == 0)
        {
            DataManager.Instance.RoleData.RoleList.Add(response.role);
            GameEventHandler.Messenger.DispatchEvent(EventConstant.CreatRole, response);

            if (response.role.Job == 1)
            {
                AVUser.CurrentUser["hero0"] = response.role.UniqueID;
            }
            else if (response.role.Job == 2)
            {
                AVUser.CurrentUser["hero1"] = response.role.UniqueID;
            }
            AVUser.CurrentUser.SaveAsync();
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
        }
    }

    /*
     * 选角开始游戏
     */
    public void StartGameRequest(string uniqueID)
    {
        RoleClass curRole = new RoleClass();
        for (int iIndex = 0; iIndex < DataManager.Instance.RoleData.RoleList.Count; ++iIndex)
        {
            RoleClass role = DataManager.Instance.RoleData.RoleList[iIndex];
            if (role.UniqueID == uniqueID)
            {
                curRole = role;
                break;
            }
        }

        List<string> equipList = CommonHandler.ConvertString2List(curRole.Equips);
        AVQuery<AVObject> equipQuery = new AVQuery<AVObject>("Equip").WhereContainedIn("objectId", equipList);
        equipQuery.FindAsync().ContinueWith(t =>
        {
            if (t.IsFaulted || t.IsCanceled)
            {
            }
            else if (t.IsCompleted)
            {
                IEnumerable<AVObject> equips = t.Result;
                foreach(AVObject equip in equips)
                {
                    EquipClass myEquip = new EquipClass();
                    myEquip.UniqueID = equip.ObjectId;
                    myEquip.EquipID = equip.Get<int>("equipId");
                    myEquip.EquipPos = equip.Get<int>("equipPos");
                    myEquip.Level = equip.Get<int>("level");
                    myEquip.RemouldOptionList[0] = equip.Get<int>("remould0");
                    myEquip.RemouldOptionList[1] = equip.Get<int>("remould1");
                    myEquip.RemouldOptionList[2] = equip.Get<int>("remould2");
                    myEquip.RemouldOptionList[3] = equip.Get<int>("remould3");
                    myEquip.RemouldOptionList[4] = equip.Get<int>("remould4");
                    DataManager.Instance.EquipData.EquipList.Add(myEquip);
                }
            }

            List<string> itemList = CommonHandler.ConvertString2List(curRole.Items);
            AVQuery<AVObject> itemQuery = new AVQuery<AVObject>("Item").WhereContainedIn("objectId", itemList);
            return itemQuery.FindAsync();
        }).Unwrap().ContinueWith(r =>
        {
            if (r.IsFaulted || r.IsCanceled)
            {
            }
            else if (r.IsCompleted)
            {
                IEnumerable<AVObject> items = r.Result;
                foreach (AVObject item in items)
                {
                    ItemClass myItem = new ItemClass();
                    myItem.UniqueID = item.ObjectId;
                    myItem.ItemID = item.Get<int>("itemId");
                    myItem.Count = item.Get<int>("count");
                    DataManager.Instance.ItemData.ItemList.Add(myItem);
                }
            } 
            
            List<string> skillList = CommonHandler.ConvertString2List(curRole.Skills);
            AVQuery<AVObject> skillQuery = new AVQuery<AVObject>("Skill").WhereContainedIn("objectId", skillList);
            return skillQuery.FindAsync();
        }).Unwrap().ContinueWith(s =>
        {
            BaseResponse response = new BaseResponse();
            response.ID = MessageConstant.START_GAME_RESPONSE;
            if (s.IsFaulted || s.IsCanceled)
            {
            }
            else if (s.IsCompleted)
            {
                IEnumerable<AVObject> skills = s.Result;
                foreach (AVObject skill in skills)
                {
                    SkillClass mySkill = new SkillClass();
                    mySkill.UniqueID = skill.ObjectId;
                    mySkill.SkillType = skill.Get<int>("skillType");
                    mySkill.SkillID = skill.Get<int>("skillId");
                    mySkill.Pos = skill.Get<int>("pos");
                    mySkill.Level = skill.Get<int>("level");
                    mySkill.NextExp = skill.Get<int>("nextExp");
                    DataManager.Instance.SkillData.SkillDataList.Add(mySkill);
                }
            }

            DataManager.Instance.CurrentRole = curRole;
            AttrHandler.CalculateTotalAttr();
            MessageQueueManager.Instance.AddMessage(response);
        });
    }

    public void StartGameResponse()
    {
        GameEventHandler.Messenger.DispatchEvent(EventConstant.ChooseRole);
    }

    /*
     * 装备
     */
    public void EquipRequest(EquipPosStrategyStruct equipPosStrategy)
    {
        EquipResponse response = new EquipResponse();
        response.ID = MessageConstant.EQUIP_RESPONSE;
        AVQuery<AVObject> equipQuery = new AVQuery<AVObject>("Equip").WhereContainedIn("objectId", equipPosStrategy.UniqueIDList);
        equipQuery.FindAsync().ContinueWith(t =>
        {
            Task task = Task.FromResult(0);
            if (t.IsFaulted || t.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
            }
            else if (t.IsCompleted)
            {
                IEnumerable<AVObject> equips = t.Result;
                foreach (AVObject equip in equips)
                {
                    int iPos = -1;
                    for(int iIndex = 0; iIndex < equipPosStrategy.UniqueIDList.Count; ++iIndex)
                    {
                        if (equip.ObjectId == equipPosStrategy.UniqueIDList[iIndex])
                        {
                            iPos = equipPosStrategy.EquipPosList[iIndex];
                        }
                    }
                    if (iPos > -1)
                    {
                        equip["equipPos"] = iPos;
                        task = task.ContinueWith(_ =>
                        {
                            return equip.SaveAsync().ContinueWith(s => 
                            {
                                if (s.IsFaulted || s.IsCanceled)
                                {
                                    response.iResponseId = ErrorConstant.ERROR_100001;
                                }
                            });
                        }).Unwrap();
                    }
                }
            }
            return task;
        }).Unwrap().ContinueWith(r =>
        {
            if (r.IsFaulted || r.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
            }
            else if (r.IsCompleted)
            {
                response.equipPosStrategy = equipPosStrategy;
                MessageQueueManager.Instance.AddMessage(response);
            }
        });
    }

    public void EquipResponse(EquipResponse response)
    {
        if (response.iResponseId == 0)
        {
            foreach (EquipClass equip in DataManager.Instance.EquipData.EquipList)
            {
                for (int iIndex = 0; iIndex < response.equipPosStrategy.UniqueIDList.Count; ++iIndex)
                {
                    if (equip.UniqueID == response.equipPosStrategy.UniqueIDList[iIndex])
                    {
                        equip.EquipPos = response.equipPosStrategy.EquipPosList[iIndex];
                    }
                }
            }
            AttrHandler.CalculateTotalAttr();
            GameEventHandler.Messenger.DispatchEvent(EventConstant.Equip);
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
        }
    }

    /*
     * 卸载装备
     */
    public void UnEquipRequest(string uniqueID)
    {
        UnEquipResponse response = new UnEquipResponse();
        response.ID = MessageConstant.UN_EQUIP_RESPONSE;
        AVQuery<AVObject> equipQuery = new AVQuery<AVObject>("Equip");
        equipQuery.GetAsync(uniqueID).ContinueWith(t =>
        {
            if (t.IsFaulted || t.IsCanceled)
            {
            }
            else if (t.IsCompleted)
            {
                AVObject equip = t.Result;
                equip["equipPos"] = 0;
                equip.SaveAsync().ContinueWith(s =>
                {
                    if (s.IsFaulted || s.IsCanceled)
                    {
                        response.iResponseId = ErrorConstant.ERROR_100001;
                    }
                    else if (s.IsCompleted)
                    {
                        response.uniqueId = uniqueID;
                        MessageQueueManager.Instance.AddMessage(response);
                    }
                });
            }
        });
    }

    public void UnEquipResponse(UnEquipResponse stUnEquipResponse)
    {
        for (int iIndex = 0; iIndex < DataManager.Instance.EquipData.EquipList.Count; ++iIndex)
        {
            EquipClass equip = DataManager.Instance.EquipData.EquipList[iIndex];
            if (equip.UniqueID == stUnEquipResponse.uniqueId)
            {
                equip.EquipPos = 0;
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
        for (int iIndex = 0; iIndex < DataManager.Instance.EquipData.EquipList.Count; ++iIndex)
        {
            EquipClass equip = DataManager.Instance.EquipData.EquipList[iIndex];
            if (equip.UniqueID == uniqueID)
            {
                equip.Level++;
                equip.RemouldOptionList.Add(optionIndex);
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