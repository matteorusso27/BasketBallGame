using System;
using UnityEngine;
using static Selectors;
using static GameSettings;
using System.Collections;

public enum GameState
{
    Start = 0,
    SpawningInstances = 1,
    ShootingPhase = 2,
    End = 3
}

[RequireComponent(typeof(ShootingPhase))]
public class GameManager : Singleton<GameManager>
{
    private ShootingPhase ShootingPhase;

    public Action<GameState> OnBeforeStateChanged;
    public Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; }

    private void Awake()
    {
        ShootingPhase = GetComponent<ShootingPhase>();
        ShootingPhase.enabled = false;
        base.Awake();
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
        ShootingPhase.enabled = true;
        ShootingPhase.Init();
        StartCoroutine(ChangeStateAfter(50f, GameState.End));
    }

    private IEnumerator ChangeStateAfter(float timer, GameState newState)
    {
        yield return new WaitForSeconds(timer);
        GameM.ChangeState(newState);
        ShootingPhase.enabled = false;
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
}
