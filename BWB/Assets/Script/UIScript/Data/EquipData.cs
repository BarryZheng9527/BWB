using System.Collections.Generic;

public class EquipData
{
    public List<EquipClass> EquipList = new List<EquipClass>();
}

public class EquipClass
{
    public string UniqueID; //装备唯一ID
    public int EquipID; //装备ID
    public int EquipPos; //装备位置
    public int Level; //装备改造次数
    public List<int> RemouldOptionList = new List<int>(); //装备改造信息
}