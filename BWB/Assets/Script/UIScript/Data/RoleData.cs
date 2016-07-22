using System.Collections.Generic;

public class RoleData
{
    public List<RoleClass> RoleList;
}

public class RoleClass
{
    public string Name;
    public int Level;
    public int Job;
    public double Exp;
    public double Gold;
    public double Money;
    public double CreateTime;
    public double LastOffLineTime;
}