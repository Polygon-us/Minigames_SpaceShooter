using UnityEngine;

public class Account
{
    public string username { get; private set; }

    public Account(string username)
    {
        this.username = username;
    }
}
