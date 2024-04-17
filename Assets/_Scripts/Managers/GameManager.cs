using System;
using UnityEngine;
using static Selectors;
using static GameSettings;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    public Action<GameState> OnBeforeStateChanged;
    public Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; }

    public enum GameState
    {
        Start = 0,
        SpawningInstances = 1,
        ShootingPhase = 2,
        End = 3
    }
    private void OnEnable()
    {
        SwipeM.SwipeMeasured += HandlePlayerBallMovement;
    }
    public void Start()
    {
        ChangeState(GameState.Start);
    }
    private void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);
        State = newState;
        switch (newState)
        {
            case GameState.Start:
                HandleStart();
                break;
            case GameState.SpawningInstances:
                HandleSpawning();
                break;
            case GameState.ShootingPhase:
                HandleShooting();
                break;
            case GameState.End:
                break;
        }
        OnAfterStateChanged?.Invoke(newState);
    }

    private void HandleShooting()
    {
        StartCoroutine(ChangeStateAfter(10f, GameState.End));
        StartCoroutine(MoveEnemyBall());
    }

    private IEnumerator ChangeStateAfter(float timer, GameState newState)
    {
        yield return new WaitForSeconds(timer);
        ChangeState(newState);
    }
    private IEnumerator MoveEnemyBall()
    {
        while (State == GameState.ShootingPhase)
        {
            var enemyBall = SpawnerM.GetBallOfFaction(Faction.Enemy);
            var finalPosition = new Vector3(2, 0, 10);
            enemyBall.Move(finalPosition);
            yield return new WaitForSeconds(5f);
            enemyBall.SetPosition(new Vector3(2, 0, 0));
        }
    }

    private void HandleSpawning()
    {
        SpawnerM.SpawnBall(BallType.Regular, new Vector3(0,0,0), Faction.Player);
        SpawnerM.SpawnBall(BallType.Regular, new Vector3(1, 0, 0), Faction.Enemy);
        ChangeState(GameState.ShootingPhase);
    }

    private void HandleStart()
    {
        ChangeState(GameState.SpawningInstances);
    }

    private void HandlePlayerBallMovement(float normalizedDistance)
    {
        if (State != GameState.ShootingPhase) return;
        var playerBall = SpawnerM.GetBallOfFaction(Faction.Player);
        playerBall.SetPosition(new Vector3(0, 0, 0));
        var finalPosition = new Vector3(0, 0, 10);
        playerBall.Move(finalPosition);
    }
}
