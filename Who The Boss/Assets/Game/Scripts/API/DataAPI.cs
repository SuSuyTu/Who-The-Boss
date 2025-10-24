using System;

public class DataAPI
{
}

[Serializable]
public class Users
{
    public int id;
    public string username;
    public string password;
    public string email;

    public Users(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

[Serializable]
public class LoginResponse
{
    public bool success;
    public string message;
    public string error;
    public string errorType;
    public Users user;
}
