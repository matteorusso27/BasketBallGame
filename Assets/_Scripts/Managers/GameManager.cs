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
    Win = 3,
    Lose = 4,
    Tie = 5,
    End = 6
}

[RequireComponent(typeof(ShootingPhase))]
public class GameManager : Singleton<GameManager>
{
    public ShootingPhase ShootingPhase;

    public Action<GameState> OnBeforeStateChanged;
    public Action<GameState> OnAfterStateChanged;
    public Action<int>       OnTimerChange;

    public GameState State { get; private set; }
    private const int shootingPhaseTimer = 50;
    private new void Awake()
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
            case GameState.Win:
            case GameState.Lose:
            case GameState.Tie:
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
        StartCoroutine(ShootingPhaseTimer(shootingPhaseTimer));
    }

    private IEnumerator ShootingPhaseTimer(int timer)
    {
        int currentTime = 0;
        while (currentTime < timer)
        {
            OnTimerChange?.Invoke(timer - currentTime++);
            yield return new WaitForSeconds(1f);
        }
        HandleEndShootingPhase();
    }

    private void HandleEndShootingPhase()
    {
        ShootingPhase.enabled = false;

        if (ScoreM.HasPlayerWon())
            GameM.ChangeState(GameState.Win);
        else
        {
            if (ScoreM.HasTie()) GameM.ChangeState(GameState.Tie);
            else
                GameM.ChangeState(GameState.Lose);
        }
    }

    private void HandleSpawning()
    {
        SpawnerM.SpawnBall(BallType.Regular, new Vector3(0,0,0), Faction.Player);
        SpawnerM.SpawnBall(BallType.Regular, new Vector3(1, 0, 0), Faction.Enemy);
        ScoreM.Init();
        ChangeState(GameState.ShootingPhase);
    }

    private void HandleStart()
    {
        ChangeState(GameState.SpawningInstances);
    }
}
