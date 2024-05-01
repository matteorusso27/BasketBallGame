using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static Selectors;
public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera;

    private void Start()
    {
        SwipeM.SwipeMeasured += (float a) => FollowPlayerBall();
        GameM.ShootingPhase.OnNewPlayerBall += () => FollowPlayerBall();
        SpawnerM.GetBallOfFaction(Faction.Player).OnScoreUpdate += () => VirtualCamera.Follow = null;
    }

    private void FollowPlayerBall() => VirtualCamera.Follow = SpawnerM.GetBallOfFaction(Faction.Player)?.transform;
}
