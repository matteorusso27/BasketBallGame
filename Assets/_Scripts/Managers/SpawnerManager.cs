using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static GameSettings;
public class SpawnerManager : Singleton<SpawnerManager>
{
    public GameObject SpawnBall(BallType t)
    {
        var ballPosition = new Vector3(0,0,0);
        var ballScriptable = ResourceSystem.Instance.GetBalls(t);
        var toSpawn = Instantiate(ballScriptable.Prefab, ballPosition, Quaternion.identity).gameObject;
        toSpawn.GetComponent<Renderer>().material = ballScriptable.Material;
        return toSpawn;
    }
}