using UnityEngine;
using static GameSettings;

public class ScriptableBall : ScriptableObject
{
    public BallType Type;

    public Faction Faction;

    public Ball Prefab;
    public Material Material;
}

public enum Faction
{
    Player,
    Enemy
}
