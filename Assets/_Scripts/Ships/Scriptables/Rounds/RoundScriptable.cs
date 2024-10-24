using UnityEngine;

[System.Serializable]
public struct Group
{
    public bool chained;
    public float timeToStart;

    public enum SpawnType { random, row, only, allAtOnce }
    [Space]
    public SpawnType spawnType;
    public float minTimeBetweenSpawn;
    public float maxTimeBetweenSpawn;

    [Space]
    public int count;
    public ShipScriptable ship;
}

[CreateAssetMenu(fileName = "Round ", menuName = "Gameplay/Round", order = 1)]
public class RoundScriptable : ScriptableObject
{
    public Group[] groups;
}