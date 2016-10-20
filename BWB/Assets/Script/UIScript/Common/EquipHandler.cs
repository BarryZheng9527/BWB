using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct EquipPosStrategyStruct
{
    public int iErrorID;

    public List<string> UniqueIDList;
    public List<int> EquipPosList;
}

public static class EquipHandler
{
    /*
     * 请求装备策略
     */
    static public EquipPosStrategyStruct GetEquipPosStrategy(EquipClass curEquip)
    {
        EquipPosStrategyStruct equipPosStrategy = new EquipPosStrategyStruct();
        equipPosStrategy.iErrorID = 0;
        equipPosStrategy.UniqueIDList = new List<string>();
        equipPosStrategy.EquipPosList = new List<int>();

        EquipStruct curEquipStruct = EquipConfig.Instance.GetEquipFromID(curEquip.EquipID);

        EquipClass lordEquip = GetEquipDataFromPos(Constant.EQUIPPOS1);
        EquipClass assistantEquip = GetEquipDataFromPos(Constant.EQUIPPOS2);
        EquipClass helmetEquip = GetEquipDataFromPos(Constant.EQUIPPOS3);
        EquipClass clothesEquip = GetEquipDataFromPos(Constant.EQUIPPOS4);
        EquipClass cloakEquip = GetEquipDataFromPos(Constant.EQUIPPOS5);
        EquipClass gloveEquip = GetEquipDataFromPos(Constant.EQUIPPOS6);
        EquipClass shoesEquip = GetEquipDataFromPos(Constant.EQUIPPOS7);
        EquipClass ringEquip = GetEquipDataFromPos(Constant.EQUIPPOS8);
        EquipClass necklaceEquip = GetEquipDataFromPos(Constant.EQUIPPOS9);

        EquipStruct lordEquipStruct = new EquipStruct();
        EquipStruct assistantEquipStruct = new EquipStruct();
        if (lordEquip != null)
        {
            lordEquipStruct = EquipConfig.Instance.GetEquipFromID(lordEquip.EquipID);
        }
        if (assistantEquip != null)
        {
            assistantEquipStruct = EquipConfig.Instance.GetEquipFromID(assistantEquip.EquipID);
        }

        switch (curEquipStruct.EquipType)
        {
            case Constant.BOW:
                {
                    if (assistantEquip != null && assistantEquipStruct.EquipType != Constant.DORLACH)
                    {
                        equipPosStrategy.UniqueIDList.Add(assistantEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    if (lordEquip != null)
                    {
                        equipPosStrategy.UniqueIDList.Add(lordEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                    equipPosStrategy.EquipPosList.Add(curEquipStruct.EquipPos);
                    break;
                }
            case Constant.WAND:
                {
                    if (assistantEquip != null && assistantEquipStruct.EquipType != Constant.SPELLBOOK)
                    {
                        equipPosStrategy.UniqueIDList.Add(assistantEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    if (lordEquip != null)
                    {
                        equipPosStrategy.UniqueIDList.Add(lordEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                    equipPosStrategy.EquipPosList.Add(curEquipStruct.EquipPos);
                    break;
                }
            case Constant.TWOHANDED:
                {
                    if (assistantEquip != null)
                    {
                        equipPosStrategy.UniqueIDList.Add(assistantEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    if (lordEquip != null)
                    {
                        equipPosStrategy.UniqueIDList.Add(lordEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                    equipPosStrategy.EquipPosList.Add(curEquipStruct.EquipPos);
                    break;
                }
            case Constant.ONEHANDED:
                {
                    if (assistantEquip != null && (assistantEquipStruct.EquipType == Constant.DORLACH || assistantEquipStruct.EquipType == Constant.SPELLBOOK))
                    {
                        equipPosStrategy.UniqueIDList.Add(assistantEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    if (lordEquip != null && (lordEquipStruct.EquipType != Constant.ONEHANDED || assistantEquip != null))
                    {
                        equipPosStrategy.UniqueIDList.Add(lordEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                    if (lordEquip != null && lordEquipStruct.EquipType == Constant.ONEHANDED && assistantEquip == null)
                    {
                        equipPosStrategy.EquipPosList.Add(Constant.EQUIPPOS2);
                    }
                    else
                    {
                        equipPosStrategy.EquipPosList.Add(Constant.EQUIPPOS1);
                    }
                    break;
                }
            case Constant.DORLACH:
                {
                    if (lordEquip != null && lordEquipStruct.EquipType == Constant.BOW)
                    {
                        if (assistantEquip != null)
                        {
                            equipPosStrategy.UniqueIDList.Add(assistantEquip.UniqueID);
                            equipPosStrategy.EquipPosList.Add(0);
                        }
                        equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(curEquipStruct.EquipPos);
                    }
                    else
                    {
                        equipPosStrategy.iErrorID = ErrorConstant.ERROR_100008;
                    }
                    break;
                }
            case Constant.SPELLBOOK:
                {
                    if (lordEquip != null && lordEquipStruct.EquipType == Constant.WAND)
                    {
                        if (assistantEquip != null)
                        {
                            equipPosStrategy.UniqueIDList.Add(assistantEquip.UniqueID);
                            equipPosStrategy.EquipPosList.Add(0);
                        }
                        equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(curEquipStruct.EquipPos);
                    }
                    else
                    {
                        equipPosStrategy.iErrorID = ErrorConstant.ERROR_100009;
                    }
                    break;
                }
            case Constant.SHIELD:
                {
                    if (lordEquip != null && (lordEquipStruct.EquipType != Constant.ONEHANDED))
                    {
                        equipPosStrategy.iErrorID = ErrorConstant.ERROR_100010;
                    }
                    else
                    {
                        if (assistantEquip != null)
                        {
                            equipPosStrategy.UniqueIDList.Add(assistantEquip.UniqueID);
                            equipPosStrategy.EquipPosList.Add(0);
                        }
                        equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(curEquipStruct.EquipPos);
                    }
                    break;
                }
            case Constant.HELMET:
                {
                    if (helmetEquip != null)
                    {
                        equipPosStrategy.UniqueIDList.Add(helmetEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                    equipPosStrategy.EquipPosList.Add(curEquipStruct.EquipPos);
                    break;
                }
            case Constant.CLOTHES:
                {
                    if (clothesEquip != null)
                    {
                        equipPosStrategy.UniqueIDList.Add(clothesEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                    equipPosStrategy.EquipPosList.Add(curEquipStruct.EquipPos);
                    break;
                }
            case Constant.CLOAK:
                {
                    if (cloakEquip != null)
                    {
                        equipPosStrategy.UniqueIDList.Add(cloakEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                    equipPosStrategy.EquipPosList.Add(curEquipStruct.EquipPos);
                    break;
                }
            case Constant.GLOVE:
                {
                    if (gloveEquip != null)
                    {
                        equipPosStrategy.UniqueIDList.Add(gloveEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                    equipPosStrategy.EquipPosList.Add(curEquipStruct.EquipPos);
                    break;
                }
            case Constant.SHOES:
                {
                    if (shoesEquip != null)
                    {
                        equipPosStrategy.UniqueIDList.Add(shoesEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                    equipPosStrategy.EquipPosList.Add(curEquipStruct.EquipPos);
                    break;
                }
            case Constant.RING:
                {
                    if (ringEquip != null)
                    {
                        equipPosStrategy.UniqueIDList.Add(ringEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                    equipPosStrategy.EquipPosList.Add(curEquipStruct.EquipPos);
                    break;
                }
            case Constant.NECKLACE:
                {
                    if (necklaceEquip != null)
                    {
                        equipPosStrategy.UniqueIDList.Add(necklaceEquip.UniqueID);
                        equipPosStrategy.EquipPosList.Add(0);
                    }
                    equipPosStrategy.UniqueIDList.Add(curEquip.UniqueID);
                    equipPosStrategy.EquipPosList.Add(curEquipStruct.EquipPos);
                    break;
                }
            default:
                break;
        }

        return equipPosStrategy;
    }

    /*
     * 获取装备槽位上当前的装备
     */
    static public EquipClass GetEquipDataFromPos(int iEquipPos)
    {
        for (int iIndex = 0; iIndex < DataManager.Instance.EquipData.EquipList.Count; ++iIndex)
        {
            EquipClass equip = DataManager.Instance.EquipData.EquipList[iIndex];
            if (equip.EquipPos == iEquipPos)
            {
                return equip;
            }
        }
        return null;
    }
}