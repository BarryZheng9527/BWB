using System.Collections.Generic;

public class ItemData
{
    public List<ItemClass> ItemList = new List<ItemClass>();
}

public class ItemClass
{
    public string UniqueID; //道具唯一ID
    public int ItemID; //道具ID
    public int Count; //道具个数
}