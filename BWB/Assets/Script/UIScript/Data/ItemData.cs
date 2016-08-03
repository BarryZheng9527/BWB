using System.Collections.Generic;

public class ItemData
{
    public List<ItemClass> ItemList;
}

public class ItemClass
{
    public int ItemType; //类型1装备，2道具

    public string UniqueID; //装备唯一ID
    public int EquipID; //装备ID
    public int EquipPos; //装备位置
    public int Level; //装备改造次数
    public List<int> RemouldOptionList = new List<int>(); //装备改造信息

    public int ItemID; //道具ID
    public int Count; //道具个数
}