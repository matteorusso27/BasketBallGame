using System;
using System.Collections.Generic;
using UnityEngine;
using static Selectors;
public class ScoreManager : Singleton<ScoreManager>
{
    public Action<int> PlayerScoreUpdate;
    public Action<int> EnemyScoreUpdate;

    private int playerScore;
    private int enemyScore;
    public void Init()
    {
        var playerBall = SpawnerM.GetBallOfFaction(Faction.Player);
        var enemyBall = SpawnerM.GetBallOfFaction(Faction.Enemy);

        playerBall.OnScoreUpdate += UpdatePlayerScore;
        enemyBall.OnScoreUpdate += UpdateEnemyScore;

        PlayerScoreUpdate?.Invoke(0);
        EnemyScoreUpdate?.Invoke(0);
    }

    public void UpdatePlayerScore()
    {
        var score = GameM.ShootingPhase.PlayerShoot == ShootingPhase.ShootType.Perfect ? 3 : 2;
        playerScore += score;
        PlayerScoreUpdate?.Invoke(playerScore);
    }

    public void UpdateEnemyScore()
    {
        var score = GameM.ShootingPhase.EnemyShoot == ShootingPhase.ShootType.Perfect ? 3 : 2;
        enemyScore += score;
        EnemyScoreUpdate?.Invoke(enemyScore);
    }

    private void OnDisable()
    {
        var playerBall = SpawnerM.GetBallOfFaction(Faction.Player);
        var enemyBall = SpawnerM.GetBallOfFaction(Faction.Enemy);

        playerBall.OnScoreUpdate -= UpdatePlayerScore;
        enemyBall.OnScoreUpdate -= UpdateEnemyScore;
    }
}
