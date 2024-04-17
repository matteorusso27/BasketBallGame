using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static GameSettings;
public class SpawnerManager : Singleton<SpawnerManager>
{
    public void SpawnBall(BallType t, Vector3 position, Faction faction)
    {
        var ballScriptable = ResourceSystem.Instance.GetBalls(t);
        var toSpawn = Instantiate(ballScriptable.Prefab, position, Quaternion.identity).gameObject;
        toSpawn.GetComponent<Renderer>().material = ballScriptable.Material;
        toSpawn.GetComponent<Ball>().BallType = t;
        toSpawn.GetComponent<Ball>().Faction = faction;
        spawnedObjects.Add(toSpawn);
    }

    private List<GameObject> spawnedObjects = new List<GameObject>();

    public List<GameObject> SpawnedObjects { get => spawnedObjects; }

    public List<Ball> SpawnedBalls()
    {
        var gos = spawnedObjects.Where(x => x.GetComponent<Ball>() != null).ToList();
        return gos.Select(x=> x.GetComponent<Ball>()).ToList();
    }
    public Ball GetBallOfFaction(Faction faction)
    {
        var go = SpawnedBalls().Where(x => x.GetComponent<Ball>().Faction == faction).FirstOrDefault();
        return go.GetComponent<Ball>();
    }
}
