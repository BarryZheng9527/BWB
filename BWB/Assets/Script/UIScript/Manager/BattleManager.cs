using FairyGUI;
using System;
using System.Collections.Generic;

public class BattleManager
{
    static private BattleManager instance = null;

    public static BattleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BattleManager();
            }
            return instance;
        }
    }

    private Dictionary<int, double> _DictTotalAttr; //角色战斗属性
    private List<SkillClass> _MySkillList; //角色技能列表
    private double _FireCD; //角色普攻CD

    private MonsterStruct _CurMonster; //当前关卡怪物
    private List<MonsterSkillStruct> _CurMonsterSkillList; //当前怪物技能

    private int iMySkillIndex; //当前技能下标
    private SkillClass _CurSkill = null; //当前技能
    private double _CurSkillCD; //当前技能CD
    private SkillStruct _CurSkillStruct = new SkillStruct();
    private SkillLevelStruct _CurSkillLevelStruct = new SkillLevelStruct();
    private int _CurStatus; //当前状态（0寻路，1普攻，2技能，3定身）
    private int _CurCountDown; //当前状态剩余时间单位

    private int iMonsterSkillIndex; //当前怪物技能下标
    private MonsterSkillStruct _CurMonsterSkill = new MonsterSkillStruct(); //当前怪物技能
    private double _CurMonsterSkillCD; //当前怪物技能CD
    private int _CurMonsterStatus; //当前怪物状态（0寻路，1普攻，2技能，3定身）
    private int _CurMonsterCountDown; //怪物当前状态剩余时间单位

    private double dTime;

    public Dictionary<int, double> DictTotalAttr
    {
        get
        {
            return _DictTotalAttr;
        }
        set
        {
            _DictTotalAttr = value;
        }
    }

    public MonsterStruct CurMonster
    {
        get
        {
            return _CurMonster;
        }
        set
        {
            _CurMonster = value;
        }
    }

    /*
     * 战斗启动
     */
    public void BattleManagerStart()
    {
        dTime = 0;
        InitBattle();
        Timers.inst.Add(0.1f, 0, UpdateBattle);
    }

    /*
     * 单次战斗启动
     */
    private void InitBattle()
    {
        GameEventHandler.Messenger.DispatchEvent(EventConstant.BattleMessage, "[" + dTime + "]" + "单次战斗启动");
        _CurStatus = Constant.BATTLESTATUS0;
        _CurMonsterStatus = Constant.BATTLESTATUS0;
        StatusTransform(_CurStatus, _CurStatus);
        MonsterStatusTransform(_CurMonsterStatus, _CurMonsterStatus);
        GameEventHandler.Messenger.DispatchEvent(EventConstant.InitHpMp);
    }

    /*
     * timer回调
     */
    public void UpdateBattle(object param)
    {
        dTime += 0.1;
        dTime = Math.Round(dTime, 1, MidpointRounding.AwayFromZero);
        _CurCountDown -= 1;
        _CurMonsterCountDown -= 1;
        if (_CurCountDown <= 0)
        {
            if (CalculateStatus(_CurStatus))
            {
                int iNextStatus = NextStatus(_CurStatus);
                StatusTransform(_CurStatus, iNextStatus);
                _CurStatus = iNextStatus;
            }
        }
        if (_CurMonsterCountDown <= 0)
        {
            if (MonsterCalculateStatus(_CurMonsterStatus))
            {
                int iMonsterNextStatus = MonsterNextStatus(_CurMonsterStatus);
                MonsterStatusTransform(_CurMonsterStatus, iMonsterNextStatus);
                _CurMonsterStatus = iMonsterNextStatus;
            }
        }
        if (_CurSkill != null)
        {
            _CurSkillCD -= 0.1;
        }
        if (_CurMonsterSkill.Index > 0)
        {
            _CurMonsterSkillCD -= 0.1;
        }
    }

    /*
     * 结算当前状态
     */
    private bool CalculateStatus(int iCurStatus)
    {
        if (iCurStatus == Constant.BATTLESTATUS0)
        {
            GameEventHandler.Messenger.DispatchEvent(EventConstant.BattleMessage, "[" + dTime + "]" + "寻怪结束");
        }
        else if (iCurStatus == Constant.BATTLESTATUS1)
        {
            double dCostHP = BattleHandler.GetRoleAttack(_DictTotalAttr, _CurMonster);
            _CurMonster.HP -= dCostHP;
            GameEventHandler.Messenger.DispatchEvent(EventConstant.BattleMessage, "[" + dTime + "]" + "角色普攻，怪物掉血" + dCostHP);
            if (_CurMonster.HP <= 0)
            {
                Timers.inst.Remove(UpdateBattle);
                GameEventHandler.Messenger.DispatchEvent(EventConstant.BattleMessage, "[" + dTime + "]" + "怪物死亡，结算奖励");
                NetManager.Instance.MonsterIndexRequest(_CurMonster.Index, true);
                return false;
            }
            GameEventHandler.Messenger.DispatchEvent(EventConstant.MonsterHPUpdate, _CurMonster.HP);
        }
        else if (iCurStatus == Constant.BATTLESTATUS2)
        {
            //使用次数
            NetManager.Instance.SkillExpAdd(_CurSkill.SkillID);
            //buff
            if (_CurSkillStruct.BuffAttrType1 > 0 && _CurSkillLevelStruct.BuffAttrValue1 > 0)
            {
                _DictTotalAttr[_CurSkillStruct.BuffAttrType1] += _CurSkillLevelStruct.BuffAttrValue1;
            }
            if (_CurSkillStruct.BuffAttrType2 > 0 && _CurSkillLevelStruct.BuffAttrValue2 > 0)
            {
                _DictTotalAttr[_CurSkillStruct.BuffAttrType2] += _CurSkillLevelStruct.BuffAttrValue2;
            }
            if (_CurSkillStruct.BuffAttrType3 > 0 && _CurSkillLevelStruct.BuffAttrValue3 > 0)
            {
                _DictTotalAttr[_CurSkillStruct.BuffAttrType3] += _CurSkillLevelStruct.BuffAttrValue3;
            }
            //HP
            double dCostHP0 = BattleHandler.GetSkillAttack(_DictTotalAttr, _CurSkill, _CurMonster);
            _CurMonster.HP -= dCostHP0;
            GameEventHandler.Messenger.DispatchEvent(EventConstant.BattleMessage, "[" + dTime + "]" + "角色技能，" + _CurSkillStruct.GetName() + "，怪物掉血" + dCostHP0);
            if (_CurMonster.HP <= 0)
            {
                Timers.inst.Remove(UpdateBattle);
                GameEventHandler.Messenger.DispatchEvent(EventConstant.BattleMessage, "[" + dTime + "]" + "怪物死亡，结算奖励");
                NetManager.Instance.MonsterIndexRequest(_CurMonster.Index, true);
                return false;
            }
            GameEventHandler.Messenger.DispatchEvent(EventConstant.MonsterHPUpdate, _CurMonster.HP);
        }
        return true;
    }

    /*
     * 结算怪物当前状态
     */
    private bool MonsterCalculateStatus(int iCurStatus)
    {
        if (iCurStatus == Constant.BATTLESTATUS0)
        {
        }
        else if (iCurStatus == Constant.BATTLESTATUS1)
        {
            double dCostHP = BattleHandler.GetMonsterAttack(_DictTotalAttr, _CurMonster);
            _DictTotalAttr[Constant.HP] -= dCostHP;
            GameEventHandler.Messenger.DispatchEvent(EventConstant.BattleMessage, "[" + dTime + "]" + "怪物普攻，角色掉血" + dCostHP);
            if (_DictTotalAttr[Constant.HP] <= 0)
            {
                Timers.inst.Remove(UpdateBattle);
                GameEventHandler.Messenger.DispatchEvent(EventConstant.BattleMessage, "[" + dTime + "]" + "角色死亡");
                NetManager.Instance.MonsterIndexRequest(_CurMonster.Index);
                return false;
            }
            GameEventHandler.Messenger.DispatchEvent(EventConstant.HPUpdate, _DictTotalAttr[Constant.HP]);
        }
        else if (iCurStatus == Constant.BATTLESTATUS2)
        {
            double dCostHP0 = BattleHandler.GetMonsterSkillAttack(_DictTotalAttr, _CurMonster, _CurMonsterSkill);
            _DictTotalAttr[Constant.HP] -= dCostHP0;
            GameEventHandler.Messenger.DispatchEvent(EventConstant.BattleMessage, "[" + dTime + "]" + "怪物技能，" + _CurMonsterSkill.Index + "，角色掉血" + dCostHP0);
            if (_DictTotalAttr[Constant.HP] <= 0)
            {
                Timers.inst.Remove(UpdateBattle);
                GameEventHandler.Messenger.DispatchEvent(EventConstant.BattleMessage, "[" + dTime + "]" + "角色死亡");
                NetManager.Instance.MonsterIndexRequest(_CurMonster.Index);
                return false;
            }
            GameEventHandler.Messenger.DispatchEvent(EventConstant.HPUpdate, _DictTotalAttr[Constant.HP]);
        }
        return true;
    }

    /*
     * 下一状态
     */
    private int NextStatus(int iCurStatus)
    {
        int iNextStatus = 0;
        if (iCurStatus == Constant.BATTLESTATUS0)
        {
            iNextStatus = Constant.BATTLESTATUS1;
        }
        else if (iCurStatus == Constant.BATTLESTATUS1)
        {
            if (_CurSkill != null && _CurSkillCD <= 0 && _DictTotalAttr[Constant.MP] >= _CurSkillLevelStruct.MPCost)
            {
                iNextStatus = Constant.BATTLESTATUS2;
            }
            else
            {
                iNextStatus = Constant.BATTLESTATUS1;
            }
        }
        else if (iCurStatus == Constant.BATTLESTATUS2)
        {
            iNextStatus = Constant.BATTLESTATUS1;
        }
        return iNextStatus;
    }

    /*
     * 怪物下一状态
     */
    private int MonsterNextStatus(int iCurStatus)
    {
        int iNextStatus = 0;
        if (iCurStatus == Constant.BATTLESTATUS0)
        {
            iNextStatus = Constant.BATTLESTATUS1;
        }
        else if (iCurStatus == Constant.BATTLESTATUS1)
        {
            if (_CurMonsterSkill.Index > 0 && _CurMonsterSkillCD <= 0 && _CurMonster.MP >= _CurMonsterSkill.MPCost)
            {
                iNextStatus = Constant.BATTLESTATUS2;
            }
            else
            {
                iNextStatus = Constant.BATTLESTATUS1;
            }
        }
        else if (iCurStatus == Constant.BATTLESTATUS2)
        {
            iNextStatus = Constant.BATTLESTATUS1;
        }
        return iNextStatus;
    }

    /*
     * 状态转换
     */
    private void StatusTransform(int iCurStatus, int iNextStatus)
    {
        switch (iNextStatus)
        {
            case Constant.BATTLESTATUS0:
                {
                    _DictTotalAttr = BattleHandler.CloneTotalAttr(DataManager.Instance.DictTotalAttr);
                    _MySkillList = SkillHandler.GetMySkillList();
                    _FireCD = Constant.FIRECD / _DictTotalAttr[Constant.FIRERATE];
                    _CurCountDown = 10 * Constant.SEARCHMONSTERTIME;
                    iMySkillIndex = 0;
                    break;
                }
            case Constant.BATTLESTATUS1:
                {
                    _CurCountDown = (int)(10 * Math.Round(_FireCD, 1, MidpointRounding.AwayFromZero));
                    if (iCurStatus != iNextStatus)
                    {
                        if (_MySkillList.Count > 0)
                        {
                            _CurSkill = _MySkillList[iMySkillIndex];
                            if (_CurSkill != null)
                            {
                                _CurSkillStruct = SkillConfig.Instance.GetSkill(_CurSkill.SkillID);
                                _CurSkillLevelStruct = _CurSkillStruct.GetSkillLevel(_CurSkill.Level);
                                _CurSkillCD = _CurSkillLevelStruct.CD;
                            }
                            iMySkillIndex ++;
                            if (iMySkillIndex >= _MySkillList.Count)
                            {
                                iMySkillIndex = 0;
                            }
                        }
                    }
                    break;
                }
            case Constant.BATTLESTATUS2:
                {
                    _CurCountDown = (int)(10 * Math.Round(_CurSkillLevelStruct.Sing / _DictTotalAttr[Constant.SINGRATE], 1, MidpointRounding.AwayFromZero));
                    _DictTotalAttr[Constant.MP] -= _CurSkillLevelStruct.MPCost;
                    GameEventHandler.Messenger.DispatchEvent(EventConstant.MPUpdate, _DictTotalAttr[Constant.MP]);
                    break;
                }
            default:
                return;
        }
    }

    /*
     * 怪物状态转换
     */
    private void MonsterStatusTransform(int iCurStatus, int iMonsterNextStatus)
    {
        switch (iMonsterNextStatus)
        {
            case Constant.BATTLESTATUS0:
                {
                    _CurMonster = MonsterConfig.Instance.GetMonster(DataManager.Instance.CurrentRole.MonsterIndex);
                    _CurMonsterSkillList = MonsterHandler.GetMonsterSkillList(DataManager.Instance.CurrentRole.MonsterIndex);
                    _CurMonsterCountDown = 10 * Constant.SEARCHMONSTERTIME;
                    iMonsterSkillIndex = 0;
                    break;
                }
            case Constant.BATTLESTATUS1:
                {
                    _CurMonsterCountDown = (int)(10 * Math.Round(Constant.FIRECD, 1, MidpointRounding.AwayFromZero));
                    if (iCurStatus != iMonsterNextStatus)
                    {
                        if (_CurMonsterSkillList.Count > 0)
                        {
                            _CurMonsterSkill = _CurMonsterSkillList[iMonsterSkillIndex];
                            if (_CurMonsterSkill.Index > 0)
                            {
                                _CurMonsterSkillCD = _CurMonsterSkill.CD;
                            }
                            iMonsterSkillIndex++;
                            if (iMonsterSkillIndex >= _CurMonsterSkillList.Count)
                            {
                                iMonsterSkillIndex = 0;
                            }
                        }
                    }
                    break;
                }
            case Constant.BATTLESTATUS2:
                {
                    _CurMonsterCountDown = (int)(10 * Math.Round(_CurMonsterSkill.Sing, 1, MidpointRounding.AwayFromZero));
                    _CurMonster.MP -= _CurMonsterSkill.MPCost;
                    GameEventHandler.Messenger.DispatchEvent(EventConstant.MonsterMPUpdate, _CurMonster.MP);
                    break;
                }
            default:
                return;
        }
    }
}