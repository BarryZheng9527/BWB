﻿using System;
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
        AVQuery<AVObject> heroQuery = new AVQuery<AVObject>("Hero");
        heroQuery.GetAsync(DataManager.Instance.CurrentRole.UniqueID).ContinueWith(t =>
        {
            GoldNotify response = new GoldNotify();
            response.ID = MessageConstant.GOLD_NOTIFY;
            if (t.IsFaulted || t.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
            }
            else if (t.IsCompleted)
            {
                AVObject hero = t.Result;
                double gold = hero.Get<double>("gold");
                gold += dGold;
                hero["gold"] = gold;
                hero.SaveAsync().ContinueWith(s =>
                {
                    if (s.IsFaulted || s.IsCanceled)
                    {
                        response.iResponseId = ErrorConstant.ERROR_100001;
                    }
                    else if (s.IsCompleted)
                    {
                        response.addGold = dGold;
                        MessageQueueManager.Instance.AddMessage(response);
                    }
                });
            }
        });
    }

    public void GoldNotifyResponse(GoldNotify response)
    {
        if (response.iResponseId == 0)
        {
            DataManager.Instance.CurrentRole.Gold += response.addGold;
            GameEventHandler.Messenger.DispatchEvent(EventConstant.GoldUpdate);
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
        }
    }

    /*
     * 经验增量
     */
    public void ExpNotify(double dExp)
    {
        AVQuery<AVObject> heroQuery = new AVQuery<AVObject>("Hero");
        heroQuery.GetAsync(DataManager.Instance.CurrentRole.UniqueID).ContinueWith(t =>
        {
            ExpNotify response = new ExpNotify();
            response.ID = MessageConstant.EXP_NOTIFY;
            if (t.IsFaulted || t.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
            }
            else if (t.IsCompleted)
            {
                AVObject hero = t.Result;
                double exp = hero.Get<double>("exp");
                int level = hero.Get<int>("level");
                exp += dExp;
                hero["exp"] = exp;
                int iNewLevel = LevelConfig.Instance.GetLevelFromExp(exp);
                if (iNewLevel > level)
                {
                    hero["level"] = iNewLevel;
                }
                hero.SaveAsync().ContinueWith(s =>
                {
                    if (s.IsFaulted || s.IsCanceled)
                    {
                        response.iResponseId = ErrorConstant.ERROR_100001;
                    }
                    else if (s.IsCompleted)
                    {
                        response.addExp = dExp;
                        response.level = hero.Get<int>("level");
                        MessageQueueManager.Instance.AddMessage(response);
                    }
                });
            }
        });
    }

    public void ExpNotifyResponse(ExpNotify response)
    {
        if (response.iResponseId == 0)
        {
            DataManager.Instance.CurrentRole.Exp += response.addExp;
            GameEventHandler.Messenger.DispatchEvent(EventConstant.ExpUpdate);
            if (response.level > DataManager.Instance.CurrentRole.Level)
            {
                DataManager.Instance.CurrentRole.Level = response.level;
                GameEventHandler.Messenger.DispatchEvent(EventConstant.LevelUpdate);
                AttrHandler.CalculateTotalAttr();
            }
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
        }
    }

    /*
     * 新增装备
     */
    public void EquipNotify(int iEquipID)
    {
        EquipNotify response = new EquipNotify();
        response.ID = MessageConstant.EQUIP_NOTIFY;
        AVObject equip = new AVObject("Equip")
        {
            {"equipId", iEquipID},
            {"equipPos", 0},
            {"level", 0},
            {"remould0", 0},
            {"remould1", 0},
            {"remould2", 0},
            {"remould3", 0},
            {"remould4", 0},
        };
        equip.SaveAsync().ContinueWith(t =>
        {
            if (t.IsFaulted || t.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
            }
            else if (t.IsCompleted)
            {
                response.equip = new EquipClass();
                response.equip.UniqueID = equip.ObjectId;
                response.equip.EquipID = equip.Get<int>("equipId");
                response.equip.EquipPos = equip.Get<int>("equipPos");
                response.equip.Level = equip.Get<int>("level");
                response.equip.RemouldOptionList[0] = equip.Get<int>("remould0");
                response.equip.RemouldOptionList[1] = equip.Get<int>("remould1");
                response.equip.RemouldOptionList[2] = equip.Get<int>("remould2");
                response.equip.RemouldOptionList[3] = equip.Get<int>("remould3");
                response.equip.RemouldOptionList[4] = equip.Get<int>("remould4");
            }

            AVQuery<AVObject> heroQuery = new AVQuery<AVObject>("Hero");
            return heroQuery.GetAsync(DataManager.Instance.CurrentRole.UniqueID);
        }).Unwrap().ContinueWith(s =>
        {
            if (s.IsFaulted || s.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
            }
            else if (s.IsCompleted)
            {
                AVObject hero = s.Result;
                string curEquips = hero.Get<string>("equips");
                List<string> equipList = CommonHandler.ConvertString2List(curEquips);
                equipList.Add(response.equip.UniqueID);
                string newEquips = CommonHandler.ConvertList2String(equipList);
                hero["equips"] = newEquips;
                hero.SaveAsync().ContinueWith(r =>
                {
                    if (r.IsFaulted || r.IsCanceled)
                    {
                        response.iResponseId = ErrorConstant.ERROR_100001;
                    }
                    else if (r.IsCompleted)
                    {
                        MessageQueueManager.Instance.AddMessage(response);
                    }
                });
            }
        });
    }

    public void EquipNotifyResponse(EquipNotify response)
    {
        if (response.iResponseId == 0)
        {
            DataManager.Instance.EquipData.EquipList.Add(response.equip);
            GameEventHandler.Messenger.DispatchEvent(EventConstant.ItemUpdate);
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
        }
    }

    /*
     * 下发道具
     */
    public void ItemNotify(int iItemID, int iCount = 1)
    {
        bool hadItem = false;
        string uniqueId = "";
        foreach(ItemClass item in DataManager.Instance.ItemData.ItemList)
        {
            if (item.ItemID == iItemID)
            {
                hadItem = true;
                uniqueId = item.UniqueID;
                break;
            }
        }
        ItemNotify response = new ItemNotify();
        response.ID = MessageConstant.ITEM_NOTIFY;
        if (hadItem)
        {
            AVQuery<AVObject> itemQuery = new AVQuery<AVObject>("Item");
            itemQuery.GetAsync(uniqueId).ContinueWith(t =>
            {
                if (t.IsFaulted || t.IsCanceled)
                {
                    response.iResponseId = ErrorConstant.ERROR_100001;
                }
                else if (t.IsCompleted)
                {
                    AVObject item = t.Result;
                    int count = item.Get<int>("count");
                    count += iCount;
                    item["count"] = count;
                    item.SaveAsync().ContinueWith(s =>
                    {
                        if (s.IsFaulted || s.IsCanceled)
                        {
                            response.iResponseId = ErrorConstant.ERROR_100001;
                        }
                        else if (s.IsCompleted)
                        {
                            response.item = new ItemClass();
                            response.item.UniqueID = item.ObjectId;
                            response.item.ItemID = item.Get<int>("itemId");
                            response.item.Count = item.Get<int>("count");
                            MessageQueueManager.Instance.AddMessage(response);
                        }
                    });
                }
            });
        }
        else
        {
            AVObject addItem = new AVObject("Item")
            {
                {"itemId", iItemID},
                {"count", iCount},
            };
            addItem.SaveAsync().ContinueWith(r =>
            {
                if (r.IsFaulted || r.IsCanceled)
                {
                    response.iResponseId = ErrorConstant.ERROR_100001;
                }
                else if (r.IsCompleted)
                {
                    response.item = new ItemClass();
                    response.item.UniqueID = addItem.ObjectId;
                    response.item.ItemID = addItem.Get<int>("itemId");
                    response.item.Count = addItem.Get<int>("count");
                }

                AVQuery<AVObject> heroQuery = new AVQuery<AVObject>("Hero");
                return heroQuery.GetAsync(DataManager.Instance.CurrentRole.UniqueID);
            }).Unwrap().ContinueWith(q =>
            {
                if (q.IsFaulted || q.IsCanceled)
                {
                    response.iResponseId = ErrorConstant.ERROR_100001;
                }
                else if (q.IsCompleted)
                {
                    AVObject hero = q.Result;
                    string curItems = hero.Get<string>("items");
                    List<string> itemList = CommonHandler.ConvertString2List(curItems);
                    itemList.Add(response.item.UniqueID);
                    string newItems = CommonHandler.ConvertList2String(itemList);
                    hero["items"] = newItems;
                    hero.SaveAsync().ContinueWith(p =>
                    {
                        if (p.IsFaulted || p.IsCanceled)
                        {
                            response.iResponseId = ErrorConstant.ERROR_100001;
                        }
                        else if (p.IsCompleted)
                        {
                            MessageQueueManager.Instance.AddMessage(response);
                        }
                    });
                }
            });
        }
    }

    public void ItemNotifyResponse(ItemNotify response)
    {
        if (response.iResponseId == 0)
        {
            bool newItem = true;
            foreach (ItemClass item in DataManager.Instance.ItemData.ItemList)
            {
                if (item.UniqueID == response.item.UniqueID)
                {
                    newItem = false;
                    item.Count = response.item.Count;
                    break;
                }
            }
            if (newItem)
            {
                DataManager.Instance.ItemData.ItemList.Add(response.item);
            }
            GameEventHandler.Messenger.DispatchEvent(EventConstant.ItemUpdate);
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
        }
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
        AVQuery<AVObject> equipQuery = new AVQuery<AVObject>("Equip");
        equipQuery.GetAsync(uniqueID).ContinueWith(t =>
        {
            UnEquipResponse response = new UnEquipResponse();
            response.ID = MessageConstant.UN_EQUIP_RESPONSE;
            if (t.IsFaulted || t.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
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

    public void UnEquipResponse(UnEquipResponse response)
    {
        if (response.iResponseId == 0)
        {
            for (int iIndex = 0; iIndex < DataManager.Instance.EquipData.EquipList.Count; ++iIndex)
            {
                EquipClass equip = DataManager.Instance.EquipData.EquipList[iIndex];
                if (equip.UniqueID == response.uniqueId)
                {
                    equip.EquipPos = 0;
                    break;
                }
            }
            AttrHandler.CalculateTotalAttr();
            GameEventHandler.Messenger.DispatchEvent(EventConstant.UnEquip);
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
        }
    }

    /*
     * 装备改造
     */
    public void RemouldRequest(string uniqueID, int optionIndex)
    {
        RemouldEquipResponse response = new RemouldEquipResponse();
        response.ID = MessageConstant.REMOULD_EQUIP_RESPONSE;
        AVQuery<AVObject> equipQuery = new AVQuery<AVObject>("Equip");
        equipQuery.GetAsync(uniqueID).ContinueWith(t =>
        {
            if (t.IsFaulted || t.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
            }
            else if (t.IsCompleted)
            {
                AVObject equip = t.Result;
                int iLevel = equip.Get<int>("level");
                string remouldIndex = "remould" + iLevel;
                iLevel++;
                equip["level"] = iLevel;
                equip[remouldIndex] = optionIndex;
                equip.SaveAsync().ContinueWith(s =>
                {
                    if (s.IsFaulted || s.IsCanceled)
                    {
                        response.iResponseId = ErrorConstant.ERROR_100001;
                    }
                    else if (s.IsCompleted)
                    {
                        response.uniqueId = uniqueID;
                        response.optionIndex = optionIndex;
                        MessageQueueManager.Instance.AddMessage(response);
                    }
                });
            }
        });
    }

    public void RemouldResponse(RemouldEquipResponse response)
    {
        if (response.iResponseId == 0)
        {
            for (int iIndex = 0; iIndex < DataManager.Instance.EquipData.EquipList.Count; ++iIndex)
            {
                EquipClass equip = DataManager.Instance.EquipData.EquipList[iIndex];
                if (equip.UniqueID == response.uniqueId)
                {
                    equip.RemouldOptionList[equip.Level] = response.optionIndex;
                    equip.Level++;
                    OptionStruct optionStruct = RemouldConfig.Instance.GetOptionStructFromID(response.optionIndex);
                    double addGold = 0 - optionStruct.Cost;
                    GoldNotify(addGold);
                    break;
                }
            }
            AttrHandler.CalculateTotalAttr();
            GameEventHandler.Messenger.DispatchEvent(EventConstant.Remould, response.uniqueId);
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
        }
    }

    /*
     * 技能获取
     */
    public void SkillGetRequest(int iSkillID)
    {
        AVObject skill = new AVObject("Skill")
        {
            {"skillId", iSkillID},
            {"pos", 0},
            {"level", 0},
            {"nextExp", 0},
        };
        skill.SaveAsync().ContinueWith(t =>
        {
            SkillGetResponse response = new SkillGetResponse();
            response.ID = MessageConstant.SKILL_GET_RESPONSE;
            if (t.IsFaulted || t.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
            }
            else if (t.IsCompleted)
            {
                response.skill = new SkillClass();
                response.skill.UniqueID = skill.ObjectId;
                response.skill.SkillID = skill.Get<int>("skillId");
                response.skill.Pos = skill.Get<int>("pos");
                response.skill.Level = skill.Get<int>("level");
                response.skill.NextExp = skill.Get<int>("nextExp");
            }
            MessageQueueManager.Instance.AddMessage(response);
        });
    }

    public void SkillGetResponse(SkillGetResponse response)
    {
        if (response.iResponseId == 0)
        {
            DataManager.Instance.SkillData.SkillDataList.Add(response.skill);

            SkillStruct skillStruct = SkillConfig.Instance.GetSkill(response.skill.SkillID);
            double addGold = 0 - skillStruct.Gold;
            GoldNotify(addGold);

            AttrHandler.CalculateTotalAttr();
            GameEventHandler.Messenger.DispatchEvent(EventConstant.SkillUpdate);
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
        }
    }

    /*
     * 技能升级
     */
    public void SkillLevelUpRequest(string uniqueID)
    {
        AVQuery<AVObject> skillQuery = new AVQuery<AVObject>("Skill");
        skillQuery.GetAsync(uniqueID).ContinueWith(t =>
        {
            SkillLevelUpResponse response = new SkillLevelUpResponse();
            response.ID = MessageConstant.SKILL_LEVEL_UP_RESPONSE;
            if (t.IsFaulted || t.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
            }
            else if (t.IsCompleted)
            {
                AVObject skill = t.Result;
                int iLevel = skill.Get<int>("level");
                iLevel++;
                skill["level"] = iLevel;
                skill["nextExp"] = 0;
                skill.SaveAsync().ContinueWith(s =>
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

    public void SkillLevelUpResponse(SkillLevelUpResponse response)
    {
        if (response.iResponseId == 0)
        {
            foreach (SkillClass skill in DataManager.Instance.SkillData.SkillDataList)
            {
                if (skill.UniqueID == response.uniqueId)
                {
                    skill.Level++;
                    skill.NextExp = 0;
                    break;
                }
            }
            AttrHandler.CalculateTotalAttr();
            GameEventHandler.Messenger.DispatchEvent(EventConstant.SkillUpdate);
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
        }
    }

    /*
     * 技能装载
     */
    public void SkillEquipRequest(string equipSkillUniqueId, int iPos, string unEquipSkillUniqueId)
    {
        string [] uniqueIdList = {equipSkillUniqueId};
        if (unEquipSkillUniqueId != "")
        {
            uniqueIdList[1] = unEquipSkillUniqueId;
        }
        SkillEquipResponse response = new SkillEquipResponse();
        response.ID = MessageConstant.SKILL_EQUIP_RESPONSE;
        AVQuery<AVObject> skillQuery = new AVQuery<AVObject>("Skill").WhereContainedIn("objectId", uniqueIdList);
        skillQuery.FindAsync().ContinueWith(t =>
        {
            Task task = Task.FromResult(0);
            if (t.IsFaulted || t.IsCanceled)
            {
                response.iResponseId = ErrorConstant.ERROR_100001;
            }
            else if (t.IsCompleted)
            {
                IEnumerable<AVObject> skills = t.Result;
                foreach (AVObject skill in skills)
                {
                    if (equipSkillUniqueId == skill.ObjectId)
                    {
                        skill["pos"] = iPos;
                    }
                    else if (unEquipSkillUniqueId != "" && unEquipSkillUniqueId == skill.ObjectId)
                    {
                        skill["pos"] = 0;
                    }
                    task = task.ContinueWith(_ =>
                    {
                        return skill.SaveAsync().ContinueWith(s =>
                        {
                            if (s.IsFaulted || s.IsCanceled)
                            {
                                response.iResponseId = ErrorConstant.ERROR_100001;
                            }
                        });
                    }).Unwrap();
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
                response.equipUniqueId = equipSkillUniqueId;
                response.iPos = iPos;
                response.unEquipUniqueId = unEquipSkillUniqueId;
                MessageQueueManager.Instance.AddMessage(response);
            }
        });
    }

    public void SkillEquipResponse(SkillEquipResponse response)
    {
        if (response.iResponseId == 0)
        {
            foreach (SkillClass skillClass in DataManager.Instance.SkillData.SkillDataList)
            {
                if (skillClass.UniqueID == response.equipUniqueId)
                {
                    skillClass.Pos = response.iPos;
                }
                if (response.unEquipUniqueId != "" && skillClass.UniqueID == response.unEquipUniqueId)
                {
                    skillClass.Pos = 0;
                }
            }
            GameEventHandler.Messenger.DispatchEvent(EventConstant.SkillEquipUpdate);
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
        }
    }

    /*
     * 技能经验增加
     */
    public void SkillExpAdd(int iSkillID)
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

    public void SkillExpSave()
    {
        List<string> uniqueIdList = new List<string>();
        foreach (SkillClass skillClass in DataManager.Instance.SkillData.SkillDataList)
        {
            if (skillClass.NextExp > 0)
            {
                uniqueIdList.Add(skillClass.UniqueID);
            }
        }
        AVQuery<AVObject> equipQuery = new AVQuery<AVObject>("Skill").WhereContainedIn("objectId", uniqueIdList);
        equipQuery.FindAsync().ContinueWith(t =>
        {
            if (t.IsFaulted || t.IsCanceled)
            {
            }
            else if (t.IsCompleted)
            {
                IEnumerable<AVObject> skills = t.Result;
                foreach (AVObject skill in skills)
                {
                    foreach (SkillClass skillClass0 in DataManager.Instance.SkillData.SkillDataList)
                    {
                        if (skillClass0.UniqueID == skill.ObjectId)
                        {
                            skill["nextExp"] = skillClass0.NextExp;
                            skill.SaveAsync();
                            break;
                        }
                    }
                }
            }
        });
    }

    /*
     * 更新关卡
     */
    public void MonsterIndexRequest(int iMonsterIndex, bool bWin = false)
    {
        if (bWin)
        {
            MonsterStruct curMonster = MonsterConfig.Instance.GetMonster(iMonsterIndex);
            MonsterStruct nextMonster = MonsterConfig.Instance.GetMonster(iMonsterIndex + 1);
            if (nextMonster.Index > 0 && DataManager.Instance.AutoMonster)
            {
                AVQuery<AVObject> heroQuery = new AVQuery<AVObject>("Hero");
                heroQuery.GetAsync(DataManager.Instance.CurrentRole.UniqueID).ContinueWith(t =>
                {
                    MonsterIndexResponse response = new MonsterIndexResponse();
                    response.ID = MessageConstant.MONSTER_INDEX_RESPONSE;
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        response.iResponseId = ErrorConstant.ERROR_100001;
                    }
                    else if (t.IsCompleted)
                    {
                        AVObject hero = t.Result;
                        hero["monsterIndex"] = nextMonster.Index;
                        hero.SaveAsync().ContinueWith(s =>
                        {
                            if (s.IsFaulted || s.IsCanceled)
                            {
                                response.iResponseId = ErrorConstant.ERROR_100001;
                            }
                            else if (s.IsCompleted)
                            {
                                response.curMonsterIndex = iMonsterIndex;
                                response.nextMonsterIndex = nextMonster.Index;
                                MessageQueueManager.Instance.AddMessage(response);
                            }
                        });
                    }
                });
            }
        }
        else if (DataManager.Instance.AutoMonster)
        {
            DataManager.Instance.AutoMonster = false;
        }
    }

    public void MonsterIndexResponse(MonsterIndexResponse response)
    {
        DataManager.Instance.CurrentRole.MonsterIndex = response.nextMonsterIndex;

        MonsterStruct curMonster = MonsterConfig.Instance.GetMonster(response.curMonsterIndex);
        double addGold = curMonster.Gold;
        GoldNotify(addGold);
        double addExp = curMonster.Exp;
        ExpNotify(addExp);
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

        BattleManager.Instance.BattleManagerStart();
    }
}