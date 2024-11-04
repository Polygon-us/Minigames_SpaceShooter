using UnityEngine;

[CreateAssetMenu(fileName = "NakamaConfig", menuName = "Nakama/Config/NakamaConfig")]
public class NakamaConfig : ScriptableObject
{
    public string scheme = "http";
    public string host = "127.0.0.1";
    public int port = 7350;
    public string serverKey = "defaultkey";
}
