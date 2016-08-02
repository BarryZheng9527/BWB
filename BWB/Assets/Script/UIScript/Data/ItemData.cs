using System.Collections.Generic;

public class ItemData
{
    public List<ItemClass> ItemList;
}

public class ItemClass
{
    public int ItemType;

    public string UniqueID;
    public int EquipID;
    public int EquipPos;
    public int Level;
    public List<int> RemouldOptionList = new List<int>();

    public int ItemID;
    public int Count;
}