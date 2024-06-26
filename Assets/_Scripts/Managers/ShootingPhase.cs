using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Selectors;
using static Helpers;
using static GameSettings;

public enum ShootType
{
    Regular,
    Perfect,
    Board,
    Fail,
    COUNT
}
public class ShootingPhase : MonoBehaviour
{
    public ShootType PlayerShoot { get; private set; }
    public ShootType EnemyShoot { get; private set; }
    
    public bool IsBoardBlinking { get; private set; }

    public Action OnPlayerMissing;
    public Action OnNewPlayerBall;
    
    private void OnDisable()
    {
        SwipeM.SwipeMeasured -= HandlePlayerBallMovement;
    }
    public void Init()
    {
        SwipeM.SwipeMeasured += HandlePlayerBallMovement;
        ScoreM.OnBoardHit += () => { IsBoardBlinking = false; ChangeBoardBlinking(); };
        StartCoroutine(MoveEnemyBall());
        StartCoroutine(BoardBlinkingTimer());
    }

    private IEnumerator MoveEnemyBall()
    {
        while (GameM.State == GameState.ShootingPhase)
        {
            var enemyBall = SpawnerM.GetBallOfFaction(Faction.Enemy);
            EnemyShoot = (ShootType)GetRandomNumber(0, (int)ShootType.COUNT); // todo

            enemyBall.Move(GetFinalShootPosition(EnemyShoot));
            yield return new WaitForSeconds(3f);
            enemyBall.SetPosition(new Vector3(2, 0, 0));
        }
    }

    private IEnumerator BoardBlinkingTimer()
    {
        while (GameM.State == GameState.ShootingPhase)
        {
            IsBoardBlinking = GetRandomNumber(0, 10) > 6;
            ChangeBoardBlinking();
            yield return new WaitForSeconds(5f);
        }
    }

    private void ChangeBoardBlinking()
    {
        var hoop = GameObject.FindGameObjectWithTag(GameTagToString(GameTag.Hoop));
        hoop.GetComponent<Renderer>().material.color = IsBoardBlinking ? Color.blue : Color.white;
    }

    private void HandlePlayerBallMovement(float normalizedDistance)
    {
        if (GameM.State != GameState.ShootingPhase) return;

        var playerBall = SpawnerM.GetBallOfFaction(Faction.Player);
        PlayerShoot = GetShootType(normalizedDistance);

        playerBall.Move(GetFinalShootPosition(PlayerShoot));
        playerBall.OnBallGrounded += ResetPlayerBall;
    }

    private void SwitchBallTo(Ball b, BallType type)
    {
        SpawnerM.RemoveBallOfFaction(Faction.Player);
        Destroy(b.gameObject);
        SpawnerM.SpawnBall(type, new Vector3(0, 0, 0), Faction.Player);
        OnNewPlayerBall?.Invoke();
    }

    private void ResetPlayerBall(Ball ball)
    {
        ball.SetPosition(new Vector3(0, 0, 0));
        ball.OnBallGrounded -= ResetPlayerBall;
        SwipeM.HandleListener(GameM.State);
        if (PlayerShoot == ShootType.Fail) OnPlayerMissing?.Invoke();

        if (ScoreM.IsPlayerEnergyBarFull && IsRegularBall(ball))
        {
            SwitchBallTo(ball, BallType.FireBall);
        }
        else if (PlayerShoot == ShootType.Fail)
        {
            SwitchBallTo(ball, BallType.Regular);
        }
    }

    private Vector3 GetFinalShootPosition(ShootType st)
    {
        switch (st)
        {
            case ShootType.Perfect:
            case ShootType.Regular:
                return new Vector3(0, 0, 10);
            case ShootType.Board:
                return new Vector3(0, 4.78f, 7.74f);
            default:
                return new Vector3(0, 0, 3);
        }
    }

    private ShootType GetShootType(float normalizedDistance)
    {
        if (RangeM.IsInsidePerfectRange(normalizedDistance))
        {
            if (RangeM.IsRegularRange(normalizedDistance))
            {
                return ShootType.Regular;
            }
            else
            {
                return ShootType.Perfect;
            }
        }
        else if (RangeM.IsBoardShoot(normalizedDistance))
        {
            return ShootType.Board;
        }
        else
        {
            return ShootType.Fail;
        }
    }
}