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