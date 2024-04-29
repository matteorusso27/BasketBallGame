using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Selectors;
using static Helpers;
public class ShootingPhase : MonoBehaviour
{
    public enum ShootType
    {
        Regular,
        Perfect,
        Board,
        Fail,
        COUNT
    }

    public ShootType PlayerShoot { get; private set; }
    public ShootType EnemyShoot { get; private set; }
    
    private void OnDisable()
    {
        SwipeM.SwipeMeasured -= HandlePlayerBallMovement;
    }
    public void Init()
    {
        SwipeM.SwipeMeasured += HandlePlayerBallMovement;
        StartCoroutine(MoveEnemyBall());
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

    private void HandlePlayerBallMovement(float normalizedDistance)
    {
        if (GameM.State != GameState.ShootingPhase) return;

        var playerBall = SpawnerM.GetBallOfFaction(Faction.Player);
        PlayerShoot = GetShootType(normalizedDistance);
        
        playerBall.Move(GetFinalShootPosition(PlayerShoot));
        playerBall.OnBallGrounded += ResetPlayerBall;
    }

    private void ResetPlayerBall(Ball ball)
    {
        ball.SetPosition(new Vector3(0, 0, 0));
        SwipeM.HandleListener(GameM.State);
        ball.OnBallGrounded -= ResetPlayerBall;
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
}