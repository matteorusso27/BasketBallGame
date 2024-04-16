using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameSettings;
public class ResourceSystem : Singleton<ResourceSystem>
{
    public List<ScriptableBall> Balls { get; private set; }
    private Dictionary<BallType, ScriptableBall> _BallsDictionary;

    protected override void Awake()
    {
        base.Awake();
        AssembleResources();
    }

    private void AssembleResources()
    {
        Balls = Resources.LoadAll<ScriptableBall>("BallsPrototypes").ToList();
        _BallsDictionary = Balls.ToDictionary(t => t.Type);
    }

    public ScriptableBall GetBalls(BallType e) => _BallsDictionary[e];
}
