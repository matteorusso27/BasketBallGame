using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Selectors;
public class ShootingPhase : MonoBehaviour
{
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
            var finalPosition = new Vector3(2, 0, 10);
            enemyBall.Move(finalPosition);
            yield return new WaitForSeconds(5f);
            enemyBall.SetPosition(new Vector3(2, 0, 0));
        }
    }

    private void HandlePlayerBallMovement(float normalizedDistance)
    {
        void ResetPlayerBall(Ball ball)
        {
            ball.SetPosition(new Vector3(0, 0, 0));
            SwipeM.HandleListener(GameM.State);
            ball.OnBallGrounded -= ResetPlayerBall;
        }
        if (GameM.State != GameState.ShootingPhase) return;
        var playerBall = SpawnerM.GetBallOfFaction(Faction.Player);
        playerBall.SetPosition(new Vector3(0, 0, 0));
        var finalPosition = new Vector3(0, 0, 10);
        playerBall.Move(finalPosition);
        playerBall.OnBallGrounded += ResetPlayerBall;
    }
}