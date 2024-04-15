using UnityEngine;
using static GameSettings;

public class ScriptableBall : ScriptableObject
{
    public BallType Type;

    public Faction faction;

    public Ball prefab;
    public Texture texture;
}

public enum Faction
{
    Player,
    Enemy
}
