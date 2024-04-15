using System;

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
    public void Start() => ChangeState(GameState.Start);
    public void StartGame() => ChangeState(GameState.Start);
    private void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);
        State = newState;
        switch (newState)
        {
            case GameState.Start:
                break;
            case GameState.SpawningInstances:
                break;
            case GameState.ShootingPhase:
                break;
            case GameState.End:
                break;
        }
        OnAfterStateChanged?.Invoke(newState);
    }
}
