using System;
using static Selectors;
using static GameSettings;
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
                break;
            case GameState.End:
                break;
        }
        OnAfterStateChanged?.Invoke(newState);
    }

    private void HandleSpawning()
    {
        SpawnerM.SpawnBall(BallType.Regular);
        SpawnerM.SpawnBall(BallType.FireBall);
    }

    private void HandleStart()
    {
        ChangeState(GameState.SpawningInstances);
    }
}
