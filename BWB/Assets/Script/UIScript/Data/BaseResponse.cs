public class BaseResponse
{
    public int iResponseId = 0;
    public int ID;
}

public class CheckUserResponse : BaseResponse
{
    public string name;
    public string password;
}

public class RegisterResponse : BaseResponse
{
    public string uid;
    public string name;
    public string password;
}

public class LoginResponse:BaseResponse
{
    public string name;
    public string password;
}

public class CreatRoleResponse : BaseResponse
{
    public RoleClass role;
}

public class EquipResponse : BaseResponse
{
    public EquipPosStrategyStruct equipPosStrategy;
}

public class UnEquipResponse : BaseResponse
{
    public string uniqueId;
}

public class RemouldEquipResponse : BaseResponse
{
    public string uniqueId;
    public int optionIndex;
}

public class EquipNotify : BaseResponse
{
    public EquipClass equip;
}

public class ItemNotify : BaseResponse
{
    public ItemClass item;
}

public class SkillGetResponse : BaseResponse
{
    public SkillClass skill;
}

public class SkillLevelUpResponse : BaseResponse
{
    public string uniqueId;
}

public class SkillEquipResponse : BaseResponse
{
    public string equipUniqueId;
    public int iPos;
    public string unEquipUniqueId;
}

public class MonsterIndexResponse : BaseResponse
{
    public int curMonsterIndex;
    public int nextMonsterIndex;
}

public class GoldNotify : BaseResponse
{
    public double addGold;
}

public class ExpNotify : BaseResponse
{
    public double addExp;
    public int level;
}