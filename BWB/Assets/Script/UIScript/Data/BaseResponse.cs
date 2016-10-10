public class BaseResponse
{
    public int iResponseId = 0;
}

public class LoginResponse:BaseResponse
{
    public string name;
    public string password;
}

public class RegisterResponse : BaseResponse
{
    public string name;
    public string password;
}