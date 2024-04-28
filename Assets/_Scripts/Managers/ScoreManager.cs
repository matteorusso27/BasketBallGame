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

        UpdatePlayerScore(0);
        UpdateEnemyScore(0);
    }

    public void UpdatePlayerScore(int score)
    {
        playerScore += score;
        PlayerScoreUpdate?.Invoke(playerScore);
    }

    public void UpdateEnemyScore(int score)
    {
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
