using System;
using System.Collections.Generic;
using UnityEngine;
using static Selectors;
using static ShootingPhase;
using static Helpers;

public class ScoreManager : Singleton<ScoreManager>
{
    public Action<int,int> PlayerScoreUpdate;
    public Action<int> EnemyScoreUpdate;
    public Action      OnBoardHit;

    private int playerScore;
    private int enemyScore;
    private int playerEnergyBar;
    public ShootingPhase ShootingPhase;
    public void Init()
    {
        var playerBall = SpawnerM.GetBallOfFaction(Faction.Player);
        var enemyBall = SpawnerM.GetBallOfFaction(Faction.Enemy);
        ShootingPhase = GameM.ShootingPhase;

        ShootingPhase.OnPlayerMissing += ResetPlayerEnergyBar;

        playerBall.OnScoreUpdate += UpdatePlayerScore;
        enemyBall.OnScoreUpdate += UpdateEnemyScore;

        playerEnergyBar = 0;
        PlayerScoreUpdate?.Invoke(0, 0);
        EnemyScoreUpdate?.Invoke(0);
    }

    public int GetScore(ShootType type)
    {
        var shootType = type;
        int score;
        if (shootType == ShootType.Perfect)
        {
            score = 3;
        }
        else
        {
            if (shootType == ShootType.Board && ShootingPhase.IsBoardBlinking)
                score = GetRandomNumber(4, 5);
            else
                score = 2;
        }
        return score;
    }
    public void UpdatePlayerScore()
    {
        var score = GetScore(GameM.ShootingPhase.PlayerShoot);
        playerScore += score;
        PlayerScoreUpdate?.Invoke(playerScore, ++playerEnergyBar);
    }

    public void UpdateEnemyScore()
    {
        var score = GetScore(GameM.ShootingPhase.EnemyShoot);
        enemyScore += score;
        EnemyScoreUpdate?.Invoke(enemyScore);
    }

    public void ResetPlayerEnergyBar()
    {
        playerEnergyBar = 0;
        PlayerScoreUpdate?.Invoke(playerScore, playerEnergyBar);
    }
    private void OnDisable()
    {
        var playerBall = SpawnerM.GetBallOfFaction(Faction.Player);
        var enemyBall = SpawnerM.GetBallOfFaction(Faction.Enemy);

        playerBall.OnScoreUpdate -= UpdatePlayerScore;
        enemyBall.OnScoreUpdate -= UpdateEnemyScore;
    }
}
