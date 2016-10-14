public class BaseResponse
{
    public int iResponseId = 0;
    public int ID;
}

public class CheckRegisterResponse : BaseResponse
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